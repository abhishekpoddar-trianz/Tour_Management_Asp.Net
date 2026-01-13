@echo off
setlocal enabledelayedexpansion

echo ========================================
echo Tour Management - AWS EKS Deployment
echo ========================================
echo.

set NAMESPACE=tour-management
set APP_NAME=tour-management

echo --- AWS EKS Configuration ---
set /p AWS_REGION="Enter AWS Region (e.g., us-east-1): "
set /p CLUSTER_NAME="Enter EKS Cluster Name: "
echo.

set /p IMAGE_URI="Enter Docker Image URI (with tag): "
if "!IMAGE_URI!"==" " (
    echo ERROR: Image URI is required
    exit /b 1
)
echo.

echo --- Database Configuration ---
set /p DB_SERVER="Enter Database Server (DB_SERVER): "
set /p DB_NAME="Enter Database Name (DB_NAME) [tourdb]: "
if "!DB_NAME!"==" " set DB_NAME=tourdb
set /p DB_USER="Enter Database User (DB_USER): "
set /p DB_PASSWORD="Enter Database Password (DB_PASSWORD): "
echo.

if "!DB_SERVER!"==" " (
    echo ERROR: Database server is required
    exit /b 1
)
if "!DB_USER!"==" " (
    echo ERROR: Database user is required
    exit /b 1
)
if "!DB_PASSWORD!"==" " (
    echo ERROR: Database password is required
    exit /b 1
)

echo Configuring kubectl for EKS cluster...
aws eks update-kubeconfig --region !AWS_REGION! --name !CLUSTER_NAME!

if !ERRORLEVEL! neq 0 (
    echo ERROR: Failed to configure kubectl
    exit /b 1
)

echo Verifying cluster connectivity...
kubectl cluster-info

if !ERRORLEVEL! neq 0 (
    echo ERROR: Cannot connect to cluster
    exit /b 1
)

echo.
echo Updating Kubernetes manifests...

set TMP_DIR=%TEMP%\k8s-deploy-%RANDOM%
mkdir !TMP_DIR!
mkdir !TMP_DIR!\kubernetes

xcopy /E /I /Y kubernetes !TMP_DIR!\kubernetes

powershell -Command "(Get-Content '!TMP_DIR!\kubernetes\deployment.yaml') -replace '{{IMAGE_URI}}', '!IMAGE_URI!' | Set-Content '!TMP_DIR!\kubernetes\deployment.yaml'"
powershell -Command "(Get-Content '!TMP_DIR!\kubernetes\deployment.yaml') -replace '{{DB_SERVER}}', '!DB_SERVER!' | Set-Content '!TMP_DIR!\kubernetes\deployment.yaml'"
powershell -Command "(Get-Content '!TMP_DIR!\kubernetes\deployment.yaml') -replace '{{DB_NAME}}', '!DB_NAME!' | Set-Content '!TMP_DIR!\kubernetes\deployment.yaml'"
powershell -Command "(Get-Content '!TMP_DIR!\kubernetes\deployment.yaml') -replace '{{DB_USER}}', '!DB_USER!' | Set-Content '!TMP_DIR!\kubernetes\deployment.yaml'"
powershell -Command "(Get-Content '!TMP_DIR!\kubernetes\deployment.yaml') -replace '{{DB_PASSWORD}}', '!DB_PASSWORD!' | Set-Content '!TMP_DIR!\kubernetes\deployment.yaml'"

echo Manifest placeholders updated successfully
echo.

echo ========================================
echo Deployment Configuration:
echo   Namespace: !NAMESPACE!
echo   Application: !APP_NAME!
echo   Image: !IMAGE_URI!
echo   Database: !DB_SERVER!/!DB_NAME!
echo ========================================
echo.

set /p CONFIRM="Proceed with deployment? (y/n): "
if /i not "!CONFIRM!"=="y" (
    echo Deployment cancelled.
    rmdir /S /Q !TMP_DIR!
    exit /b 0
)

echo.
echo Creating namespace...
kubectl apply -f !TMP_DIR!\kubernetes\namespace.yaml

echo Deploying application...
kubectl apply -f !TMP_DIR!\kubernetes\deployment.yaml

echo Creating service...
kubectl apply -f !TMP_DIR!\kubernetes\service.yaml

echo Creating ingress...
kubectl apply -f !TMP_DIR!\kubernetes\ingress.yaml

echo.
echo Waiting for deployment rollout...
kubectl rollout status deployment/!APP_NAME! -n !NAMESPACE! --timeout=300s

if !ERRORLEVEL! neq 0 (
    echo ERROR: Deployment rollout failed
    echo Checking pod status...
    kubectl get pods -n !NAMESPACE!
    echo.
    echo Checking recent events...
    kubectl get events -n !NAMESPACE! --sort-by=.lastTimestamp
    rmdir /S /Q !TMP_DIR!
    exit /b 1
)

echo.
echo ========================================
echo Deployment Status:
echo ========================================
kubectl get pods,svc,ingress -n !NAMESPACE!

echo.
echo ========================================
echo SUCCESS!
echo ========================================
echo.

echo Application deployed successfully to EKS cluster: !CLUSTER_NAME!
echo.
echo To access the application:
echo 1. Get the ingress URL:
echo    kubectl get ingress -n !NAMESPACE!
echo.
echo 2. View logs:
echo    kubectl logs -f deployment/!APP_NAME! -n !NAMESPACE!
echo.
echo 3. Check pod status:
echo    kubectl get pods -n !NAMESPACE!
echo.
echo 4. Describe deployment:
echo    kubectl describe deployment !APP_NAME! -n !NAMESPACE!
echo.

rmdir /S /Q !TMP_DIR!
endlocal