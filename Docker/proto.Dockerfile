FROM alpine:latest as base

WORKDIR /proto-store

RUN apk add --update --no-cache \
    ca-certificates \
    git \
    openssh-client && \
    rm -rf /var/cache/apk/* && \
    git clone https://github.com/U22-2024/proto.git 
    
ENTRYPOINT ["sh", "-c"]