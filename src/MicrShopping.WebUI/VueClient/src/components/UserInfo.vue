<template>
  <div class="hello">
    <div v-if="IsLogin">
      {{UserInfo.UserName}}
      <button @click="GetUserInfo">获取用户信息</button>
      <button @click="logout">退出</button>
    </div>
    <div v-else>
      <button @click="login">登录</button>
    </div>
    
  </div>
</template>

<script>
import Oidc from "oidc-client" ;
import {mapState,mapMutations} from 'vuex';
import userService from "../service/userService" ;

var config = {
    authority: "http://192.168.0.189:5008/",
    client_id: "js-vue",
    redirect_uri: "http://192.168.0.189:5015/#/callback",
    response_type: "code",
    scope:"openid profile orderapi productapi identityapi",
    post_logout_redirect_uri : "http://192.168.0.189:5015/",
};


export default {
  name: 'UserInfo1',
  props: {
    msg: String
  },
  data(){
    return {
      IsLogin:false,
      OidcManager:{},
    }
  },
      computed: {
            ...mapState([
                'UserInfo'
            ]),
    },
  methods:{
        ...mapMutations([
                'SaveUserInfo','LoginOut'
            ]),
    login() {
        this.OidcManager.signinRedirect();
    },

    GetUserInfo() {
      userService.GetUserInfo(this.UserInfo.UserId).then((data)=>{
        window.console.log("GetUserInfo", data);
      });
    },

    logout() {
        this.LoginOut();
        localStorage.setItem("access_token",'');
        localStorage.setItem("user_name",'');

        this.OidcManager.signoutRedirect();
        
    }
  },
  created(){
    var that=this;
    this.OidcManager = new Oidc.UserManager(config);
    this.OidcManager.getUser().then(function (user) {
        if (user) {
          that.IsLogin=true;
            localStorage.setItem("id_token",user.id_token);
            localStorage.setItem("access_token",user.access_token);
            localStorage.setItem("user_name",user.profile.name);
            localStorage.setItem("token_type",user.token_type);
            localStorage.setItem("expires_at",user.expires_at);
            localStorage.setItem("refresh_token",user.refresh_token);
            localStorage.setItem("session_state",user.session_state);
            
            window.console.log("User logged in", user);
            //更新到store
            var userInfo={
              UserName:user.profile.name,
              UserId:user.profile.sub,
            };
            that.SaveUserInfo(userInfo);
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
