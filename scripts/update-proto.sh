#!/bin/sh

echo "Updating proto files..."
git -C /proto-store/proto pull origin main
cp -rf -T --remove-destination /proto-store/proto/gen/csharp /app/GrpcService/Generated