import axios from "../common/Request"

const orderService={
    GetShoppingCartList:()=> axios.get('/Order/ShoppingCart/List'),
    PostShoppingCart:(data)=> axios.post('/Order/ShoppingCart/Add',data),
    /**获取产品列表 */
    GetOrderList:(PageIndex,PageSize)=> axios.get(`/Order/Order/List?pageSize=${PageSize}&pageIndex=${PageIndex}`),
    /**获取产品详情 */
    GetOrderDetail:(id)=> axios.get('/Order/Order/Detail/'+id),
    /**创建订单 */
    CreateOrder:(data)=> axios.post('/Order/Order/Create',data),
};
export default orderService;

