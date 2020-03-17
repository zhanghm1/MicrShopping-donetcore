<template>
  <div class="hello">
  </div>
</template>

<script>
import Oidc from "oidc-client" ;
import {mapState,mapMutations} from 'vuex';

export default {
  name: 'LoginCallBack',
computed: {
            ...mapState([
                'UserInfo'
            ]),
    },
      methods:{
        ...mapMutations([
                'SaveUserInfo','LoginOut'
            ]),
      },
  created(){
      var that=this;
       new Oidc.UserManager({ response_mode: "query" }).signinRedirectCallback().then( (user)=> {
            //window.location = "index.html";
            window.console.log("LoginCallBack",user);
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
                that.$router.push("/");
            }else{
            window.console.log("Login Fail");
            }
            
        }).catch(function (e) {
            console.error(e);
        });
  }
}
</script>

<style scoped>

</style>
