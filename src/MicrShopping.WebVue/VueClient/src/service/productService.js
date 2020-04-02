import axios from "../common/Request"

const productService={
    /**获取产品列表 */
    GetProductList:(PageIndex,PageSize)=> axios.get(`/product/product/List?pageSize=${PageSize}&pageIndex=${PageIndex}`),
    /**获取产品分类 */
    GetProductClassList:()=> axios.get(`/product/product/ClassList`),
    /**获取产品详情 */
    GetProductDetail:(id)=> axios.get('/product/product/Detail/'+id),
};
export default productService;

