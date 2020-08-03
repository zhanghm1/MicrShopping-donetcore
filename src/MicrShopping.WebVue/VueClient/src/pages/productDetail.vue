<template>
  <div class="ProductDetail">
    <div>{{product.name}}</div>
    <div>编号：{{product.code}}</div>
    <div>价格：{{product.realPrice}}</div>
    <div>库存：{{product.nowCount}}</div>
    <button @click="addShoppingCart">加入购物车</button>
  </div>
</template>
<script>
import productService from "../service/productService";
import orderService from "../service/orderService";

export default {
  name: "ProductDetail",
  data() {
    return {
      product: { name: "", code: "", realPrice: 0 },
    };
  },
  methods: {
    addShoppingCart() {
      //展示购物车
      let data = { productId: this.product.id, number: 1 };
      orderService.PostShoppingCart(data).then((resp) => {
        window.console.log(resp);
      });
    },
  },
  created() {
    let id = this.$route.params.id;
    productService.GetProductDetail(id).then((data) => {
      window.console.log(data);
      this.product = data.data;
    });
  },
};
</script>
<style>
</style>