import Vue from 'vue'
import App from './App.vue'
import router from './router';
import ElementUI from 'element-ui';
import store from './store';
import 'element-ui/lib/theme-chalk/index.css';
import bus from './common/bus';

Vue.config.productionTip = false
Vue.use(ElementUI);

Vue.prototype.$bus = bus;

new Vue({
  render: h => h(App),
  router: router,
  store: store
}).$mount('#app')
