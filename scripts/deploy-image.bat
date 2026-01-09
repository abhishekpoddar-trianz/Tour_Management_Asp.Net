=== scripts/deploy-image.bat ===
@echo off
setlocal enabledelayedexpansion

echo ========================================
echo AWS ECS Fargate Deployment Script
echo ========================================
echo.

REM Prompt for AWS configuration
set /p AWS_REGION="Enter AWS region (e.g., us-east-1): "
set /p CLUSTER_NAME="Enter ECS cluster name (e.g., tour-management-cluster): "
set /p VPC_ID="Enter VPC ID (e.g., vpc-0abc123def456): "
set /p SUBNETS="Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): "
set /p SECURITY_GROUP="Enter Security Group ID (e.g., sg-0abc123def): "
set /p IMAGE_URI="Enter Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/tour-management-asp:latest): "

echo.
echo --- Database Configuration ---
set /p DB_SERVER="Enter Database Server (e.g., mydb.cluster-abc.us-east-1.rds.amazonaws.com): "
set /p DB_NAME="Enter Database Name (e.g., TourManagementDB): "
set /p DB_USER="Enter Database User (e.g., admin): "
set /p DB_PASSWORD="Enter Database Password: "

REM Parse subnets
for /f "tokens=1,2 delims=," %%a in ("!SUBNETS!") do (
    set SUBNET_1=%%a
    set SUBNET_2=%%b
)
if "!SUBNET_2!"=="" set SUBNET_2=!SUBNET_1!

REM Get AWS Account ID
echo.
echo Getting AWS Account ID...
for /f "delims=" %%i in ('aws sts get-caller-identity --query Account --output text') do set ACCOUNT_ID=%%i
echo Account ID: !ACCOUNT_ID!

REM Check/Create ECS Cluster
echo.
echo Checking ECS cluster...
aws ecs describe-clusters --clusters "!CLUSTER_NAME!" --region "!AWS_REGION!" >nul 2>&1
if !ERRORLEVEL! neq 0 (
    echo Cluster does not exist. Creating ECS cluster: !CLUSTER_NAME!
    aws ecs create-cluster --cluster-name "!CLUSTER_NAME!" --region "!AWS_REGION!"
)

REM Create CloudWatch Log Group
echo.
echo Creating CloudWatch log group...
aws logs create-log-group --log-group-name "/ecs/tour-management-app" --region "!AWS_REGION!" 2>nul
if !ERRORLEVEL! neq 0 echo Log group already exists

REM Load Balancer Configuration
echo.
set /p NEED_LB="Do you need a load balancer for this service? (y/n): "

if /i "!NEED_LB!"=="y" (
    echo.
    echo --- Creating Application Load Balancer ---
    
    set ALB_NAME=tour-mgmt-alb
    echo Creating Application Load Balancer: !ALB_NAME!
    
    for /f "delims=" %%i in ('aws elbv2 create-load-balancer --name "!ALB_NAME!" --subnets !SUBNET_1! !SUBNET_2! --security-groups "!SECURITY_GROUP!" --scheme internet-facing --type application --ip-address-type ipv4 --region "!AWS_REGION!" --query "LoadBalancers[0].LoadBalancerArn" --output text 2^>nul') do set ALB_ARN=%%i
    
    if "!ALB_ARN!"=="" (
        for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --names "!ALB_NAME!" --region "!AWS_REGION!" --query "LoadBalancers[0].LoadBalancerArn" --output text') do set ALB_ARN=%%i
    )
    
    echo ALB ARN: !ALB_ARN!
    
    for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --load-balancer-arns "!ALB_ARN!" --region "!AWS_REGION!" --query "LoadBalancers[0].DNSName" --output text') do set ALB_DNS=%%i
    
    set TG_NAME=tour-mgmt-tg
    echo Creating Target Group: !TG_NAME!
    
    for /f "delims=" %%i in ('aws elbv2 create-target-group --name "!TG_NAME!" --protocol HTTP --port 8080 --vpc-id "!VPC_ID!" --target-type ip --health-check-enabled --health-check-protocol HTTP --health-check-path "/health" --health-check-interval-seconds 30 --health-check-timeout-seconds 10 --healthy-threshold-count 2 --unhealthy-threshold-count 3 --region "!AWS_REGION!" --query "TargetGroups[0].TargetGroupArn" --output text 2^>nul') do set TARGET_GROUP_ARN=%%i
    
    if "!TARGET_GROUP_ARN!"=="" (
        for /f "delims=" %%i in ('aws elbv2 describe-target-groups --names "!TG_NAME!" --region "!AWS_REGION!" --query "TargetGroups[0].TargetGroupArn" --output text') do set TARGET_GROUP_ARN=%%i
    )
    
    echo Target Group ARN: !TARGET_GROUP_ARN!
    
    echo Creating ALB Listener...
    aws elbv2 create-listener --load-balancer-arn "!ALB_ARN!" --protocol HTTP --port 80 --default-actions Type=forward,TargetGroupArn="!TARGET_GROUP_ARN!" --region "!AWS_REGION!" 2>nul
    if !ERRORLEVEL! neq 0 echo Listener already exists
    
    set LOAD_BALANCER_CONFIG=--load-balancers targetGroupArn=!TARGET_GROUP_ARN!,containerName=tour-management-app,containerPort=8080 --health-check-grace-period-seconds 300
) else (
    echo Skipping load balancer configuration
    set LOAD_BALANCER_CONFIG=
)

REM Prepare task definition
echo.
echo Preparing task definition...
copy /y ecs\task-definition.json ecs\task-definition-deploy.json >nul

powershell -Command "(Get-Content ecs\task-definition-deploy.json) -replace '{{IMAGE_URI}}', '%IMAGE_URI%' | Set-Content ecs\task-definition-deploy.json"
powershell -Command "(Get-Content ecs\task-definition-deploy.json) -replace '{{AWS_REGION}}', '%AWS_REGION%' | Set-Content ecs\task-definition-deploy.json"
powershell -Command "(Get-Content ecs\task-definition-deploy.json) -replace '{{ACCOUNT_ID}}', '%ACCOUNT_ID%' | Set-Content ecs\task-definition-deploy.json"
powershell -Command "(Get-Content ecs\task-definition-deploy.json) -replace '{{DB_SERVER}}', '%DB_SERVER%' | Set-Content ecs\task-definition-deploy.json"
powershell -Command "(Get-Content ecs\task-definition-deploy.json) -replace '{{DB_NAME}}', '%DB_NAME%' | Set-Content ecs\task-definition-deploy.json"
powershell -Command "(Get-Content ecs\task-definition-deploy.json) -replace '{{DB_USER}}', '%DB_USER%' | Set-Content ecs\task-definition-deploy.json"
powershell -Command "(Get-Content ecs\task-definition-deploy.json) -replace '{{DB_PASSWORD}}', '%DB_PASSWORD%' | Set-Content ecs\task-definition-deploy.json"

REM Register task definition
echo.
echo Registering ECS task definition...
for /f "delims=" %%i in ('aws ecs register-task-definition --cli-input-json file://ecs/task-definition-deploy.json --region "!AWS_REGION!" --query "taskDefinition.taskDefinitionArn" --output text') do set TASK_DEFINITION_ARN=%%i

echo Task Definition ARN: !TASK_DEFINITION_ARN!

REM Check if service exists
echo.
echo Checking if service exists...
for /f "delims=" %%i in ('aws ecs describe-services --cluster "!CLUSTER_NAME!" --services tour-management-service --region "!AWS_REGION!" --query "services[0].serviceName" --output text 2^>nul') do set SERVICE_EXISTS=%%i

if "!SERVICE_EXISTS!"=="tour-management-service" (
    echo Service exists. Updating service...
    aws ecs update-service --cluster "!CLUSTER_NAME!" --service tour-management-service --task-definition "!TASK_DEFINITION_ARN!" --force-new-deployment --region "!AWS_REGION!"
) else (
    echo Service does not exist. Creating service...
    aws ecs create-service --cluster "!CLUSTER_NAME!" --service-name tour-management-service --task-definition "!TASK_DEFINITION_ARN!" --desired-count 2 --launch-type FARGATE --platform-version LATEST --network-configuration "awsvpcConfiguration={subnets=[!SUBNET_1!,!SUBNET_2!],securityGroups=[!SECURITY_GROUP!],assignPublicIp=ENABLED}" !LOAD_BALANCER_CONFIG! --enable-ecs-managed-tags --propagate-tags SERVICE --tags key=Environment,value=production key=Application,value=TourManagement key=ManagedBy,value=ECS --region "!AWS_REGION!"
)

REM Wait for service stability
echo.
echo Waiting for service to become stable...
aws ecs wait services-stable --cluster "!CLUSTER_NAME!" --services tour-management-service --region "!AWS_REGION!"

REM Verify deployment
echo.
echo ========================================
echo Deployment Complete
echo ========================================
echo.

for /f "delims=" %%i in ('aws ecs describe-services --cluster "!CLUSTER_NAME!" --services tour-management-service --region "!AWS_REGION!" --query "services[0].runningCount" --output text') do set RUNNING_COUNT=%%i

echo Service: tour-management-service
echo Cluster: !CLUSTER_NAME!
echo Region: !AWS_REGION!
echo Running Tasks: !RUNNING_COUNT!

if /i "!NEED_LB!"=="y" (
    echo.
    echo Application URL: http://!ALB_DNS!
    echo Health Check: http://!ALB_DNS!/health
)

echo.
echo CloudWatch Logs: /ecs/tour-management-app
echo.
echo To view logs:
echo aws logs tail /ecs/tour-management-app --follow --region !AWS_REGION!
echo.
echo To check service status:
echo aws ecs describe-services --cluster !CLUSTER_NAME! --services tour-management-service --region !AWS_REGION!
echo.

REM Cleanup
del /f /q ecs\task-definition-deploy.json 2>nul

endlocal