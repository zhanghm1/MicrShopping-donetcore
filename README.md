# MicrShopping-donetcore

开发使用的IP为192.168.0.189，可以为自己的主机设置IP,如果不嫌麻烦可以把docker-compose有关文件 中的192.168.0.189都换成你自己的IP.
最好是检查下有没有端口的使用情况，以免端口占用，特别是数据库端口，这里使用的是 postgreSQL,端口为默认端口5432

  为什么用IP?

  - 应为docker容器访问主机端口用ip；

  为什么不直接访问容器服务？

  - 有些是用的容器服务，有些没用，不服打我。

在linux 中构建镜像，vs 生成的Dockerfile不应该Dockerfile所在位置执行，从应从sln所在目录执行指定位置的Dockerfile

~/MicrShopping-donetcore/src/MicrShopping.OrderApi# docker build . --no-cache   这是错误的，会提示没有文件目录

~/MicrShopping-donetcore/src# docker build -f MicrShopping.OrderApi/Dockerfile -t MicrShopping.OrderApi . --no-cache  ##这样就没问题

 -f  文件位置
 
 -t  镜像名称

