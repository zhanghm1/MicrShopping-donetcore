import {SaveUserInfo,SaveUserPermission,LoginOut} from "./mutation-types"
export default {
    [SaveUserInfo](state,userinfo){
        window.console.log(userinfo);
        state.UserInfo=userinfo;
    },
    [SaveUserPermission](state,userPermission){
        state.UserPermission=userPermission;
    },
    [LoginOut](state){
        state.UserInfo={};
        state.UserPermission=[];
    }
}
