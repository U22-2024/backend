FROM node:alpine

# 必要なパッケージをインストール
RUN apk update \
 && apk --no-cache add openjdk17-jre-headless \
 && rm -rf /var/cache/apk/* \
 && npm install -g firebase-tools

WORKDIR /opt/firebase
CMD [ "firebase", "emulators:start", "--project", "u22-2024" ]
