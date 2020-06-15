<template>
  <div class="OrderList" >
      <div>
          <div  class="close" @click="close">X</div>
            <div>我的订单</div>
      </div>
      
        <div class="product-item" v-for="item in OrderList" :key="item.id">
            <div>订单号{{item.code}}</div>
            <div>收件地址：{{item.address}}</div>
            <div>收件电话：</div>
            <div>金额：{{item.totalPrice}}</div>
            <button @click="ConfirmReceipt">确认收货</button>
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
                this.OrderList=resp.data.list;
            });
        },
        ConfirmReceipt(item){
            this.$confirm('确认已收货吗？').then(()=>{
                window.console.log("确认");
                window.console.log(item);
            })
        }
        
    },
    created(){
        this.GetOrderList();

    }
}
</script>

<style>
.OrderList{
    position: fixed;
    top: 10px;
    right: 10px;
    width: 300px;
    height: 400px;
    border: 1px solid red;
    min-height: 280px;
}
.close{
    float: right;
    padding: 0 10px;
    cursor: pointer;
}

.OrderList .product-item{
    border: 1px red solid;
    width: 100%;
    height: auto;
    padding: 10px 15px;
    margin: 0;
    float: none;
    box-sizing: border-box;
    overflow: hidden;
}
.Settlement{
    position:absolute;
    bottom: 0;
    height: 50px;
}
</style>
