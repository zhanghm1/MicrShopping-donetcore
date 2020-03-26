
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
$old_docker_tag=$1
$new_docker_tag=$2


#添加tag
cmd="
docker tag micrshopping.orderapi 127.0.0.1:30003/micrshopping/micrshopping.orderapi:"${new_docker_tag}"
docker tag micrshopping.identity 127.0.0.1:30003/micrshopping/micrshopping.identity:${new_docker_tag}
docker tag micrshopping.payapi 127.0.0.1:30003/micrshopping/micrshopping.payapi:${new_docker_tag}
docker tag micrshopping.productapi 127.0.0.1:30003/micrshopping/micrshopping.productapi:${new_docker_tag}
docker tag micrshopping.usermanageapi 127.0.0.1:30003/micrshopping/micrshopping.usermanageapi:${new_docker_tag}
docker tag micrshopping.webmvc 127.0.0.1:30003/micrshopping/micrshopping.webmvc:${new_docker_tag}
docker tag micrshopping.webvue 127.0.0.1:30003/micrshopping/micrshopping.webvue:${new_docker_tag}

docker push 127.0.0.1:30003/micrshopping/micrshopping.orderapi:${new_docker_tag}
docker push 127.0.0.1:30003/micrshopping/micrshopping.identity:${new_docker_tag}
docker push 127.0.0.1:30003/micrshopping/micrshopping.payapi:${new_docker_tag}
docker push 127.0.0.1:30003/micrshopping/micrshopping.productapi:${new_docker_tag}
docker push 127.0.0.1:30003/micrshopping/micrshopping.usermanageapi:${new_docker_tag}
docker push 127.0.0.1:30003/micrshopping/micrshopping.webmvc:${new_docker_tag}
docker push 127.0.0.1:30003/micrshopping/micrshopping.webvue:${new_docker_tag}
"
$cmd



cd ../../../../charts/micr-shopping
# 移除将要创建的文件夹
cmd="rm -rf "${new_docker_tag}
$cmd

# 复制新版本的chart
cmd="cp -r "${old_docker_tag}"/ "${new_docker_tag}
$cmd

# 编辑版本号
cmd="sed -i 's/"${old_docker_tag#*v}"/"${new_docker_tag#*v}"/g' "${new_docker_tag}"/Chart.yaml"
$cmd

#提交到git
git add .
git commit -m "创建charts"

#添加tag
cmd="git tag "${new_docker_tag}
$cmd

#执行字符串
cmd="echo hello word"

$cmd
