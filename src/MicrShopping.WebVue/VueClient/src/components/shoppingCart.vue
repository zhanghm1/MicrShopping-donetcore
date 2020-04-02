<template>
  <div class="shoppingCart">
  </div>
</template>

<script>
import orderService from "../service/orderService" ;

export default {
    name: 'shoppingCart',
    props: {
        productId: String,
        productPrice:String
    },
    data(){
        return {
            productList:[]
        }
    },
    methods:{
        createOrder(){
            if(this.productList.length==0){
                return ;
            }
            let data={};
            this.productList.forEach(element => {
                data.data.push({productId:element.id,number:element.number});
            });
            data.address="深圳";
            data.phone= "13500000000";

            orderService.PostEditOrder(data).then(resp=>{
                window.console.log(resp);
            })
        }
    },
    created(){
        orderService.GetShoppingCartList().then(resp=>{
            window.console.log(resp);
        });
    }
}
</script>

<style>

</style>
