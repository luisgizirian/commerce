#!/bin/sh
set -e

echo "Generating envoy.yaml config file..."
cat /tmpl/envoy.yaml.tmpl | envsubst \$ENVOY_CATALOG_API_ADDRESS,\$ENVOY_ORDERING_API_ADDRESS,\$ENVOY_FUNCTIONS_ADDRESS,\$ENVOY_CATALOG_API_PORT,\$ENVOY_ORDERING_API_PORT,\$ENVOY_FUNCTIONS_PORT > /etc/envoy.yaml

echo "Starting Envoy..."
/usr/local/bin/envoy -c /etc/envoy.yaml