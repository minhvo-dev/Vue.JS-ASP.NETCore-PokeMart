<template>
  <v-container justify="center">
    <v-row>
      <v-col cols="8">
        <v-card class="display-1 text-center purple--text text--darken-4">Shopping Cart</v-card>
      </v-col>
      <v-col cols="4">
        <v-card class="display-1 text-center">&#x1F6D2;</v-card>
      </v-col>
    </v-row>
    <v-row style="color:green;" justify="center" align="center">{{status}}</v-row>
    <!-- items in cart and total price -->
    <div v-if="this.cart.length > 0" style="max-height:50vh;">
      <!-- header -->
      <v-row class="title text-center purple--text text--darken-4 mt-5">
        <v-col class="pa-0 ma-0" cols="2">Id</v-col>
        <v-col class="pa-0 ma-0 text-left" cols="3">Name</v-col>
        <v-col class="pa-0 ma-0" cols="1">Qty</v-col>
        <v-col class="pa-0 ma-0" cols="2">Price</v-col>
        <v-col class="pa-0 ma-0" cols="4">Ext</v-col>
      </v-row>
      <!-- items in cart -->
      <div class="purple--text text--darken-3" style="max-height:45vh;overflow:auto;">
        <v-row v-for="item in cart" :key="item.id">
          <v-col cols="2">{{item.id}}</v-col>
          <v-col cols="3">{{item.product.productName}}</v-col>
          <v-col cols="1">{{item.qty}}</v-col>
          <v-col cols="2">{{item.product.msrp | currency}}</v-col>
          <v-col cols="4" class="text-right">{{(item.qty * item.product.msrp) | currency}}</v-col>
        </v-row>
      </div>
      <!-- calculated fields: subtotal, tax, total  -->
      <v-row class="font-weight-medium mt-2 mb-1 pb-0 purple--text text--darken-4 mt-5">
        <v-col cols="8" class="text-right mb-1 pb-0">Sub total:</v-col>
        <v-col
          cols="4"
          class="text-right mb-1 pb-0"
          style="border-top:solid 1px"
        >{{subTotal | currency}}</v-col>
      </v-row>
      <v-row class="font-weight-medium my-1 py-0 purple--text text--darken-4">
        <v-col cols="8" class="text-right my-1 py-0">Tax:</v-col>
        <v-col cols="4" class="text-right my-1 py-0">{{(subTotal * 0.13) | currency}}</v-col>
      </v-row>
      <v-row class="font-weight-bold mb-2 mt-1 pt-0 purple--text text--darken-4">
        <v-col cols="8" class="text-right mt-1 pt-0">Total:</v-col>
        <v-col cols="4" class="text-right mt-1 pt-0">{{(subTotal * 1.13) | currency}}</v-col>
      </v-row>
      <v-row align="center" justify="center" class="d-flex">
        <v-btn medium outlined class="primary white--text mr-2" @click="clearCart">Clear Cart</v-btn>
        <v-btn medium outlined class="primary white--text ml-2" @click="addOrder">Order Now</v-btn>
      </v-row>
    </div>
  </v-container>
</template>>

<script>
import fetcher from "../mixins/fetcher";

export default {
  name: "CartDetails",
  data() {
    return {
      subTotal: 0,
      cart: [],
      status: ""
    };
  },
  beforeMount: function() {
    if (sessionStorage.getItem("cart")) {
      this.cart = JSON.parse(sessionStorage.getItem("cart"));
      this.cart.map(item => (this.subTotal += item.product.msrp * item.qty));
    } else {
      this.cart = [];
    }
  },
  mixins: [fetcher],
  methods: {
    clearCart: function() {
      sessionStorage.removeItem("cart");
      this.cart = [];
      this.subTotal = 0;
      this.status = "Cart cleared";
    },
    addOrder: async function() {
      let user = JSON.parse(sessionStorage.getItem("user"));
      try{
        this.status = "Sending order to server";
        let orderHelper = {
          email: user.email, 
          selections: this.cart
        };
        let msg = await this.$_postdata("order", orderHelper);
        if(msg.indexOf("ID") > 0){ // order added
          this.clearCart();
        }
        this.status = msg;
      }
      catch(err){
        console.log(err);
        this.status = `Error addOrder: ${err}`;
      }
    }
  }
};
</script>