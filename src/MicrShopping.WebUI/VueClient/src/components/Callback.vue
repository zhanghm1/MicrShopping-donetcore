<template>
  <div class="hello">
  </div>
</template>

<script>
import Oidc from "oidc-client" ;


export default {
  name: 'Callback',

  created(){
      var that=this;
       new Oidc.UserManager({ response_mode: "query" }).signinRedirectCallback().then(function () {
            //window.location = "index.html";
            that.$router.push("/");
        }).catch(function (e) {
            console.error(e);
        });
  }
}
</script>

<style scoped>

</style>
