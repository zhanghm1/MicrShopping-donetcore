import Vue from 'vue';
 import router from '../router';
import axios from 'axios';
import {apiUrl} from '../configs/config';
const service = axios.create({
    baseURL: apiUrl,
    timeout: 30000
});
service.interceptors.request.use(
    config => {
        let access_token= localStorage.getItem("access_token");
        let token_type= localStorage.getItem("token_type");
        //重定向需要添加此请求头标识是ajax 请求
        config.headers['X-Requested-With'] = `XMLHttpRequest`;
        // 标识语言
        config.headers['Content-Language'] = localStorage.getItem("ContentLanguage");
        
        if(access_token){
            config.headers['Authorization'] = `${token_type} ${access_token}`;
        }
        return config;
    },
    error => {
        window.console.log(error);
        
        return Promise.reject();
    }
);

service.interceptors.response.use(
    response => {
        if (response.status === 200) {
            return response.data;
        }else {
            Promise.reject();
        }
    },
    error=> {
        
        if (error.response.status === 401) {
            window.console.log("response--error--data",error.response);
            if(error.response.data.Code=='Unauthorized'){
                Vue.prototype.$msgbox({
                    type: 'info',
                    message: error.response.data.Message
                });
                // 登陆无效
                //window.location.href='#/login'
                router.push("/login");
            }else if(error.response.data.Code=='NotApiAccess'){
                //可能是没有登录或者用户没有接口的访问权限
                Vue.prototype.$msgbox({
                    type: 'info',
                    message: error.response.data.Message
                  });
                //Vue.prototype.$alert(error.response.data.Message);
            }
        }
        return Promise.reject();
    }
);

export default service;