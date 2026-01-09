=== docs/DEPLOYMENT.md ===
# Tour Management ASP.NET Core - Deployment Guide

## Table of Contents

1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [Local Development Setup](#local-development-setup)
4. [Docker Deployment](#docker-deployment)
5. [AWS ECS Fargate Deployment](#aws-ecs-fargate-deployment)
6. [Configuration Management](#configuration-management)
7. [Troubleshooting](#troubleshooting)
8. [Security Considerations](#security-considerations)
9. [Monitoring and Observability](#monitoring-and-observability)

---

## Overview

This guide provides comprehensive instructions for deploying the Tour Management ASP.NET Core 8.0 Razor Pages application using Docker containers and AWS ECS Fargate.

**Application Details:**
- **Framework:** .NET 8.0 (ASP.NET Core)
- **Application Type:** Razor Pages Web Application
- **Architecture:** Clean Architecture (Domain, Application, Infrastructure, Web layers)
- **Database:** SQL Server with Entity Framework Core 8.0
- **Logging:** Serilog with console and file sinks
- **Health Checks:** ASP.NET Core Health Checks with database validation
- **Port:** 8080 (HTTP)
- **Health Endpoint:** `/health`

---

## Prerequisites

### General Requirements

- **Operating System:** Linux, macOS, or Windows
- **Git:** For cloning the repository
- **.NET 8.0 SDK:** For local development and testing
- **Docker:** Version 20.10 or higher
- **Docker Compose:** Version 2.0 or higher

### AWS ECS Fargate Requirements

- **AWS Account:** Active AWS account with appropriate permissions
- **AWS CLI:** Version 2.x installed and configured
  ```bash
  aws --version
  aws configure
  ```
- **IAM Permissions:** User with permissions for:
  - ECS (create clusters, services, task definitions)
  - ECR (push/pull images)
  - EC2 (VPC, subnets, security groups)
  - IAM (create/attach roles)
  - CloudWatch Logs (create log groups)
  - Elastic Load Balancing (optional, for ALB)

### Network Requirements

- **VPC:** Configured VPC with at least 2 subnets in different availability zones
- **Security Groups:** Security group allowing:
  - Inbound: Port 8080 (application) from ALB or 0.0.0.0/0
  - Outbound: Port 443 (HTTPS) for ECR, Port 1433 (SQL Server)
- **Database Access:** SQL Server instance accessible from ECS tasks

### IAM Roles Required

**1. ECS Task Execution Role** (`ecsTaskExecutionRole`):
```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "ecr:GetAuthorizationToken",
        "ecr:BatchCheckLayerAvailability",
        "ecr:GetDownloadUrlForLayer",
        "ecr:BatchGetImage",
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Resource": "*"
    }
  ]
}
```

**2. ECS Task Role** (`ecsTaskRole`) - Optional, for task-level permissions:
```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "s3:GetObject",
        "s3:PutObject"
      ],
      "Resource": "arn:aws:s3:::your-bucket/*"
    }
  ]
}
```

Create roles using AWS CLI:
```bash
# Create execution role
aws iam create-role --role-name ecsTaskExecutionRole --assume-role-policy-document file://trust-policy.json
aws iam attach-role-policy --role-name ecsTaskExecutionRole --policy-arn arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy

# Create task role
aws iam create-role --role-name ecsTaskRole --assume-role-policy-document file://trust-policy.json
```

---

## Local Development Setup

### 1. Clone the Repository

```bash
cd /modernize-data/studio-data/TNT1001/APP2856/transformed-code/962/studio-workspace/Tour_Management_Asp
```

### 2. Configure Database Connection

Update `src/TourManagement.Web/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TourManagementDB;User Id=sa;Password=YourStrong@Password;TrustServerCertificate=true;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Build the Application

```bash
cd src/TourManagement.Web
dotnet build -c Release
```

### 5. Run the Application

```bash
dotnet run
```

Access the application:
- **HTTP:** http://localhost:5000
- **HTTPS:** https://localhost:5001
- **Health Check:** http://localhost:5000/health

---

## Docker Deployment

### 1. Build Docker Image Locally

```bash
# From repository root
docker build -f Dockerfile -t tour-management-asp:latest .
```

### 2. Run with Docker Compose

```bash
# Set environment variables
export DB_SERVER=your-database-server.com
export DB_NAME=TourManagementDB
export DB_USER=sa
export DB_PASSWORD=YourStrong@Password

# Start the application
docker-compose up -d

# View logs
docker-compose logs -f

# Access application
curl http://localhost:8080/health
```

### 3. Build and Push to Registry

**Option A: Using Interactive Script (Recommended)**

```bash
# Linux/macOS
chmod +x scripts/build-push.sh
./scripts/build-push.sh

# Windows
scripts\build-push.bat
```

The script will:
1. Prompt for registry type (AWS ECR or Docker Hub)
2. Prompt for registry credentials and details
3. Build the Docker image
4. Authenticate with the registry
5. Push the image

**Option B: Manual Push to AWS ECR**

```bash
# Set variables
AWS_REGION=us-east-1
AWS_ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
ECR_REPO=tour-management-asp
IMAGE_TAG=latest

# Create ECR repository
aws ecr create-repository --repository-name $ECR_REPO --region $AWS_REGION

# Authenticate Docker to ECR
aws ecr get-login-password --region $AWS_REGION | docker login --username AWS --password-stdin $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com

# Build and tag image
docker build -f Dockerfile -t $ECR_REPO:$IMAGE_TAG .
docker tag $ECR_REPO:$IMAGE_TAG $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/$ECR_REPO:$IMAGE_TAG

# Push to ECR
docker push $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/$ECR_REPO:$IMAGE_TAG
```

---

## AWS ECS Fargate Deployment

### Prerequisites Checklist

- [ ] AWS CLI configured with credentials
- [ ] VPC with at least 2 subnets in different AZs
- [ ] Security group configured (allow port 8080 inbound)
- [ ] IAM roles created (ecsTaskExecutionRole, ecsTaskRole)
- [ ] SQL Server database accessible from ECS
- [ ] Docker image pushed to ECR
- [ ] CloudWatch log group permissions

### ECS Fargate Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    Application Load Balancer            │
│                  (Optional, Port 80/443)                │
└─────────────────┬───────────────────────────────────────┘
                  │
                  │ HTTP Traffic
                  │
┌─────────────────▼───────────────────────────────────────┐
│                   Target Group (IP Mode)                 │
│              Health Check: /health (Port 8080)          │
└─────────────────┬───────────────────────────────────────┘
                  │
        ┌─────────┴─────────┐
        │                   │
┌───────▼─────────┐ ┌───────▼─────────┐
│  ECS Task 1     │ │  ECS Task 2     │
│  (Fargate)      │ │  (Fargate)      │
│                 │ │                 │
│  Container:     │ │  Container:     │
│  tour-mgmt-app  │ │  tour-mgmt-app  │
│  Port: 8080     │ │  Port: 8080     │
└─────────────────┘ └─────────────────┘
        │                   │
        └─────────┬─────────┘
                  │
                  │ SQL Connection (Port 1433)
                  │
        ┌─────────▼─────────┐
        │   SQL Server      │
        │  (RDS/External)   │
        └───────────────────┘

┌─────────────────────────────────────────────────────────┐
│              CloudWatch Logs                            │
│       Log Group: /ecs/tour-management-app              │
└─────────────────────────────────────────────────────────┘
```

### Understanding ECS Task Definition

The task definition (`ecs/task-definition.json`) specifies:

**1. Fargate Configuration:**
```json
{
  "requiresCompatibilities": ["FARGATE"],
  "networkMode": "awsvpc",
  "cpu": "512",
  "memory": "1024"
}
```

**Valid Fargate CPU/Memory Combinations:**
- CPU: `256` (.25 vCPU) → Memory: 512, 1024, 2048 MB
- CPU: `512` (.5 vCPU) → Memory: 1024, 2048, 3072, 4096 MB
- CPU: `1024` (1 vCPU) → Memory: 2048-8192 MB (increments of 1024)
- CPU: `2048` (2 vCPU) → Memory: 4096-16384 MB
- CPU: `4096` (4 vCPU) → Memory: 8192-30720 MB

**2. Container Definition:**
```json
{
  "name": "tour-management-app",
  "image": "{{IMAGE_URI}}",
  "essential": true,
  "portMappings": [
    {"containerPort": 8080, "protocol": "tcp"}
  ]
}
```

**3. Environment Variables:**
```json
"environment": [
  {"name": "ASPNETCORE_ENVIRONMENT", "value": "Production"},
  {"name": "ASPNETCORE_URLS", "value": "http://+:8080"},
  {"name": "DB_SERVER", "value": "{{DB_SERVER}}"},
  {"name": "DB_NAME", "value": "{{DB_NAME}}"},
  {"name": "DB_USER", "value": "{{DB_USER}}"},
  {"name": "DB_PASSWORD", "value": "{{DB_PASSWORD}}"}
]
```

**4. Logging Configuration:**
```json
"logConfiguration": {
  "logDriver": "awslogs",
  "options": {
    "awslogs-group": "/ecs/tour-management-app",
    "awslogs-region": "{{AWS_REGION}}",
    "awslogs-stream-prefix": "ecs"
  }
}
```

### ECS Service Configuration

The service definition (`ecs/service-definition.json`) specifies:

**1. Service Settings:**
```json
{
  "serviceName": "tour-management-service",
  "desiredCount": 2,
  "launchType": "FARGATE"
}
```

**2. Network Configuration (awsvpc mode):**
```json
"networkConfiguration": {
  "awsvpcConfiguration": {
    "subnets": ["subnet-xxx", "subnet-yyy"],
    "securityGroups": ["sg-xxx"],
    "assignPublicIp": "ENABLED"
  }
}
```

**3. Load Balancer Configuration (if using ALB):**
```json
"loadBalancers": [
  {
    "targetGroupArn": "arn:aws:elasticloadbalancing:...",
    "containerName": "tour-management-app",
    "containerPort": 8080
  }
]
```

**4. Deployment Configuration:**
```json
"deploymentConfiguration": {
  "maximumPercent": 200,
  "minimumHealthyPercent": 50,
  "deploymentCircuitBreaker": {
    "enable": true,
    "rollback": true
  }
}
```

### Deployment Walkthrough

**Step 1: Prepare Network Infrastructure**

```bash
# Get VPC ID
VPC_ID=$(aws ec2 describe-vpcs --filters "Name=isDefault,Values=true" --query "Vpcs[0].VpcId" --output text --region us-east-1)

# Get Subnet IDs (need at least 2 in different AZs)
SUBNET_1=$(aws ec2 describe-subnets --filters "Name=vpc-id,Values=$VPC_ID" --query "Subnets[0].SubnetId" --output text --region us-east-1)
SUBNET_2=$(aws ec2 describe-subnets --filters "Name=vpc-id,Values=$VPC_ID" --query "Subnets[1].SubnetId" --output text --region us-east-1)

# Create Security Group
SG_ID=$(aws ec2 create-security-group \
  --group-name tour-management-sg \
  --description "Security group for Tour Management ECS tasks" \
  --vpc-id $VPC_ID \
  --region us-east-1 \
  --query 'GroupId' \
  --output text)

# Allow inbound traffic on port 8080
aws ec2 authorize-security-group-ingress \
  --group-id $SG_ID \
  --protocol tcp \
  --port 8080 \
  --cidr 0.0.0.0/0 \
  --region us-east-1

# Allow outbound traffic (SQL Server)
aws ec2 authorize-security-group-egress \
  --group-id $SG_ID \
  --protocol tcp \
  --port 1433 \
  --cidr 0.0.0.0/0 \
  --region us-east-1
```

**Step 2: Create CloudWatch Log Group**

```bash
aws logs create-log-group \
  --log-group-name /ecs/tour-management-app \
  --region us-east-1

# Set retention (optional, 7 days)
aws logs put-retention-policy \
  --log-group-name /ecs/tour-management-app \
  --retention-in-days 7 \
  --region us-east-1
```

**Step 3: Deploy Using Interactive Script (Recommended)**

```bash
# Linux/macOS
chmod +x scripts/deploy-image.sh
./scripts/deploy-image.sh

# Windows
scripts\deploy-image.bat
```

The deployment script will:
1. Prompt for AWS region and cluster name
2. Prompt for network configuration (VPC, subnets, security groups)
3. Prompt for Docker image URI from ECR
4. Prompt for database connection details
5. Ask if you need a load balancer
6. If yes, automatically create ALB and Target Group
7. Create/update ECS cluster
8. Register task definition with provided configuration
9. Create or update ECS service
10. Wait for service to become stable
11. Display deployment status and access URLs

**Step 4: Verify Deployment**

```bash
# Check service status
aws ecs describe-services \
  --cluster tour-management-cluster \
  --services tour-management-service \
  --region us-east-1

# List running tasks
aws ecs list-tasks \
  --cluster tour-management-cluster \
  --service-name tour-management-service \
  --region us-east-1

# Get task details
TASK_ARN=$(aws ecs list-tasks --cluster tour-management-cluster --service-name tour-management-service --region us-east-1 --query 'taskArns[0]' --output text)

aws ecs describe-tasks \
  --cluster tour-management-cluster \
  --tasks $TASK_ARN \
  --region us-east-1
```

**Step 5: Access Application**

If using Application Load Balancer:
```bash
# Get ALB DNS name
ALB_DNS=$(aws elbv2 describe-load-balancers \
  --names tour-mgmt-alb \
  --region us-east-1 \
  --query 'LoadBalancers[0].DNSName' \
  --output text)

echo "Application URL: http://$ALB_DNS"
echo "Health Check: http://$ALB_DNS/health"

# Test health endpoint
curl http://$ALB_DNS/health
```

If using public IP directly:
```bash
# Get task public IP
TASK_ARN=$(aws ecs list-tasks --cluster tour-management-cluster --service-name tour-management-service --region us-east-1 --query 'taskArns[0]' --output text)

ENI_ID=$(aws ecs describe-tasks \
  --cluster tour-management-cluster \
  --tasks $TASK_ARN \
  --region us-east-1 \
  --query 'tasks[0].attachments[0].details[?name==`networkInterfaceId`].value' \
  --output text)

PUBLIC_IP=$(aws ec2 describe-network-interfaces \
  --network-interface-ids $ENI_ID \
  --region us-east-1 \
  --query 'NetworkInterfaces[0].Association.PublicIp' \
  --output text)

echo "Application URL: http://$PUBLIC_IP:8080"
echo "Health Check: http://$PUBLIC_IP:8080/health"

curl http://$PUBLIC_IP:8080/health
```

---

## Configuration Management

### Environment Variables

The application uses environment variables for configuration:

| Variable | Description | Example |
|----------|-------------|----------|
| `ASPNETCORE_ENVIRONMENT` | Application environment | `Production`, `Development` |
| `ASPNETCORE_URLS` | Kestrel listening URLs | `http://+:8080` |
| `DB_SERVER` | SQL Server hostname | `mydb.cluster-abc.us-east-1.rds.amazonaws.com` |
| `DB_NAME` | Database name | `TourManagementDB` |
| `DB_USER` | Database username | `admin` |
| `DB_PASSWORD` | Database password | `YourStrong@Password` |
| `TZ` | Timezone | `UTC`, `America/New_York` |

### Secrets Management

**Using AWS Secrets Manager (Recommended):**

1. Store database credentials in Secrets Manager:
```bash
aws secretsmanager create-secret \
  --name tour-management/db-credentials \
  --description "Database credentials for Tour Management" \
  --secret-string '{"username":"admin","password":"YourStrong@Password"}' \
  --region us-east-1
```

2. Update task definition to reference secrets:
```json
"secrets": [
  {
    "name": "DB_USER",
    "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789:secret:tour-management/db-credentials:username::"
  },
  {
    "name": "DB_PASSWORD",
    "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789:secret:tour-management/db-credentials:password::"
  }
]
```

3. Grant task execution role permissions:
```bash
aws iam put-role-policy \
  --role-name ecsTaskExecutionRole \
  --policy-name SecretsManagerAccess \
  --policy-document '{
    "Version": "2012-10-17",
    "Statement": [{
      "Effect": "Allow",
      "Action": [
        "secretsmanager:GetSecretValue"
      ],
      "Resource": "arn:aws:secretsmanager:us-east-1:123456789:secret:tour-management/*"
    }]
  }'
```

### Application Settings Override

To override appsettings.json in production:

1. Create custom appsettings.Production.json
2. Mount as volume in docker-compose or store in S3
3. Set environment variable: `ASPNETCORE_APPSETTINGS_PATH=/app/appsettings-override/appsettings.Production.json`

---

## Troubleshooting

### Common ECS Issues

**Issue 1: Tasks failing to start**

**Symptoms:** Tasks transition from PENDING to STOPPED immediately

**Diagnosis:**
```bash
# Get stopped task details
TASK_ARN=$(aws ecs list-tasks --cluster tour-management-cluster --desired-status STOPPED --region us-east-1 --query 'taskArns[0]' --output text)

aws ecs describe-tasks \
  --cluster tour-management-cluster \
  --tasks $TASK_ARN \
  --region us-east-1 \
  --query 'tasks[0].stoppedReason'
```

**Common Causes:**
- Invalid CPU/memory combination (must use valid Fargate combinations)
- ECR authentication failure (check execution role permissions)
- Image pull error (verify image URI and ECR permissions)
- Health check failures (check `/health` endpoint)

**Solutions:**
- Verify CPU/memory: Use `cpu: "512", memory: "1024"` as safe defaults
- Check execution role: `aws iam get-role --role-name ecsTaskExecutionRole`
- Test image locally: `docker pull <IMAGE_URI>`
- Test health endpoint: `curl http://localhost:8080/health`

---

**Issue 2: Cannot pull image from ECR**

**Symptoms:** Error: "CannotPullContainerError" in task stopped reason

**Diagnosis:**
```bash
# Check ECR repository
aws ecr describe-repositories --repository-names tour-management-asp --region us-east-1

# Check image tags
aws ecr list-images --repository-name tour-management-asp --region us-east-1

# Verify execution role policy
aws iam list-attached-role-policies --role-name ecsTaskExecutionRole
```

**Solutions:**
1. Ensure execution role has ECR permissions
2. Verify image URI format: `123456789.dkr.ecr.us-east-1.amazonaws.com/tour-management-asp:latest`
3. Check image exists in ECR repository
4. Ensure task is in same region as ECR repository

---

**Issue 3: Database connection failures**

**Symptoms:** Application starts but health check fails, logs show database connection errors

**Diagnosis:**
```bash
# View application logs
aws logs tail /ecs/tour-management-app --follow --region us-east-1

# Check security group rules
aws ec2 describe-security-groups --group-ids $SG_ID --region us-east-1
```

**Common Causes:**
- Database security group not allowing inbound from ECS security group
- Database hostname not resolvable from VPC
- Incorrect database credentials
- Database not accessible from public subnets (if using assignPublicIp: ENABLED)

**Solutions:**
1. Update database security group:
```bash
aws ec2 authorize-security-group-ingress \
  --group-id $DB_SG_ID \
  --protocol tcp \
  --port 1433 \
  --source-group $ECS_SG_ID \
  --region us-east-1
```
2. Verify connection string format
3. Test database connection from local machine
4. Ensure RDS instance is in same VPC or VPC peering is configured

---

**Issue 4: Health check failures**

**Symptoms:** Tasks start but ALB shows unhealthy targets

**Diagnosis:**
```bash
# Check target group health
aws elbv2 describe-target-health \
  --target-group-arn $TG_ARN \
  --region us-east-1

# Test health endpoint directly from task
PUBLIC_IP=<TASK_PUBLIC_IP>
curl http://$PUBLIC_IP:8080/health
```

**Common Causes:**
- Health check path incorrect
- Health check timeout too short
- Database unavailable (ASP.NET Core health check validates DB connection)
- Application startup time exceeds health check grace period

**Solutions:**
1. Increase `healthCheckGracePeriodSeconds` to 300 seconds
2. Update target group health check settings:
```bash
aws elbv2 modify-target-group \
  --target-group-arn $TG_ARN \
  --health-check-interval-seconds 30 \
  --health-check-timeout-seconds 10 \
  --healthy-threshold-count 2 \
  --unhealthy-threshold-count 3 \
  --region us-east-1
```
3. Verify health endpoint responds with HTTP 200
4. Check application logs for startup errors

---

**Issue 5: Service fails to stabilize**

**Symptoms:** `aws ecs wait services-stable` times out, tasks continuously restarting

**Diagnosis:**
```bash
# Check service events
aws ecs describe-services \
  --cluster tour-management-cluster \
  --services tour-management-service \
  --region us-east-1 \
  --query 'services[0].events[0:10]'

# Monitor task transitions
aws ecs describe-services \
  --cluster tour-management-cluster \
  --services tour-management-service \
  --region us-east-1 \
  --query 'services[0].deployments'
```

**Common Causes:**
- Health checks failing repeatedly
- Insufficient resources (CPU/memory)
- Application crashes on startup
- Deployment circuit breaker triggered

**Solutions:**
1. Check CloudWatch logs for application errors
2. Increase task resources (CPU/memory)
3. Disable circuit breaker temporarily for debugging:
```json
"deploymentConfiguration": {
  "deploymentCircuitBreaker": {
    "enable": false
  }
}
```
4. Fix application errors and redeploy

---

### Viewing Logs

**View logs in real-time:**
```bash
aws logs tail /ecs/tour-management-app --follow --region us-east-1
```

**View logs for specific time range:**
```bash
aws logs tail /ecs/tour-management-app \
  --since 1h \
  --region us-east-1
```

**Filter logs:**
```bash
aws logs tail /ecs/tour-management-app \
  --follow \
  --filter-pattern "ERROR" \
  --region us-east-1
```

**Export logs to file:**
```bash
aws logs tail /ecs/tour-management-app \
  --since 24h \
  --region us-east-1 > logs-export.txt
```

---

### Debugging Network Issues

**Test connectivity from ECS task to database:**

1. Enable ECS Exec on service:
```bash
aws ecs update-service \
  --cluster tour-management-cluster \
  --service tour-management-service \
  --enable-execute-command \
  --region us-east-1
```

2. Connect to running task:
```bash
TASK_ARN=$(aws ecs list-tasks --cluster tour-management-cluster --service-name tour-management-service --region us-east-1 --query 'taskArns[0]' --output text)

aws ecs execute-command \
  --cluster tour-management-cluster \
  --task $TASK_ARN \
  --container tour-management-app \
  --command "/bin/bash" \
  --interactive \
  --region us-east-1
```

3. Test database connectivity:
```bash
# Inside container
apt-get update && apt-get install -y telnet
telnet $DB_SERVER 1433
```

---

## Security Considerations

### Container Security

**1. Non-root User:**

The Dockerfile creates and uses a non-root user:
```dockerfile
RUN groupadd -r appuser && useradd -r -g appuser appuser
USER appuser
```

**2. Minimal Base Image:**

Using official Microsoft runtime images:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
```

**3. No Unnecessary Tools:**

Do not install `curl`, `wget`, or other tools in production images.

### Network Security

**1. Security Group Rules:**

- Allow only necessary inbound traffic (port 8080 from ALB)
- Restrict outbound traffic to required destinations
- Use security group references instead of CIDR blocks where possible

**2. Private Subnets (Recommended):**

For production, use private subnets with NAT Gateway:
```json
"networkConfiguration": {
  "awsvpcConfiguration": {
    "subnets": ["subnet-private-1", "subnet-private-2"],
    "securityGroups": ["sg-xxx"],
    "assignPublicIp": "DISABLED"
  }
}
```

**3. Database Security:**

- Use RDS in private subnets
- Enable encryption at rest
- Enable encryption in transit (SSL/TLS)
- Use IAM database authentication when possible

### Application Security

**1. HTTPS Enforcement:**

Enable HTTPS redirection in production:
```csharp
// Program.cs
if (app.Environment.IsProduction())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
```

**2. Security Headers:**

Add security headers middleware:
```csharp
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    await next();
});
```

**3. Input Validation:**

Use FluentValidation for input validation (already included).

**4. SQL Injection Prevention:**

Entity Framework Core uses parameterized queries by default.

### Secrets Management

**Best Practices:**

1. **Never** commit secrets to source control
2. Use AWS Secrets Manager or AWS Systems Manager Parameter Store
3. Rotate secrets regularly
4. Use IAM roles instead of access keys where possible
5. Enable encryption for secrets at rest

---

## Monitoring and Observability

### CloudWatch Metrics

**ECS Service Metrics:**

```bash
# View CPU utilization
aws cloudwatch get-metric-statistics \
  --namespace AWS/ECS \
  --metric-name CPUUtilization \
  --dimensions Name=ServiceName,Value=tour-management-service Name=ClusterName,Value=tour-management-cluster \
  --start-time 2024-01-01T00:00:00Z \
  --end-time 2024-01-02T00:00:00Z \
  --period 300 \
  --statistics Average \
  --region us-east-1

# View memory utilization
aws cloudwatch get-metric-statistics \
  --namespace AWS/ECS \
  --metric-name MemoryUtilization \
  --dimensions Name=ServiceName,Value=tour-management-service Name=ClusterName,Value=tour-management-cluster \
  --start-time 2024-01-01T00:00:00Z \
  --end-time 2024-01-02T00:00:00Z \
  --period 300 \
  --statistics Average \
  --region us-east-1
```

**Application Load Balancer Metrics:**

```bash
# Target response time
aws cloudwatch get-metric-statistics \
  --namespace AWS/ApplicationELB \
  --metric-name TargetResponseTime \
  --dimensions Name=LoadBalancer,Value=app/tour-mgmt-alb/xxx \
  --start-time 2024-01-01T00:00:00Z \
  --end-time 2024-01-02T00:00:00Z \
  --period 300 \
  --statistics Average \
  --region us-east-1

# Request count
aws cloudwatch get-metric-statistics \
  --namespace AWS/ApplicationELB \
  --metric-name RequestCount \
  --dimensions Name=LoadBalancer,Value=app/tour-mgmt-alb/xxx \
  --start-time 2024-01-01T00:00:00Z \
  --end-time 2024-01-02T00:00:00Z \
  --period 300 \
  --statistics Sum \
  --region us-east-1
```

### CloudWatch Alarms

**Create alarm for high CPU utilization:**

```bash
aws cloudwatch put-metric-alarm \
  --alarm-name tour-management-high-cpu \
  --alarm-description "Alert when CPU exceeds 80%" \
  --metric-name CPUUtilization \
  --namespace AWS/ECS \
  --statistic Average \
  --period 300 \
  --threshold 80 \
  --comparison-operator GreaterThanThreshold \
  --evaluation-periods 2 \
  --dimensions Name=ServiceName,Value=tour-management-service Name=ClusterName,Value=tour-management-cluster \
  --region us-east-1
```

**Create alarm for unhealthy targets:**

```bash
aws cloudwatch put-metric-alarm \
  --alarm-name tour-management-unhealthy-targets \
  --alarm-description "Alert when no healthy targets" \
  --metric-name HealthyHostCount \
  --namespace AWS/ApplicationELB \
  --statistic Average \
  --period 60 \
  --threshold 1 \
  --comparison-operator LessThanThreshold \
  --evaluation-periods 2 \
  --dimensions Name=TargetGroup,Value=targetgroup/tour-mgmt-tg/xxx Name=LoadBalancer,Value=app/tour-mgmt-alb/xxx \
  --region us-east-1
```

### Application Insights (Optional)

Integrate Azure Application Insights or AWS X-Ray:

**1. Add Application Insights NuGet package:**
```bash
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

**2. Configure in Program.cs:**
```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

**3. Set instrumentation key via environment variable:**
```json
"environment": [
  {
    "name": "APPLICATIONINSIGHTS_CONNECTION_STRING",
    "value": "InstrumentationKey=xxx-xxx-xxx"
  }
]
```

### Structured Logging with Serilog

The application already uses Serilog. Enhance with structured logging:

```csharp
// In your code
_logger.LogInformation("User {UserId} created booking {BookingId} for tour {TourId}", 
    userId, bookingId, tourId);
```

Query logs in CloudWatch Logs Insights:
```
fields @timestamp, @message
| filter @message like /ERROR/
| sort @timestamp desc
| limit 100
```

---

## Scaling and Performance

### ECS Service Auto Scaling

**1. Create scaling policy:**

```bash
# Register scalable target
aws application-autoscaling register-scalable-target \
  --service-namespace ecs \
  --resource-id service/tour-management-cluster/tour-management-service \
  --scalable-dimension ecs:service:DesiredCount \
  --min-capacity 2 \
  --max-capacity 10 \
  --region us-east-1

# Create scaling policy (target tracking)
aws application-autoscaling put-scaling-policy \
  --service-namespace ecs \
  --resource-id service/tour-management-cluster/tour-management-service \
  --scalable-dimension ecs:service:DesiredCount \
  --policy-name tour-management-cpu-scaling \
  --policy-type TargetTrackingScaling \
  --target-tracking-scaling-policy-configuration '{
    "TargetValue": 70.0,
    "PredefinedMetricSpecification": {
      "PredefinedMetricType": "ECSServiceAverageCPUUtilization"
    },
    "ScaleInCooldown": 300,
    "ScaleOutCooldown": 60
  }' \
  --region us-east-1
```

### .NET Performance Tuning

**1. Enable ReadyToRun (R2R) compilation:**

Update .csproj:
```xml
<PropertyGroup>
  <PublishReadyToRun>true</PublishReadyToRun>
</PropertyGroup>
```

**2. Enable tiered compilation:**

Set environment variable:
```json
{"name": "DOTNET_TieredCompilation", "value": "1"}
```

**3. Optimize Entity Framework queries:**

- Use `.AsNoTracking()` for read-only queries
- Implement pagination for large result sets
- Use compiled queries for frequently executed queries
- Enable query result caching where appropriate

---

## Backup and Disaster Recovery

### Database Backups

**For RDS:**

1. Enable automated backups (retention 7-35 days)
2. Configure backup window during low-traffic periods
3. Enable point-in-time recovery
4. Create manual snapshots before major changes

### Task Definition Versioning

ECS automatically versions task definitions. Rollback if needed:

```bash
# List task definition versions
aws ecs list-task-definitions \
  --family-prefix tour-management-task \
  --region us-east-1

# Rollback to previous version
aws ecs update-service \
  --cluster tour-management-cluster \
  --service tour-management-service \
  --task-definition tour-management-task:5 \
  --region us-east-1
```

### Blue/Green Deployments

For zero-downtime deployments, use AWS CodeDeploy with ECS:

1. Create CodeDeploy application and deployment group
2. Configure deployment configuration (Linear, Canary, or All-at-once)
3. Deploy new task definition via CodeDeploy
4. Automatic traffic shifting and rollback on failures

---

## Additional Resources

- **AWS ECS Documentation:** https://docs.aws.amazon.com/ecs/
- **AWS Fargate Documentation:** https://docs.aws.amazon.com/fargate/
- **.NET 8 Documentation:** https://learn.microsoft.com/en-us/dotnet/core/
- **ASP.NET Core Documentation:** https://learn.microsoft.com/en-us/aspnet/core/
- **Docker Documentation:** https://docs.docker.com/
- **AWS CLI Reference:** https://awscli.amazonaws.com/v2/documentation/api/latest/reference/ecs/index.html

---

## Support and Maintenance

For issues and questions:

1. Check CloudWatch logs for application errors
2. Review ECS service events for deployment issues
3. Verify network connectivity and security group rules
4. Consult AWS documentation and troubleshooting guides
5. Open support ticket with AWS Support (if applicable)

---

**End of Deployment Guide**