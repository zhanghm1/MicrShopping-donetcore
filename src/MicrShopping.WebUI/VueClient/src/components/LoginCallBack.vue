<template>
  <div class="hello">
  </div>
</template>

<script>
import Oidc from "oidc-client" ;


export default {
  name: 'LoginCallBack',

  created(){
      var that=this;
       new Oidc.UserManager({ response_mode: "query" }).signinRedirectCallback().then(function (e) {
            //window.location = "index.html";
            window.console.log("LoginCallBack",e);
            that.$router.push("/");
        }).catch(function (e) {
            console.error(e);
        });
  }
}
</script>

<style scoped>

</style>
