#!/bin/sh

#登录镜像仓库
docker  login -u admin -p 123456 http://127.0.0.1:30003

# 获取版本号，将tag用版本号代替

old_docker_tag=$1
new_docker_tag=$2

cmd=("docker tag micrshopping.orderapi 127.0.0.1:30003/micrshopping/micrshopping.orderapi:$new_docker_tag" "docker tag micrshopping.identity 127.0.0.1:30003/micrshopping/micrshopping.identity:$new_docker_tag" "docker tag micrshopping.payapi 127.0.0.1:30003/micrshopping/micrshopping.payapi:$new_docker_tag" "docker tag micrshopping.productapi 127.0.0.1:30003/micrshopping/micrshopping.productapi:$new_docker_tag" "docker tag micrshopping.usermanageapi 127.0.0.1:30003/micrshopping/micrshopping.usermanageapi:$new_docker_tag" "docker tag micrshopping.webmvc 127.0.0.1:30003/micrshopping/micrshopping.webmvc:$new_docker_tag" "docker tag micrshopping.webvue 127.0.0.1:30003/micrshopping/micrshopping.webvue:$new_docker_tag" "docker push 127.0.0.1:30003/micrshopping/micrshopping.orderapi:$new_docker_tag" "docker push 127.0.0.1:30003/micrshopping/micrshopping.identity:$new_docker_tag" "docker push 127.0.0.1:30003/micrshopping/micrshopping.payapi:$new_docker_tag" "docker push 127.0.0.1:30003/micrshopping/micrshopping.productapi:$new_docker_tag" "docker push 127.0.0.1:30003/micrshopping/micrshopping.usermanageapi:$new_docker_tag" "docker push 127.0.0.1:30003/micrshopping/micrshopping.webmvc:$new_docker_tag" "docker push 127.0.0.1:30003/micrshopping/micrshopping.webvue:$new_docker_tag")

for value in ${cmd[@]}
do
echo $value
$value
done


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
$cmd

#提交到git
git add .
git commit -m "创建charts"

#添加tag
cmd="git tag ${new_docker_tag}"
echo $cmd
$cmd

