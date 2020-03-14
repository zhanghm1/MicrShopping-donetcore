import Vue from 'vue'
import Vuex from 'vuex'
import mutations from './mutations'

Vue.use(Vuex)
//这里是具体的状态值
const state = {
    UserInfo: {},
    UserPermission:[]
};

//mutations 是更新state某个值的具体行为
//例如state有UserInfo，则需要更新UserInfo需要调用mutations的SaveUserInfo()方法

export default new Vuex.Store({
	state,
	mutations,
})