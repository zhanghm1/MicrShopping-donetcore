#!/bin/sh

#拉取最新代码
git checkout master

docker build -f MicrShopping.OrderApi/Dockerfile -t micrshopping.orderapi . --no-cache
docker build -f MicrShopping.Identity/Dockerfile -t micrshopping.identity . --no-cache
docker build -f MicrShopping.PayApi/Dockerfile -t micrshopping.payapi . --no-cache
docker build -f MicrShopping.ProductApi/Dockerfile -t micrshopping.productapi . --no-cache
docker build -f MicrShopping.UserManageApi/Dockerfile -t micrshopping.usermanageapi . --no-cache
docker build -f MicrShopping.WebMVC/Dockerfile -t micrshopping.webmvc . --no-cache

cd MicrShopping.WebVue/VueClient
npm i
npm run build
cd dist

cmd="sed -i 's/=\//=/g' index.html"
echo $cmd
eval $cmd

docker build -f Dockerfile -t micrshopping.webvue . --no-cache




