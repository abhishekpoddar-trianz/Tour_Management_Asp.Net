@echo off
setlocal enabledelayedexpansion

echo ========================================
echo Tour Management - Docker Build ^& Push
echo ========================================
echo.

set PROJECT_NAME=tour-management
set DOCKERFILE_PATH=Dockerfile

for /f "delims=" %%i in ('powershell -Command "'%PROJECT_NAME%'.ToLower() -replace '[^a-z0-9]+', '-' -replace '^-+', '' -replace '-+$', ''"') do set IMAGE_NAME=%%i

echo Select registry type:
echo 1. AWS ECR (Elastic Container Registry)
echo 2. Docker Hub
set /p REGISTRY_CHOICE="Enter choice (1 or 2): "
echo.

if "!REGISTRY_CHOICE!"=="1" (
    echo --- AWS ECR Configuration ---
    set /p AWS_REGION="Enter AWS Region (e.g., us-east-1): "
    set /p AWS_ACCOUNT_ID="Enter AWS Account ID: "
    set /p ECR_REPO="Enter ECR Repository Name [!IMAGE_NAME!]: "
    if "!ECR_REPO!"==" " set ECR_REPO=!IMAGE_NAME!
    
    set REGISTRY_URL=!AWS_ACCOUNT_ID!.dkr.ecr.!AWS_REGION!.amazonaws.com
    set FULL_IMAGE_BASE=!REGISTRY_URL!/!ECR_REPO!
    
    echo.
    echo Authenticating with AWS ECR...
    for /f "delims=" %%p in ('aws ecr get-login-password --region !AWS_REGION!') do set ECR_PASSWORD=%%p
    echo !ECR_PASSWORD! | docker login --username AWS --password-stdin !REGISTRY_URL!
    
    if !ERRORLEVEL! neq 0 (
        echo ERROR: ECR authentication failed
        exit /b 1
    )
    
    echo Checking if ECR repository exists...
    aws ecr describe-repositories --repository-names !ECR_REPO! --region !AWS_REGION! >nul 2>&1
    if !ERRORLEVEL! neq 0 (
        echo Repository does not exist. Creating ECR repository: !ECR_REPO!
        aws ecr create-repository --repository-name !ECR_REPO! --region !AWS_REGION!
        if !ERRORLEVEL! neq 0 (
            echo ERROR: Failed to create ECR repository
            exit /b 1
        )
        echo ECR repository created successfully
    )
    
) else if "!REGISTRY_CHOICE!"=="2" (
    echo --- Docker Hub Configuration ---
    set /p DOCKER_USERNAME="Enter Docker Hub username: "
    set /p DOCKER_PASSWORD="Enter Docker Hub password/token: "
    echo.
    
    set FULL_IMAGE_BASE=!DOCKER_USERNAME!/!IMAGE_NAME!
    
    echo Authenticating with Docker Hub...
    echo !DOCKER_PASSWORD! | docker login --username !DOCKER_USERNAME! --password-stdin
    
    if !ERRORLEVEL! neq 0 (
        echo ERROR: Docker Hub authentication failed
        exit /b 1
    )
) else (
    echo ERROR: Invalid choice. Please select 1 or 2.
    exit /b 1
)

echo.
set /p IMAGE_TAG="Enter image tag [latest]: "
if "!IMAGE_TAG!"==" " set IMAGE_TAG=latest

for /f "delims=" %%i in ('powershell -Command "'!IMAGE_TAG!'.ToLower() -replace '[^a-z0-9.-]+', '-' -replace '^-+', '' -replace '-+$', ''"') do set IMAGE_TAG=%%i
if "!IMAGE_TAG!"==" " set IMAGE_TAG=latest

set FULL_IMAGE_NAME=!FULL_IMAGE_BASE!:!IMAGE_TAG!

echo.
echo ========================================
echo Build Configuration:
echo   Image Name: !FULL_IMAGE_NAME!
echo   Dockerfile: !DOCKERFILE_PATH!
echo ========================================
echo.

set /p CONFIRM="Proceed with build? (y/n): "
if /i not "!CONFIRM!"=="y" (
    echo Build cancelled.
    exit /b 0
)

echo.
echo Building Docker image...
docker build -f !DOCKERFILE_PATH! -t !FULL_IMAGE_NAME! .

if !ERRORLEVEL! neq 0 (
    echo ERROR: Docker build failed
    exit /b 1
)

echo.
echo Pushing image to registry...
docker push !FULL_IMAGE_NAME!

if !ERRORLEVEL! neq 0 (
    echo ERROR: Docker push failed
    exit /b 1
)

echo.
echo ========================================
echo SUCCESS!
echo Image pushed: !FULL_IMAGE_NAME!
echo ========================================
echo.
echo Next steps:
echo 1. Update kubernetes/deployment.yaml with image URI
echo 2. Run scripts\deploy-image.bat to deploy to EKS
echo.

endlocal