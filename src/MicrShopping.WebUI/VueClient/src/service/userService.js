import axios from "../common/Request"

const UserService={
    /**获取 */
    GetUserInfo:(currentUserId)=> axios.get('/identity/user/'+currentUserId),
    /**编辑 */
    EditUserInfo:(data)=> axios.patch('/identity/user',data),
    /**获取权限 */
    GetUserPermissions:()=> axios.get('/identity/UserPermissions'),
    /**注册 */
    Register:(data)=> axios.post('/identity/user/Register',data),
};
export default UserService;

