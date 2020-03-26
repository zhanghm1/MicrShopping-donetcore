
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
docker build -f Dockerfile -t micrshopping.webvue . --no-cache


#登录镜像仓库
docker  login -u admin -p 123456 http://127.0.0.1:30003

# 获取版本号，将tag用版本号代替
old_docker_tag=v0.1.1
new_docker_tag=v0.1.2

#添加tag
docker tag micrshopping.orderapi 127.0.0.1:30003/micrshopping/micrshopping.orderapi:v0.1.2
docker tag micrshopping.identity 127.0.0.1:30003/micrshopping/micrshopping.identity:v0.1.2
docker tag micrshopping.payapi 127.0.0.1:30003/micrshopping/micrshopping.payapi:v0.1.2
docker tag micrshopping.productapi 127.0.0.1:30003/micrshopping/micrshopping.productapi:v0.1.2
docker tag micrshopping.usermanageapi 127.0.0.1:30003/micrshopping/micrshopping.usermanageapi:v0.1.2
docker tag micrshopping.webmvc 127.0.0.1:30003/micrshopping/micrshopping.webmvc:v0.1.2
docker tag micrshopping.webvue 127.0.0.1:30003/micrshopping/micrshopping.webvue:v0.1.2

#推送
docker push 127.0.0.1:30003/micrshopping/micrshopping.orderapi:v0.1.2
docker push 127.0.0.1:30003/micrshopping/micrshopping.identity:v0.1.2
docker push 127.0.0.1:30003/micrshopping/micrshopping.payapi:v0.1.2
docker push 127.0.0.1:30003/micrshopping/micrshopping.productapi:v0.1.2
docker push 127.0.0.1:30003/micrshopping/micrshopping.usermanageapi:v0.1.2
docker push 127.0.0.1:30003/micrshopping/micrshopping.webmvc:v0.1.2
docker push 127.0.0.1:30003/micrshopping/micrshopping.webvue:v0.1.2

# docker pull 127.0.0.1:30003/library/micrshopping.orderapi:v0.1.2


cd ../../../../charts/micr-shopping

cp -r v0.1.1/ v0.1.2

sed -i 's/0.1.1/0.1.2/g' v0.1.2/Chart.yaml

#执行字符串
cmd="echo hello word"

$cmd
