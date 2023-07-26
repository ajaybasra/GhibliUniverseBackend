#!/usr/bin/env bash
set -euo pipefail

image_tag=$(git rev-parse --short HEAD)

echo "Deploying to $ENVIRONMENT..."
ktmpl ./ops/deployment/template.yaml -f "./ops/deployment/params/$ENVIRONMENT.yaml" --parameter imageTag "$image_tag" | kubectl apply -f -
echo "Deployment completed successfully."
