<template>
  <v-container fluid class="purple--text text--darken-4">
    <v-row justify="center" class="text-center display-2" style="margin-top:.5em;">Previous Orders</v-row>
    <v-row justify="center">
      <v-col class="title text-center green--text">{{status}}</v-col>
    </v-row>
    <!-- order list item -->
    <div v-if="orders.length > 0">
      <!-- heading -->
      <v-row class="justify-center text-center title accent">
        <v-col cols="3">Order#</v-col>
        <v-col cols="5">Date</v-col>
        <v-col cols="4">Total</v-col>
      </v-row>
      <!-- body -->

      <v-list class="overflow-y-auto" style="max-height:45vh;">
        <v-list-item-group>
          <v-list-item
            v-for="(order, i) in orders"
            :key="i"
            class="ma-0 px-2"
            :class="{accent: i % 2 == 1}"
            @click="popDialog(order)"
          >
            <v-col cols="3" class="purple--text text--darken-4 ma-0 pa-0 text-left">{{order.id}}</v-col>
            <v-col
              cols="5"
              class="purple--text text--darken-4 ma-0 pa-0 text-center"
            >{{formatDate(order.orderDate)}}</v-col>
            <v-col
              cols="4"
              class="purple--text text--darken-4 ma-0 pa-0 text-right"
            >{{order.totalAmount * 1.13 | currency}}</v-col>
          </v-list-item>
        </v-list-item-group>
      </v-list>
    </div>
    <!-- popup dialog -->
    <v-dialog
      v-model="dialog"
      v-if="selectedOrder !== null"
      class="justify-center align-center"
      wrap
    >
      <v-card class="purple--text text--darken-4 pa-0 ma-0">
        <!-- close button -->
        <v-row class="d-flex flex-row-reverse pa-0 ma-0">
          <v-btn
            class="pa-0 ma-0 purple--text text--darken-4"
            text
            @click="dialog = false"
            style="font-size:XX-large;"
          >&times;</v-btn>
        </v-row>
        <!-- headline with image  -->
        <v-row class="d-flex align-center justify-center ma-1 pa-0">
          <v-col class="ma-1">
            <div class="text-center headline">{{`Order #${selectedOrder.id}`}}</div>
            <div class="text-center body-1">{{formatDate(selectedOrder.orderDate)}}</div>
          </v-col>
          <v-col class="ma-1 pa-0">
            <v-img
              :src="require('../assets/img/shoppingbag.png')"
              height="15vh"
              width="15vh"
              contain
              aspect-ratio="1"
            />
          </v-col>
        </v-row>
        <!-- headings -->
        <v-row
          class="subtitle-1 title align-center justify-center text-center purple--text text--darken-4 ma-0 pa-0"
        >
          <v-col class="ma-0 pa-0 pl-1 text-left" cols="3">Name</v-col>
          <v-col class="ma-0 pa-0" cols="3">
            <div class="text-center">Quantity</div>
            <v-row class="subtitle-2 ma-0 pa-0">
              <v-col class="ma-0 pa-0" cols="4">O</v-col>
              <v-col class="ma-0 pa-0" cols="4">S</v-col>
              <v-col class="ma-0 pa-0" cols="4">B</v-col>
            </v-row>
          </v-col>
          <v-col class="ma-0 pa-0" cols="3">MSRP</v-col>
          <v-col class="ma-0 pa-0" cols="3">Extended</v-col>
        </v-row>
        <!-- order details -->
        <v-row
          v-for="(detail, i) in selectedOrderDetails"
          :key="i"
          style="max-height:40vh;margin:0;padding:0;"
          :class="{accent: i % 2 == 0}"
        >
          <v-col class="ma-0 pa-0 pl-1" cols="3">{{detail.productName}}</v-col>
          <v-col class="ma-0 pa-0 text-center" cols="1">{{detail.qtyOrdered}}</v-col>
          <v-col class="ma-0 pa-0 text-center" cols="1">{{detail.qtySold}}</v-col>
          <v-col class="ma-0 pa-0 text-center" cols="1">{{detail.qtyBackOrdered}}</v-col>
          <v-col class="ma-0 pa-0 pr-1 text-right" cols="3">{{detail.msrp | currency}}</v-col>
          <v-col class="ma-0 pa-0 pr-1 text-right" cols="3">{{detail.qtySold * detail.msrp | currency}}</v-col>
        </v-row>
        <!-- calculated fields: subtotal, tax, total -->
        <v-row class="mt-5 mb-1 mx-0 pa-0 text-right font-weight-medium">
          <v-col class="ma-0 pa-0 pt-2 pr-5" cols="8">Sub Total:</v-col>
          <v-col class="ma-0 pt-2 pr-1 pl-0 pb-0" style="border-top:solid 1px" cols="4">{{selectedOrder.totalAmount | currency}}</v-col>
        </v-row>
        <v-row class="ma-0 mb-1 pa-0 text-right font-weight-medium">
          <v-col class="ma-0 pa-0 pr-5" cols="8">Tax:</v-col>
          <v-col class="ma-0 pa-0 pr-1" cols="4">{{selectedOrder.totalAmount * 0.13 | currency}}</v-col>
        </v-row>
        <v-row class="ma-0 pa-0 text-right font-weight-bold">
          <v-col class="ma-0 pa-0 pr-5 pb-5" cols="8">Order Total:</v-col>
          <v-col class="ma-0 pa-0 pr-1 pb-5" cols="4">{{selectedOrder.totalAmount * 1.13 | currency}}</v-col>
        </v-row>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
import fetcher from "../mixins/fetcher";
import datertn from "../mixins/datertn";

export default {
  name: "OrderList",
  data() {
    return {
      orders: [],
      status: "",
      dialog: false,
      selectedOrder: null,
      selectedOrderDetails: {},
      dialogStatus: ""
    };
  },
  mixins: [fetcher, datertn],
  beforeMount: async function() {
    try {
      let user = JSON.parse(sessionStorage.getItem("user"));
      this.status = "fetching orders from server...";
      let route = this.$_buildRouteWithParams("order", user.email.trimEnd());
      this.orders = await this.$_getdata(route);
      this.status = `loaded ${this.orders.length} orders`;
    } catch (error) {
      console.log(error);
      this.status = `Error has occured: ${error.message}`;
    }
  },
  methods: {
    // get selected order from server
    getSelectedOrder: async function(orderId) {
      if (orderId > 0) {
        // make sure that the order id is valid
        try {
          let user = JSON.parse(sessionStorage.getItem("user")); // get user information from sessionStorage
          this.status = `fetching details for order #${orderId}...`;
          // build the route match the order controller
          let route = this.$_buildRouteWithParams(
            "order",
            orderId,
            user.email.trimEnd()
          );
          this.selectedOrderDetails = await this.$_getdata(route);
          this.status = `found order #${orderId} details`;
        } catch (error) {
          console.log(error);
          this.status = `Error has occured: ${error.message}`;
        }
      }
    },
    popDialog: async function(order) {
      this.dialogStatus = "";
      this.dialog = true;
      this.selectedOrder = order;
      await this.getSelectedOrder(order.id);
    }
  }
};
</script>