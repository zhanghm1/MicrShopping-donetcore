#!/bin/sh

#拉取最新代码
git checkout master

docker build -f MicrShopping.OrderApi/Dockerfile -t micrshopping.orderapi . --no-cache
docker build -f MicrShopping.Identity/Dockerfile -t micrshopping.identity . --no-cache
docker build -f MicrShopping.PayApi/Dockerfile -t micrshopping.payapi . --no-cache
docker build -f MicrShopping.ProductApi/Dockerfile -t micrshopping.productapi . --no-cache
docker build -f MicrShopping.UserManageApi/Dockerfile -t micrshopping.usermanageapi . --no-cache
docker build -f MicrShopping.WebMVC/Dockerfile -t micrshopping.webmvc . --no-cache


#登录镜像仓库
docker  login -u admin -p Harbor12345 http://127.0.0.1:30003

# 获取版本号，将tag用版本号代替

old_docker_tag=$1
new_docker_tag=$2

cmd="docker tag micrshopping.orderapi 127.0.0.1:30003/micrshopping/micrshopping.orderapi:$new_docker_tag"
$cmd
cmd="docker tag micrshopping.identity 127.0.0.1:30003/micrshopping/micrshopping.identity:$new_docker_tag" 
$cmd
cmd="docker tag micrshopping.payapi 127.0.0.1:30003/micrshopping/micrshopping.payapi:$new_docker_tag"
$cmd
cmd="docker tag micrshopping.productapi 127.0.0.1:30003/micrshopping/micrshopping.productapi:$new_docker_tag" 
$cmd
cmd="docker tag micrshopping.usermanageapi 127.0.0.1:30003/micrshopping/micrshopping.usermanageapi:$new_docker_tag" 
$cmd
cmd="docker tag micrshopping.webmvc 127.0.0.1:30003/micrshopping/micrshopping.webmvc:$new_docker_tag" 
$cmd

cmd="docker push 127.0.0.1:30003/micrshopping/micrshopping.orderapi:$new_docker_tag" 
$cmd
cmd="docker push 127.0.0.1:30003/micrshopping/micrshopping.identity:$new_docker_tag"  
$cmd
cmd="docker push 127.0.0.1:30003/micrshopping/micrshopping.payapi:$new_docker_tag" 
$cmd
cmd="docker push 127.0.0.1:30003/micrshopping/micrshopping.productapi:$new_docker_tag" 
$cmd
cmd="docker push 127.0.0.1:30003/micrshopping/micrshopping.usermanageapi:$new_docker_tag"
$cmd
cmd="docker push 127.0.0.1:30003/micrshopping/micrshopping.webmvc:$new_docker_tag" 
$cmd

cd ../charts/micr-shopping


# 移除将要创建的文件夹
cmd="rm -rf $new_docker_tag"
echo $cmd
$cmd

# 复制新版本的chart
cmd="cp -r $old_docker_tag/ $new_docker_tag"
echo $cmd
$cmd

# 编辑版本号
cmd="sed -i 's/${old_docker_tag#*v}/${new_docker_tag#*v}/g' $new_docker_tag/Chart.yaml"
echo $cmd
eval $cmd


#提交到git
git add .
git commit -m "创建charts"






