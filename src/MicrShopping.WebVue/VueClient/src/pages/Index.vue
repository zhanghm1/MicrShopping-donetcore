<template>
  <div class="Index">
    index 哈哈哈
    <div class="product-list">
      <div
        class="product-item"
        v-for="item in productList"
        :key="item.id"
        @click="goProductDetail(item.id)"
      >
        <div>{{item.name}}</div>
      </div>
    </div>
  </div>
</template>
<script>
import productService from "../service/productService";

export default {
  name: "Index",
  data() {
    return {
      productPageSize: 10,
      productPageIndex: 1,
      searchName: "产",
      productList: [],
      productClassList: [],
    };
  },
  methods: {
    goProductDetail(id) {
      window.console.log(id);
      this.$router.push("/product/" + id);
    },
  },
  created() {
    productService.GetProductClassList().then((data) => {
      window.console.log(data);
      this.productClassList = data.data.list;
    });
    productService
      .GetProductList(
        this.productPageIndex,
        this.productPageSize,
        this.searchName
      )
      .then((data) => {
        window.console.log(data);
        this.productList = data.data.list;
      });
  },
};
</script>
<style>
.product-list {
  clear: both;
  width: 960px;
  margin: 0 auto;
  padding: 15px;
}
.product-item {
  border: 1px red solid;
  width: 50px;
  height: 80px;
  padding: 10px 15px;
  margin: 10px 15px;
  float: left;
}
</style>