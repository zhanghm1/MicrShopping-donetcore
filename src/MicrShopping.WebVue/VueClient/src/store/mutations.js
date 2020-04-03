import {SaveUserInfo,SaveUserPermission,LoginOut,SaveShoppingCarts} from "./mutation-types"
export default {
    [SaveUserInfo](state,userinfo){
        window.console.log(userinfo);
        state.UserInfo=userinfo;
    },
    [SaveUserPermission](state,userPermission){
        state.UserPermission=userPermission;
    },
    [LoginOut](state){
        state.UserInfo=null;
        state.UserPermission=[];
    },
    [SaveShoppingCarts](state,shoppingCarts){
        state.ShoppingCarts=shoppingCarts;
    }
}
