#!/bin/bash
set -e
set -o pipefail

echo "========================================"
echo "Tour Management - Docker Build & Push"
echo "========================================"
echo ""

# Project configuration
PROJECT_NAME="tour-management"
DOCKERFILE_PATH="Dockerfile"

# Sanitize image name
IMAGE_NAME=$(echo "$PROJECT_NAME" | tr '[:upper:]' '[:lower:]' | tr -cs 'a-z0-9' '-' | sed 's/^-*//;s/-*$//')

echo "Select registry type:"
echo "1. AWS ECR (Elastic Container Registry)"
echo "2. Docker Hub"
read -p "Enter choice (1 or 2): " REGISTRY_CHOICE
echo ""

if [ "$REGISTRY_CHOICE" = "1" ]; then
    echo "--- AWS ECR Configuration ---"
    read -p "Enter AWS Region (e.g., us-east-1): " AWS_REGION
    read -p "Enter AWS Account ID: " AWS_ACCOUNT_ID
    read -p "Enter ECR Repository Name [$IMAGE_NAME]: " ECR_REPO
    ECR_REPO=${ECR_REPO:-$IMAGE_NAME}
    
    REGISTRY_URL="${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_REGION}.amazonaws.com"
    FULL_IMAGE_BASE="${REGISTRY_URL}/${ECR_REPO}"
    
    echo ""
    echo "Authenticating with AWS ECR..."
    aws ecr get-login-password --region "$AWS_REGION" | docker login --username AWS --password-stdin "$REGISTRY_URL"
    
    if [ $? -ne 0 ]; then
        echo "ERROR: ECR authentication failed"
        exit 1
    fi
    
    echo "Checking if ECR repository exists..."
    aws ecr describe-repositories --repository-names "$ECR_REPO" --region "$AWS_REGION" >/dev/null 2>&1 || {
        echo "Repository does not exist. Creating ECR repository: $ECR_REPO"
        aws ecr create-repository --repository-name "$ECR_REPO" --region "$AWS_REGION"
        echo "ECR repository created successfully"
    }
    
elif [ "$REGISTRY_CHOICE" = "2" ]; then
    echo "--- Docker Hub Configuration ---"
    read -p "Enter Docker Hub username: " DOCKER_USERNAME
    read -sp "Enter Docker Hub password/token: " DOCKER_PASSWORD
    echo ""
    
    FULL_IMAGE_BASE="${DOCKER_USERNAME}/${IMAGE_NAME}"
    
    echo "Authenticating with Docker Hub..."
    echo "$DOCKER_PASSWORD" | docker login --username "$DOCKER_USERNAME" --password-stdin
    
    if [ $? -ne 0 ]; then
        echo "ERROR: Docker Hub authentication failed"
        exit 1
    fi
else
    echo "ERROR: Invalid choice. Please select 1 or 2."
    exit 1
fi

echo ""
read -p "Enter image tag [latest]: " IMAGE_TAG
IMAGE_TAG=${IMAGE_TAG:-latest}

# Sanitize tag
IMAGE_TAG=$(echo "$IMAGE_TAG" | tr '[:upper:]' '[:lower:]' | tr -cs 'a-z0-9.-' '-' | sed 's/^-*//;s/-*$//')
IMAGE_TAG=${IMAGE_TAG:-latest}

FULL_IMAGE_NAME="${FULL_IMAGE_BASE}:${IMAGE_TAG}"

echo ""
echo "========================================"
echo "Build Configuration:"
echo "  Image Name: $FULL_IMAGE_NAME"
echo "  Dockerfile: $DOCKERFILE_PATH"
echo "========================================"
echo ""

read -p "Proceed with build? (y/n): " CONFIRM
if [ "$CONFIRM" != "y" ] && [ "$CONFIRM" != "Y" ]; then
    echo "Build cancelled."
    exit 0
fi

echo ""
echo "Building Docker image..."
docker build -f "$DOCKERFILE_PATH" -t "$FULL_IMAGE_NAME" .

if [ $? -ne 0 ]; then
    echo "ERROR: Docker build failed"
    exit 1
fi

echo ""
echo "Pushing image to registry..."
docker push "$FULL_IMAGE_NAME"

if [ $? -ne 0 ]; then
    echo "ERROR: Docker push failed"
    exit 1
fi

echo ""
echo "========================================"
echo "SUCCESS!"
echo "Image pushed: $FULL_IMAGE_NAME"
echo "========================================"
echo ""
echo "Next steps:"
echo "1. Update kubernetes/deployment.yaml with image URI"
echo "2. Run ./scripts/deploy-image.sh to deploy to EKS"
echo ""