<template>
  <div class="shoppingCart" v-if="IsShowShoppingCart" >
      <div>
          <div  class="close" @click="close">X</div>
        <div>购物车</div>
        
      </div>
      
        <div class="product-item" v-for="item in ShoppingCarts" :key="item.id">
            <div>{{item.name}}</div>
            <div>数量{{item.number}}</div>
            <div>实际价格：{{item.realPrice}}</div>
        </div>
        <div class="Settlement">
            <span> 总计：{{Zongji}}</span> <button @click="createOrder">去付款</button>
        </div>
        
  </div>
</template>

<script>
import bus from '../common/bus';
import orderService from "../service/orderService" ;
import {mapState,mapMutations} from 'vuex';
export default {
    name: 'shoppingCart',
    data(){
        return {
            IsShowShoppingCart:false,
        }
    },
    computed: {
        ...mapState([
            'ShoppingCarts'
        ]),
        Zongji:function(){
            let reslut =0;
            this.ShoppingCarts.forEach(item=>{
                reslut+=item.number*item.realPrice;
            });

            return reslut;
        }
    },
    methods:{
        ...mapMutations([
            'SaveShoppingCarts'
        ]),
        createOrder(){
            if(this.ShoppingCarts.length==0){
                return ;
            }
            let data={
                data:[]
            };
            this.ShoppingCarts.forEach(element => {
                data.data.push({productId:element.productId,number:element.number});
            });
            data.address="深圳";
            data.phone= "13500000000";

            orderService.CreateOrder(data).then(resp=>{
                window.console.log(resp);
            })
        },
        close(){
            this.IsShowShoppingCart=false;
        },
        GetShoppingCartList(){
            orderService.GetShoppingCartList().then(resp=>{
                window.console.log(resp);
                this.SaveShoppingCarts(resp.data);
            });
        },
        
    },
    created(){
        this.GetShoppingCartList();
        bus.$on("ShowShoppingCart",data=>{
            window.console.log(data);
            this.GetShoppingCartList();
            this.IsShowShoppingCart=data;
        });
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
