#!/bin/sh

#拉取最新代码
git pull

cd MicrShopping.WebVue/VueClient
npm i
npm run build
cd dist

docker build -f Dockerfile -t micrshopping.webvue . --no-cache


#登录镜像仓库
docker  login -u admin -p Harbor12345 http://127.0.0.1:30003

# 获取版本号，将tag用版本号代替

old_docker_tag=$1
new_docker_tag=$2


cmd="docker tag micrshopping.webvue 127.0.0.1:30003/micrshopping/micrshopping.webvue:$new_docker_tag"
$cmd

cmd="docker push 127.0.0.1:30003/micrshopping/micrshopping.webvue:$new_docker_tag"
$cmd

#拉取最新代码
git pull

cd ../../../../charts/micr-shopping-webvue
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

