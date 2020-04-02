import axios from "../common/Request"

const orderService={
    GetShoppingCartList:()=> axios.get('/Order/ShoppingCart/List'),
    PostShoppingCart:(data)=> axios.post('/Order/ShoppingCart/Add',data),
    /**获取产品列表 */
    GetorderList:(PageIndex,PageSize)=> axios.get(`/Order/Order/List?pageSize=${PageSize}&pageIndex=${PageIndex}`),
    /**获取产品详情 */
    GetorderDetail:(id)=> axios.get('/Order/Order/Detail/'+id),
    /**创建订单 */
    PostEditOrder:(data)=> axios.post('/Order/Order/CreateOrder',data),
};
export default orderService;

