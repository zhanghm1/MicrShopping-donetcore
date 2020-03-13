<template>
  <div class="hello">
    <button @click="login">登录</button>
    <button @click="api">获取API接口数据</button>
    <button @click="logout">退出</button>
    <div v-for="(item,index) in userinfo" :key="index" >
      {{item.type}} : {{item.value}}
    </div>
  </div>
</template>

<script>
import Oidc from "oidc-client" ;
var config = {
    authority: "http://192.168.0.189:5008/",
    client_id: "js-vue",
    redirect_uri: "http://192.168.0.189:5015/#/callback",
    response_type: "code",
    scope:"openid profile orderapi productapi",
    post_logout_redirect_uri : "http://192.168.0.189:5015/",
};


export default {
  name: 'HelloWorld',
  props: {
    msg: String
  },
  data(){
    return {
      userinfo:[],
      OidcManager:{}
    }
  },
  methods:{
 


    login() {
        this.OidcManager.signinRedirect();
    },

    api() {
      var access_token = localStorage.getItem("access_token");
      var url = "http://192.168.0.189:5004/order/identity";
      var that=this;
      var xhr = new XMLHttpRequest();
      xhr.open("GET", url);
      xhr.onload = function () {
        window.console.log(xhr.status, JSON.parse(xhr.responseText));
        that.userinfo=JSON.parse(xhr.responseText);

      }
      xhr.setRequestHeader("Authorization", "Bearer " + access_token);
      xhr.send();
    },

    logout() {
        this.OidcManager.signoutRedirect();
    }
  },
  created(){
    this.OidcManager = new Oidc.UserManager(config);
    this.OidcManager.getUser().then(function (user) {
        if (user) {
            localStorage.setItem("access_token",user.access_token);
            window.console.log("User logged in", user.profile);
        }
        else {
            window.console.log("User not logged in");
        }
    });
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
