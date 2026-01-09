=== scripts/deploy-image.sh ===
#!/bin/bash

set -e
set -o pipefail

echo "========================================"
echo "AWS ECS Fargate Deployment Script"
echo "========================================"
echo ""

# Prompt for AWS configuration
read -p "Enter AWS region (e.g., us-east-1): " AWS_REGION
read -p "Enter ECS cluster name (e.g., tour-management-cluster): " CLUSTER_NAME
read -p "Enter VPC ID (e.g., vpc-0abc123def456): " VPC_ID
read -p "Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): " SUBNETS
read -p "Enter Security Group ID (e.g., sg-0abc123def): " SECURITY_GROUP
read -p "Enter Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/tour-management-asp:latest): " IMAGE_URI

echo ""
echo "--- Database Configuration ---"
read -p "Enter Database Server (e.g., mydb.cluster-abc.us-east-1.rds.amazonaws.com): " DB_SERVER
read -p "Enter Database Name (e.g., TourManagementDB): " DB_NAME
read -p "Enter Database User (e.g., admin): " DB_USER
read -sp "Enter Database Password: " DB_PASSWORD
echo ""

# Parse subnets
IFS=',' read -ra SUBNET_ARRAY <<< "$SUBNETS"
SUBNET_1=${SUBNET_ARRAY[0]}
SUBNET_2=${SUBNET_ARRAY[1]:-$SUBNET_1}

# Get AWS Account ID
echo ""
echo "Getting AWS Account ID..."
ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
echo "Account ID: $ACCOUNT_ID"

# Check/Create ECS Cluster
echo ""
echo "Checking ECS cluster..."
aws ecs describe-clusters --clusters "$CLUSTER_NAME" --region "$AWS_REGION" >/dev/null 2>&1 || {
    echo "Cluster does not exist. Creating ECS cluster: $CLUSTER_NAME"
    aws ecs create-cluster --cluster-name "$CLUSTER_NAME" --region "$AWS_REGION"
}

# Create CloudWatch Log Group
echo ""
echo "Creating CloudWatch log group..."
aws logs create-log-group --log-group-name "/ecs/tour-management-app" --region "$AWS_REGION" 2>/dev/null || echo "Log group already exists"

# Load Balancer Configuration
echo ""
read -p "Do you need a load balancer for this service? (y/n): " NEED_LB

if [[ "$NEED_LB" =~ ^[Yy]$ ]]; then
    echo ""
    echo "--- Creating Application Load Balancer ---"
    
    # Create ALB
    ALB_NAME="tour-mgmt-alb"
    echo "Creating Application Load Balancer: $ALB_NAME"
    ALB_ARN=$(aws elbv2 create-load-balancer \
        --name "$ALB_NAME" \
        --subnets $SUBNET_1 $SUBNET_2 \
        --security-groups "$SECURITY_GROUP" \
        --scheme internet-facing \
        --type application \
        --ip-address-type ipv4 \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].LoadBalancerArn' \
        --output text 2>/dev/null || aws elbv2 describe-load-balancers --names "$ALB_NAME" --region "$AWS_REGION" --query 'LoadBalancers[0].LoadBalancerArn' --output text)
    
    echo "ALB ARN: $ALB_ARN"
    
    # Get ALB DNS Name
    ALB_DNS=$(aws elbv2 describe-load-balancers --load-balancer-arns "$ALB_ARN" --region "$AWS_REGION" --query 'LoadBalancers[0].DNSName' --output text)
    
    # Create Target Group
    TG_NAME="tour-mgmt-tg"
    echo "Creating Target Group: $TG_NAME"
    TARGET_GROUP_ARN=$(aws elbv2 create-target-group \
        --name "$TG_NAME" \
        --protocol HTTP \
        --port 8080 \
        --vpc-id "$VPC_ID" \
        --target-type ip \
        --health-check-enabled \
        --health-check-protocol HTTP \
        --health-check-path "/health" \
        --health-check-interval-seconds 30 \
        --health-check-timeout-seconds 10 \
        --healthy-threshold-count 2 \
        --unhealthy-threshold-count 3 \
        --region "$AWS_REGION" \
        --query 'TargetGroups[0].TargetGroupArn' \
        --output text 2>/dev/null || aws elbv2 describe-target-groups --names "$TG_NAME" --region "$AWS_REGION" --query 'TargetGroups[0].TargetGroupArn' --output text)
    
    echo "Target Group ARN: $TARGET_GROUP_ARN"
    
    # Create Listener
    echo "Creating ALB Listener..."
    aws elbv2 create-listener \
        --load-balancer-arn "$ALB_ARN" \
        --protocol HTTP \
        --port 80 \
        --default-actions Type=forward,TargetGroupArn="$TARGET_GROUP_ARN" \
        --region "$AWS_REGION" 2>/dev/null || echo "Listener already exists"
    
    LOAD_BALANCER_CONFIG="--load-balancers targetGroupArn=$TARGET_GROUP_ARN,containerName=tour-management-app,containerPort=8080 --health-check-grace-period-seconds 300"
else
    echo "Skipping load balancer configuration"
    LOAD_BALANCER_CONFIG=""
    # Remove loadBalancers section from service definition
    sed -i '/"loadBalancers"/,/],/d' ecs/service-definition.json
    sed -i '/"healthCheckGracePeriodSeconds"/d' ecs/service-definition.json
fi

# Replace placeholders in task definition
echo ""
echo "Preparing task definition..."
cp ecs/task-definition.json ecs/task-definition-deploy.json

sed -i "s|{{IMAGE_URI}}|$IMAGE_URI|g" ecs/task-definition-deploy.json
sed -i "s|{{AWS_REGION}}|$AWS_REGION|g" ecs/task-definition-deploy.json
sed -i "s|{{ACCOUNT_ID}}|$ACCOUNT_ID|g" ecs/task-definition-deploy.json
sed -i "s|{{DB_SERVER}}|$DB_SERVER|g" ecs/task-definition-deploy.json
sed -i "s|{{DB_NAME}}|$DB_NAME|g" ecs/task-definition-deploy.json
sed -i "s|{{DB_USER}}|$DB_USER|g" ecs/task-definition-deploy.json
sed -i "s|{{DB_PASSWORD}}|$DB_PASSWORD|g" ecs/task-definition-deploy.json

# Register task definition
echo ""
echo "Registering ECS task definition..."
TASK_DEFINITION_ARN=$(aws ecs register-task-definition \
    --cli-input-json file://ecs/task-definition-deploy.json \
    --region "$AWS_REGION" \
    --query 'taskDefinition.taskDefinitionArn' \
    --output text)

echo "Task Definition ARN: $TASK_DEFINITION_ARN"

# Check if service exists
echo ""
echo "Checking if service exists..."
SERVICE_EXISTS=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services tour-management-service \
    --region "$AWS_REGION" \
    --query 'services[0].serviceName' \
    --output text 2>/dev/null)

if [ "$SERVICE_EXISTS" = "tour-management-service" ]; then
    echo "Service exists. Updating service..."
    aws ecs update-service \
        --cluster "$CLUSTER_NAME" \
        --service tour-management-service \
        --task-definition "$TASK_DEFINITION_ARN" \
        --force-new-deployment \
        --region "$AWS_REGION"
else
    echo "Service does not exist. Creating service..."
    aws ecs create-service \
        --cluster "$CLUSTER_NAME" \
        --service-name tour-management-service \
        --task-definition "$TASK_DEFINITION_ARN" \
        --desired-count 2 \
        --launch-type FARGATE \
        --platform-version LATEST \
        --network-configuration "awsvpcConfiguration={subnets=[$SUBNET_1,$SUBNET_2],securityGroups=[$SECURITY_GROUP],assignPublicIp=ENABLED}" \
        $LOAD_BALANCER_CONFIG \
        --enable-ecs-managed-tags \
        --propagate-tags SERVICE \
        --tags key=Environment,value=production key=Application,value=TourManagement key=ManagedBy,value=ECS \
        --region "$AWS_REGION"
fi

# Wait for service stability
echo ""
echo "Waiting for service to become stable..."
aws ecs wait services-stable \
    --cluster "$CLUSTER_NAME" \
    --services tour-management-service \
    --region "$AWS_REGION"

# Verify deployment
echo ""
echo "========================================"
echo "Deployment Complete"
echo "========================================"
echo ""

RUNNING_COUNT=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services tour-management-service \
    --region "$AWS_REGION" \
    --query 'services[0].runningCount' \
    --output text)

echo "Service: tour-management-service"
echo "Cluster: $CLUSTER_NAME"
echo "Region: $AWS_REGION"
echo "Running Tasks: $RUNNING_COUNT"

if [[ "$NEED_LB" =~ ^[Yy]$ ]]; then
    echo ""
    echo "Application URL: http://$ALB_DNS"
    echo "Health Check: http://$ALB_DNS/health"
fi

echo ""
echo "CloudWatch Logs: /ecs/tour-management-app"
echo ""
echo "To view logs:"
echo "aws logs tail /ecs/tour-management-app --follow --region $AWS_REGION"
echo ""
echo "To check service status:"
echo "aws ecs describe-services --cluster $CLUSTER_NAME --services tour-management-service --region $AWS_REGION"
echo ""

# Cleanup
rm -f ecs/task-definition-deploy.json