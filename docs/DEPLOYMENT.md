# Tour Management Application - Deployment Guide

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Local Development Setup](#local-development-setup)
3. [Docker Deployment](#docker-deployment)
4. [AWS EKS Deployment](#aws-eks-deployment)
5. [Configuration Management](#configuration-management)
6. [Troubleshooting](#troubleshooting)
7. [Security Considerations](#security-considerations)

---

## Prerequisites

### Required Tools
- **.NET 8.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Docker Desktop** - [Download](https://www.docker.com/products/docker-desktop)
- **AWS CLI v2** - [Installation Guide](https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html)
- **kubectl** - [Installation Guide](https://kubernetes.io/docs/tasks/tools/)
- **Git** - [Download](https://git-scm.com/downloads)

### AWS Requirements
- AWS Account with appropriate permissions
- IAM user with permissions for:
  - Amazon ECR (Elastic Container Registry)
  - Amazon EKS (Elastic Kubernetes Service)
  - EC2 (for EKS worker nodes)
  - VPC and networking resources
- Existing EKS cluster or permissions to create one
- AWS Load Balancer Controller installed on EKS cluster

### External Services
- **SQL Server Database**
  - Compatible versions: SQL Server 2016+, Azure SQL Database
  - Database user with appropriate permissions
  - Network connectivity from Kubernetes pods to database

---

## Local Development Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd TourManagement
```

### 2. Configure Database Connection

Create or update `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "dbconnection": "Server=localhost;Database=tourdb;User Id=sa;Password=YourPassword;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

### 3. Restore Dependencies
```bash
cd DotNetFrameworkProject_CE040_CE087/Tour_Management
dotnet restore
```

### 4. Run the Application
```bash
dotnet run
```

The application will start on http://localhost:8080

### 5. Run with Docker Compose (Local)
```bash
# Set environment variables
export DB_SERVER=your-db-server
export DB_NAME=tourdb
export DB_USER=your-user
export DB_PASSWORD=your-password

# Start the application
docker-compose up -d

# View logs
docker-compose logs -f

# Stop the application
docker-compose down
```

---

## Docker Deployment

### Building the Docker Image

#### Linux/macOS
```bash
chmod +x scripts/build-push.sh
./scripts/build-push.sh
```

#### Windows
```cmd
scripts\build-push.bat
```

The script will:
1. Prompt for registry selection (AWS ECR or Docker Hub)
2. Request registry credentials
3. Build the Docker image
4. Push to the selected registry

### Manual Docker Build
```bash
# Build image
docker build -t tour-management:latest .

# Run container
docker run -d \
  -p 8080:8080 \
  -e DB_SERVER=your-db-server \
  -e DB_NAME=tourdb \
  -e DB_USER=your-user \
  -e DB_PASSWORD=your-password \
  --name tour-management \
  tour-management:latest

# View logs
docker logs -f tour-management
```

---

## AWS EKS Deployment

### 1. Setup EKS Cluster

If you don't have an existing EKS cluster:

```bash
# Install eksctl
# macOS: brew install weaveworks/tap/eksctl
# Linux: See https://github.com/weaveworks/eksctl

# Create cluster (adjust parameters as needed)
eksctl create cluster \
  --name tour-management-cluster \
  --region us-east-1 \
  --nodegroup-name standard-workers \
  --node-type t3.medium \
  --nodes 2 \
  --nodes-min 1 \
  --nodes-max 4 \
  --managed
```

### 2. Install AWS Load Balancer Controller

Required for the Ingress to work:

```bash
# Add IAM OIDC provider
eksctl utils associate-iam-oidc-provider \
  --region us-east-1 \
  --cluster tour-management-cluster \
  --approve

# Create IAM policy
curl -o iam_policy.json https://raw.githubusercontent.com/kubernetes-sigs/aws-load-balancer-controller/main/docs/install/iam_policy.json

aws iam create-policy \
  --policy-name AWSLoadBalancerControllerIAMPolicy \
  --policy-document file://iam_policy.json

# Create service account
eksctl create iamserviceaccount \
  --cluster=tour-management-cluster \
  --namespace=kube-system \
  --name=aws-load-balancer-controller \
  --attach-policy-arn=arn:aws:iam::<AWS_ACCOUNT_ID>:policy/AWSLoadBalancerControllerIAMPolicy \
  --override-existing-serviceaccounts \
  --approve

# Install controller using Helm
helm repo add eks https://aws.github.io/eks-charts
helm repo update

helm install aws-load-balancer-controller eks/aws-load-balancer-controller \
  -n kube-system \
  --set clusterName=tour-management-cluster \
  --set serviceAccount.create=false \
  --set serviceAccount.name=aws-load-balancer-controller
```

### 3. Build and Push Image to ECR

```bash
# Run the build script
./scripts/build-push.sh

# Select option 1 (AWS ECR)
# Enter your AWS region and account ID
# Note the full image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/tour-management:latest)
```

### 4. Deploy to EKS

#### Linux/macOS
```bash
chmod +x scripts/deploy-image.sh
./scripts/deploy-image.sh
```

#### Windows
```cmd
scripts\deploy-image.bat
```

The deployment script will:
1. Prompt for AWS region and EKS cluster name
2. Request the Docker image URI
3. Prompt for database configuration
4. Configure kubectl
5. Update Kubernetes manifests with your values
6. Deploy the application
7. Wait for rollout completion
8. Display deployment status

### 5. Verify Deployment

```bash
# Check pod status
kubectl get pods -n tour-management

# View logs
kubectl logs -f deployment/tour-management -n tour-management

# Check service
kubectl get svc -n tour-management

# Get ingress URL
kubectl get ingress -n tour-management

# Describe deployment (useful for troubleshooting)
kubectl describe deployment tour-management -n tour-management
```

### 6. Access the Application

```bash
# Get the Load Balancer URL
kubectl get ingress tour-management-ingress -n tour-management -o jsonpath='{.status.loadBalancer.ingress[0].hostname}'
```

The application will be accessible at the ALB hostname (it may take 2-3 minutes for DNS to propagate).

### 7. Update Custom Domain (Optional)

Update `kubernetes/ingress.yaml` to use your custom domain:
```yaml
spec:
  rules:
  - host: tour.yourdomain.com  # Change this
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: tour-management-service
            port:
              number: 80
```

Then create a CNAME record in Route 53 pointing to the ALB hostname.

---

## Configuration Management

### Environment Variables

The application uses the following environment variables:

| Variable | Description | Required | Default |
|----------|-------------|----------|----------|
| `ASPNETCORE_ENVIRONMENT` | ASP.NET Core environment | No | Production |
| `PORT` | Application port | No | 8080 |
| `DB_SERVER` | SQL Server hostname/IP | Yes | - |
| `DB_NAME` | Database name | Yes | tourdb |
| `DB_USER` | Database username | Yes | - |
| `DB_PASSWORD` | Database password | Yes | - |
| `CHART_TEMP_DIR` | Chart image temp directory | No | /tmp/charts |
| `UPLOAD_PATH` | File upload directory | No | /app/uploads |

### Kubernetes Secrets

For production, use Kubernetes secrets for sensitive data:

```bash
# Create secret for database password
kubectl create secret generic tour-management-secrets \
  --from-literal=db-password='YourSecurePassword' \
  -n tour-management

# Verify secret
kubectl get secret tour-management-secrets -n tour-management
```

The deployment manifest already references this secret for `DB_PASSWORD`.

### Health Checks

The application exposes the following health check endpoints:

- **Liveness Probe**: `GET /health` - Checks if application is running
- **Readiness Probe**: `GET /ready` - Checks if application can accept traffic

Both endpoints return HTTP 200 when healthy.

---

## Troubleshooting

### Pod Fails to Start

```bash
# Check pod status
kubectl get pods -n tour-management

# Describe pod for events
kubectl describe pod <pod-name> -n tour-management

# View pod logs
kubectl logs <pod-name> -n tour-management

# Check previous container logs if pod restarted
kubectl logs <pod-name> -n tour-management --previous
```

**Common Issues:**
- **ImagePullBackOff**: Check ECR permissions and image URI
- **CrashLoopBackOff**: Check database connectivity and environment variables
- **Pending**: Check node resources and scheduling constraints

### Database Connection Issues

```bash
# Test database connectivity from pod
kubectl exec -it <pod-name> -n tour-management -- /bin/bash

# Inside pod, check environment variables
env | grep DB_

# Test DNS resolution
nslookup your-db-server
```

**Solutions:**
- Verify database server is accessible from EKS VPC
- Check security groups allow traffic on SQL Server port (1433)
- Verify database credentials are correct
- Check connection string format in `appsettings.json`

### Health Check Failures

```bash
# Check health endpoint directly
kubectl port-forward deployment/tour-management 8080:8080 -n tour-management
curl http://localhost:8080/health
curl http://localhost:8080/ready
```

**Solutions:**
- Increase `initialDelaySeconds` in deployment.yaml if app startup is slow
- Check database connectivity (health check validates DB connection)
- Review application logs for errors

### Ingress Not Working

```bash
# Check ingress status
kubectl get ingress -n tour-management
kubectl describe ingress tour-management-ingress -n tour-management

# Check AWS Load Balancer Controller logs
kubectl logs -n kube-system deployment/aws-load-balancer-controller
```

**Solutions:**
- Verify AWS Load Balancer Controller is installed
- Check IAM permissions for Load Balancer Controller
- Verify security groups allow HTTP/HTTPS traffic
- Check target group health in AWS console

### Scaling Issues

```bash
# Scale deployment
kubectl scale deployment tour-management --replicas=3 -n tour-management

# Check HPA status (if configured)
kubectl get hpa -n tour-management

# View resource usage
kubectl top pods -n tour-management
```

### Rollback Deployment

```bash
# View rollout history
kubectl rollout history deployment/tour-management -n tour-management

# Rollback to previous version
kubectl rollout undo deployment/tour-management -n tour-management

# Rollback to specific revision
kubectl rollout undo deployment/tour-management --to-revision=2 -n tour-management
```

---

## Security Considerations

### Application Security

1. **Non-root User**: The Docker image runs as a non-root user (`appuser`)
2. **Read-only Root Filesystem**: Consider adding `readOnlyRootFilesystem: true` to security context
3. **Resource Limits**: CPU and memory limits are enforced to prevent resource exhaustion

### Network Security

1. **Private Subnets**: Deploy EKS worker nodes in private subnets
2. **Security Groups**: 
   - Allow only necessary inbound traffic to worker nodes
   - Restrict database access to EKS security group
3. **Network Policies**: Implement Kubernetes Network Policies for pod-to-pod communication

### Secrets Management

1. **Kubernetes Secrets**: Store sensitive data in Kubernetes secrets (encrypted at rest)
2. **AWS Secrets Manager**: For enhanced security, integrate with AWS Secrets Manager:

```bash
# Install Secrets Store CSI Driver
helm repo add secrets-store-csi-driver https://kubernetes-sigs.github.io/secrets-store-csi-driver/charts
helm install csi-secrets-store secrets-store-csi-driver/secrets-store-csi-driver --namespace kube-system

# Install AWS provider
kubectl apply -f https://raw.githubusercontent.com/aws/secrets-store-csi-driver-provider-aws/main/deployment/aws-provider-installer.yaml
```

3. **Rotate Credentials**: Regularly rotate database passwords and API keys

### Container Security

1. **Image Scanning**: Enable ECR image scanning
```bash
aws ecr put-image-scanning-configuration \
  --repository-name tour-management \
  --image-scanning-configuration scanOnPush=true \
  --region us-east-1
```

2. **Vulnerability Patching**: Regularly update base images and dependencies
3. **Image Signing**: Consider implementing image signing with Notary or Cosign

### RBAC (Role-Based Access Control)

Implement least-privilege access:

```yaml
apiVersion: v1
kind: ServiceAccount
metadata:
  name: tour-management-sa
  namespace: tour-management
---
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  name: tour-management-role
  namespace: tour-management
rules:
- apiGroups: [""]
  resources: ["secrets", "configmaps"]
  verbs: ["get", "list"]
---
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: tour-management-rolebinding
  namespace: tour-management
subjects:
- kind: ServiceAccount
  name: tour-management-sa
roleRef:
  kind: Role
  name: tour-management-role
  apiGroup: rbac.authorization.k8s.io
```

Update deployment to use service account:
```yaml
spec:
  template:
    spec:
      serviceAccountName: tour-management-sa
```

### Monitoring and Auditing

1. **CloudWatch Logs**: Configure container logs to stream to CloudWatch
2. **AWS CloudTrail**: Enable CloudTrail for EKS API audit logging
3. **Container Insights**: Enable Container Insights for EKS monitoring

```bash
# Enable Container Insights
aws eks update-cluster-config \
  --region us-east-1 \
  --name tour-management-cluster \
  --logging '{"clusterLogging":[{"types":["api","audit","authenticator","controllerManager","scheduler"],"enabled":true}]}'
```

---

## .NET-Specific Deployment Notes

### Performance Optimization

1. **ReadyToRun Images**: Consider using ReadyToRun compilation for faster startup:
```xml
<PropertyGroup>
  <PublishReadyToRun>true</PublishReadyToRun>
</PropertyGroup>
```

2. **Tiered Compilation**: Enabled by default in .NET 8.0 for better performance

3. **Garbage Collection**: Configure GC for containerized environments:
```dockerfile
ENV DOTNET_gcServer=1
ENV DOTNET_GCHeapHardLimit=0x40000000  # 1GB limit
```

### Culture and Globalization

The application uses invariant globalization (`DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false`) to support localized date/time formats.

### Health Checks

The application uses `AspNetCore.HealthChecks.SqlServer` to validate database connectivity. The health check will fail if the database is unreachable.

### System.Web Adapters

This application uses `Microsoft.AspNetCore.SystemWebAdapters` for migrating legacy ASP.NET code to ASP.NET Core. Be aware of compatibility limitations.

---

## Additional Resources

- [.NET 8.0 Documentation](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [ASP.NET Core Health Checks](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks)
- [AWS EKS Best Practices](https://aws.github.io/aws-eks-best-practices/)
- [Kubernetes Documentation](https://kubernetes.io/docs/home/)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)

---

## Support

For issues or questions:
1. Check the troubleshooting section above
2. Review application logs
3. Consult AWS EKS documentation
4. Contact your DevOps team

---

**Document Version**: 1.0  
**Last Updated**: January 2026  
**Application**: Tour Management  
**Platform**: AWS EKS (Kubernetes)