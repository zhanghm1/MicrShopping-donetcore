import Vue from 'vue';
import Router from 'vue-router';


Vue.use(Router);

const routes = [
    {
        path: '/callback',
        component: () => import('../components/Callback.vue'),
    },
    {
        path: '/',
        component: () => import('../components/HelloWorld.vue'),
    }

];

  // 3. 创建 router 实例，然后传 `routes` 配置
  // 你还可以传别的配置参数, 不过先这么简单着吧。
  const router = new Router({
    routes: routes
  });


  export default router;


  