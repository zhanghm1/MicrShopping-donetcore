# 设置基础镜像
FROM nginx:alpine

RUN echo $'server {
    listen       80;
    listen  [::]:80;
    server_name  localhost;

    location / {
        root   /usr/share/nginx/html;
        index  index.html index.htm;
        try_files $uri $uri/ /index.html;
    }

}
' >/etc/nginx/conf.d/default.conf
# 将dist文件中的内容复制到 /usr/share/nginx/html/ 这个目录下面
COPY .  /usr/share/nginx/html/