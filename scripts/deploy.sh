#!/usr/bin/env bash
set -euo pipefail

image_tag=$(git rev-parse --short HEAD)

ktmpl ./templates/template.yaml -f ./templates/default.yaml --parameter imageTag ${image_tag} | kubectl apply -f -