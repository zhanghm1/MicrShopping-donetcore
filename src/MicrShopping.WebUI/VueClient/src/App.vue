<template>
  <div id="app">
    <vUserInfo></vUserInfo>
    <router-view></router-view>
  </div>
</template>

<script>
import vUserInfo  from "./components/UserInfo" ;
export default {
  name: 'App',
  components: {
        vUserInfo,
    },
}
</script>

<style>

</style>
