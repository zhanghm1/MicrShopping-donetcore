<template>
  <div class="hello">
    <div v-if="UserInfo!=null&&UserInfo!=undefined">
      {{UserInfo.UserName}}
      <button @click="GetUserInfo">获取用户信息</button>
      <button @click="logout">退出</button>
      <button @click="shoppingCart">查看购物车</button>
      <vShoppingCart></vShoppingCart>
    </div>
    <div v-else>
      <button @click="login">登录</button>
    </div>
    
  </div>
</template>

<script>
import Oidc from "oidc-client" ;
import {mapState,mapMutations} from 'vuex';
import {OidcConfig} from '../configs/config';
import userService from "../service/userService" ;

import vShoppingCart from "./shoppingCart" ;
import bus from '../common/bus';

export default {
  name: 'UserInfo1',
  props: {
    msg: String
  },
  components: {
        vShoppingCart,
    },
  data(){
    return {
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
      userService.GetMyUserInfo().then((data)=>{
        window.console.log("GetUserInfo", data);
      });
    },

    logout() {
        this.LoginOut(); //清除store内存
        localStorage.setItem("access_token",''); //清除缓存
        localStorage.setItem("user_name",''); //清除缓存

        this.OidcManager.signoutRedirect();
        
    },
    shoppingCart(){
      bus.$emit("ShowShoppingCart",true);
    }
  },
  created(){
    //var that=this;
    this.OidcManager = new Oidc.UserManager(OidcConfig);
    this.OidcManager.getUser().then((user)=> {
        if (user) {
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
            this.SaveUserInfo(userInfo);
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
