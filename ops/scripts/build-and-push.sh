#!/usr/bin/env bash
set -euo pipefail

standard_image_path="docker.myob.com/future-makers-academy"
image_name=ghibli-universe
image_tag=$(git rev-parse --short HEAD)

docker build -t "$standard_image_path/$image_name:$image_tag" .

docker push "$standard_image_path/$image_name:$image_tag"