<template>
  <div class="hello">
    <button @click="login">登录</button>
    <button @click="api">获取API接口数据</button>
    <button @click="logout">退出</button>
  </div>
</template>

<script>
import Oidc from "oidc-client" ;
var config = {
    authority: "http://localhost:5000",
    client_id: "js",
    redirect_uri: "http://localhost:5003/callback.html",
    response_type: "code",
    scope:"openid profile api1",
    post_logout_redirect_uri : "http://localhost:5003/index.html",
};
var mgr = new Oidc.UserManager(config);
mgr.getUser().then(function (user) {
    if (user) {
        window.log("User logged in", user.profile);
    }
    else {
        window.log("User not logged in");
    }
});

export default {
  name: 'HelloWorld',
  props: {
    msg: String
  },
  methods:{
 


 login() {
    mgr.signinRedirect();
},

 api() {
    mgr.getUser().then(function (user) {
        var url = "http://localhost:5001/identity";

        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
           window.log(xhr.status, JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
},

 logout() {
    mgr.signoutRedirect();
}
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
