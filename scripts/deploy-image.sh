#!/bin/bash
set -e
set -o pipefail

echo "========================================"
echo "Tour Management - AWS EKS Deployment"
echo "========================================"
echo ""

# Configuration
NAMESPACE="tour-management"
APP_NAME="tour-management"

echo "--- AWS EKS Configuration ---"
read -p "Enter AWS Region (e.g., us-east-1): " AWS_REGION
read -p "Enter EKS Cluster Name: " CLUSTER_NAME
echo ""

read -p "Enter Docker Image URI (with tag): " IMAGE_URI
if [ -z "$IMAGE_URI" ]; then
    echo "ERROR: Image URI is required"
    exit 1
fi
echo ""

echo "--- Database Configuration ---"
read -p "Enter Database Server (DB_SERVER): " DB_SERVER
read -p "Enter Database Name (DB_NAME) [tourdb]: " DB_NAME
DB_NAME=${DB_NAME:-tourdb}
read -p "Enter Database User (DB_USER): " DB_USER
read -sp "Enter Database Password (DB_PASSWORD): " DB_PASSWORD
echo ""
echo ""

if [ -z "$DB_SERVER" ] || [ -z "$DB_USER" ] || [ -z "$DB_PASSWORD" ]; then
    echo "ERROR: Database configuration is required"
    exit 1
fi

echo "Configuring kubectl for EKS cluster..."
aws eks update-kubeconfig --region "$AWS_REGION" --name "$CLUSTER_NAME"

if [ $? -ne 0 ]; then
    echo "ERROR: Failed to configure kubectl"
    exit 1
fi

echo "Verifying cluster connectivity..."
kubectl cluster-info

if [ $? -ne 0 ]; then
    echo "ERROR: Cannot connect to cluster"
    exit 1
fi

echo ""
echo "Updating Kubernetes manifests..."

# Create temporary directory for processed manifests
TMP_DIR=$(mktemp -d)
trap "rm -rf $TMP_DIR" EXIT

cp -r kubernetes "$TMP_DIR/"

# Replace placeholders in manifests
sed -i "s|{{IMAGE_URI}}|$IMAGE_URI|g" "$TMP_DIR/kubernetes/deployment.yaml"
sed -i "s|{{DB_SERVER}}|$DB_SERVER|g" "$TMP_DIR/kubernetes/deployment.yaml"
sed -i "s|{{DB_NAME}}|$DB_NAME|g" "$TMP_DIR/kubernetes/deployment.yaml"
sed -i "s|{{DB_USER}}|$DB_USER|g" "$TMP_DIR/kubernetes/deployment.yaml"
sed -i "s|{{DB_PASSWORD}}|$DB_PASSWORD|g" "$TMP_DIR/kubernetes/deployment.yaml"

echo "Manifest placeholders updated successfully"
echo ""

echo "========================================"
echo "Deployment Configuration:"
echo "  Namespace: $NAMESPACE"
echo "  Application: $APP_NAME"
echo "  Image: $IMAGE_URI"
echo "  Database: $DB_SERVER/$DB_NAME"
echo "========================================"
echo ""

read -p "Proceed with deployment? (y/n): " CONFIRM
if [ "$CONFIRM" != "y" ] && [ "$CONFIRM" != "Y" ]; then
    echo "Deployment cancelled."
    exit 0
fi

echo ""
echo "Creating namespace..."
kubectl apply -f "$TMP_DIR/kubernetes/namespace.yaml"

echo "Deploying application..."
kubectl apply -f "$TMP_DIR/kubernetes/deployment.yaml"

echo "Creating service..."
kubectl apply -f "$TMP_DIR/kubernetes/service.yaml"

echo "Creating ingress..."
kubectl apply -f "$TMP_DIR/kubernetes/ingress.yaml"

echo ""
echo "Waiting for deployment rollout..."
kubectl rollout status deployment/$APP_NAME -n $NAMESPACE --timeout=300s

if [ $? -ne 0 ]; then
    echo "ERROR: Deployment rollout failed"
    echo "Checking pod status..."
    kubectl get pods -n $NAMESPACE
    echo ""
    echo "Checking recent events..."
    kubectl get events -n $NAMESPACE --sort-by='.lastTimestamp' | tail -20
    exit 1
fi

echo ""
echo "========================================"
echo "Deployment Status:"
echo "========================================"
kubectl get pods,svc,ingress -n $NAMESPACE

echo ""
echo "========================================"
echo "SUCCESS!"
echo "========================================"
echo ""

echo "Application deployed successfully to EKS cluster: $CLUSTER_NAME"
echo ""
echo "To access the application:"
echo "1. Get the ingress URL:"
echo "   kubectl get ingress -n $NAMESPACE"
echo ""
echo "2. View logs:"
echo "   kubectl logs -f deployment/$APP_NAME -n $NAMESPACE"
echo ""
echo "3. Check pod status:"
echo "   kubectl get pods -n $NAMESPACE"
echo ""
echo "4. Describe deployment:"
echo "   kubectl describe deployment $APP_NAME -n $NAMESPACE"
echo ""