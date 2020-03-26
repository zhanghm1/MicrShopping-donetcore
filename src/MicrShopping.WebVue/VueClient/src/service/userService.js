import axios from "../common/Request"

const UserService={
    /**获取 */
    GetUserInfo:(currentUserId)=> axios.get('/account/user/'+currentUserId),
    /**编辑 */
    EditUserInfo:(data)=> axios.patch('/account/user',data),
    /**获取权限 */
    GetUserPermissions:()=> axios.get('/account/UserPermissions'),
    /**注册 */
    Register:(data)=> axios.post('/account/user/Register',data),
};
export default UserService;

