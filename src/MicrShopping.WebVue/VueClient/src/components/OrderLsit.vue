<template>
  <div class="OrderList" >
      <div>
          <div  class="close" @click="close">X</div>
        <div>我的订单</div>
        
      </div>
      
        <div class="product-item" v-for="item in OrderList" :key="item.id">
            <div>{{item.orderNo}}</div>
        </div>
  </div>
</template>

<script>
// import bus from '../common/bus';
import orderService from "../service/orderService" ;
export default {
    name: 'OrderList',
    data(){
        return {
            IsShowShoppingCart:false,
            OrderList:[],
            pageIndex:1,
            pageSize:10,
        }
    },
    computed: {

    },
    methods:{

        close(){
            
        },
        GetOrderList(){

            orderService.GetOrderList(this.pageIndex,this.pageSize).then(resp=>{
                window.console.log(resp);
                this.OrderList=resp.data;
            });
        },
        
    },
    created(){
        this.GetOrderList();

    }
}
</script>

<style>
.shoppingCart{
    position: fixed;
    top: 10px;
    right: 10px;
    width: 200px;
    border: 1px solid red;
    min-height: 280px;
}
.close{
    float: right;
    padding: 5px 10px;
}

.shoppingCart .product-item{
    border: 1px red solid;
    width: 100%;
    height: 80px;
    padding: 10px 15px;
    margin: 0;
    float: none;
    box-sizing: border-box;
}
.Settlement{
    position:absolute;
    bottom: 0;
    height: 50px;
}
</style>
