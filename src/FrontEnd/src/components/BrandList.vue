<template>
  <v-container fluid class="purple--text text--darken-4">
    <v-row justify="center" class="text-center display-2" style="margin-top:.5em;">Regions</v-row>
    <v-row justify="center">
      <v-col class="title text-center green--text">{{status}}</v-col>
    </v-row>
    <v-row justify="center">
      <v-col class="text-left display-1">
        <v-select
          :items="brands"
          item-text="name"
          style="max-height: 50%;"
          item-value="id"
          @input="changeBrand"
          v-model="selectedId"
        ></v-select>
      </v-col>
    </v-row>
    <!-- display list item -->
    <div v-if="products.length">
      <v-row justify="center" class="text-center headline">Pokémon</v-row>
      <v-row justify="center" style="margin-top:1vh;">
        <v-col class="text-left display-2">
          <v-list style="max-height: 50vh;" class="overflow-y-auto">
            <v-list-item-group>
              <v-list-item
                v-for="(product, i) in products"
                :key="i"
                @click="popDialog(product)"
                :class="{accent: i % 2 == 0}"
              >
                <v-col style="width:25%;">
                  <v-img
                    :src="require('../assets/img/products/animated/' + product.graphicName)"
                    class="my-3"
                    style="height:10vh;width:10vh;"
                    aspect-ratio="1"
                    contain
                  />
                </v-col>
                <v-col style="width:75%;">
                  <v-list-item-content>
                    <v-list-item-title
                      class="title purple--text text--darken-4"
                      v-text="product.productName"
                    >></v-list-item-title>
                  </v-list-item-content>
                </v-col>
              </v-list-item>
            </v-list-item-group>
          </v-list>
        </v-col>
      </v-row>
    </div>
    <!-- popup dialog -->
    <v-dialog v-model="dialog" v-if="selectedProduct !== null" justify="center" align="center" wrap>
      <v-card class="purple--text text--darken-4 pa-0 ma-0">
        <v-row class="d-flex flex-row-reverse pa-0 ma-0">
          <v-btn class="pa-0 ma-0 purple--text text--darken-4" text @click="dialog = false" style="font-size:XX-large">&times;</v-btn>
        </v-row>
        <v-row class="d-flex align-center ma-1 pa-0">
          <v-col class="text-center headline ma-1 pa-0">{{selectedProduct.productName}}</v-col>
          <v-col class="ma-1 pa-0">
            <v-img
              :src="require('../assets/img/products/animated/' + selectedProduct.graphicName)"
              height="15vh"
              width="15vh"
              contain
              aspect-ratio="1"
            />
          </v-col>
        </v-row>
        <v-row class="title text-uppercase ma-1 pa-0">
          <v-col class="text-center ma-1 pa-0" style="width:30%;">MSRP:</v-col>
          <v-col class="text-left ma-1 pa-0" style="width:70%;">{{selectedProduct.msrp | currency}}</v-col>
        </v-row>
        <!-- description -->
        <v-row justify="center" class="title ma-0 pa-0">Description</v-row>
        <v-card class="overflow-y-auto px-5 mx-2 mb-4 accent" style="max-height:35vh;">
          <v-row
            class="font-italic purple--text text--darken-3"
            style="margin-left:2vw;margin-right:2vw;"
          >{{selectedProduct.description}}</v-row>
        </v-card>
        <!-- Stock information -->
        <v-row class="align-center ma-0 pa-0">
          <v-col cols="6" class="text-right my-0 py-0">In stock:</v-col>
          <v-col cols="6" class="text-right my-0 py-0">
            <div class="text-right" style="width:15vw;">{{selectedProduct.qtyOnHand}}</div>
          </v-col>
        </v-row>
        <!-- quantity input -->
        <v-row class="align-center ma-0 pa-0">
          <v-col cols="6" class="text-right">Quantity:</v-col>
          <v-col>
            <input
              cols="6"
              type="number"
              maxlength="3"
              placeholder="0"
              size="3"
              style="width:15vw;border-bottom:solid;text-align:right;"
              v-model="qty"
            />
          </v-col>
        </v-row>
        <!-- buttons -->
        <v-row class="d-flex justify-center align-center mt-5 pa-0">
          <v-btn
            style="width:10em;"
            class="mr-2 primary white--text"
            medium
            outlined
            @click="addToCart"
          >Add To Cart</v-btn>
          <v-btn
            style="width:10em;"
            class="ml-2 primary white--text"
            medium
            outlined
            @click="viewCart"
          >View Cart</v-btn>
        </v-row>
        <!-- status display -->
        <v-row
          justify="center"
          align="center"
          class="ml-0 mr-0 pl-0 pr-0 mt-2 pb-2"
          style="color:green;"
        >{{this.dialogStatus}}</v-row>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
import fetcher from "../mixins/fetcher"; // import the mixin fetcher

export default {
  name: "BrandList",
  data() {
    return {
      brands: [], // list of brands
      status: {},
      selectedId: 0, // selected brand
      products: [], // list of products
      dialog: false, // control the dialog
      selectedProduct: null, // selected product object
      dialogStatus: "",
      qty: 0, // number of products add to cart
      cart: []
    };
  },
  mixins: [fetcher], // add fetcher to the mixins list
  mounted: async function() {
    // don't use arrow function here
    try {
      this.status = "fetching regions from server...";
      //let response = await fetch(`https://localhost:44313/api/brand`);
      //this.brands = await response.json();
      this.brands = await this.$_getdata("brand"); // fetchdata is in the fetcher mixin
      this.status = `loaded ${this.brands.length} regions`;
      this.brands.unshift({ name: "Current Region", id: 0 });
    } catch (err) {
      console.log(err);
      this.status = `Error has occured: ${err.message}`;
    }
    if (sessionStorage.getItem("cart")) {
      this.cart = await JSON.parse(sessionStorage.getItem("cart"));
    }
  },
  methods: {
    changeBrand: async function(brandId) {
      if (brandId > 0) {
        try {
          this.status = `fetching pokémon for ${brandId}...`;
          this.products = await this.$_getdata(`product/${brandId}`);
          this.status = `found ${this.products.length} pokémon`;
        } catch (err) {
          console.log(err);
          this.status = `Error has occured: ${err.message}`;
        }
      } else {
        this.products = [];
      }
    },
    popDialog: function(product) {
      this.dialogStatus = "";
      this.dialog = true; // show to dialog
      this.selectedProduct = product;
    },
    addToCart: function() {
      const index = this.cart.findIndex(
        // check if this product is already in cart
        product => product.id === this.selectedProduct.id
      );
      if (this.qty !== "0") {
        // user add product to the cart
        let obj = {
          id: this.selectedProduct.id,
          qty: parseInt(this.qty),
          product: this.selectedProduct
        };
        if (index === -1) {
          // product is not in the cart
          this.cart.push(obj); // add product and quantity to the cart
        } else {
          // product is already in the cart
          this.cart[index] = obj; // replace the exist product by the new one
        }
        // update the status
        this.dialogStatus = `${this.qty} ${this.selectedProduct.productName}(s) added`;
      } else {
        // user remove product from the cart
        if (index !== -1) {
          // check if the index is valid
          this.cart.splice(index, 1); // remove from the cart
          this.dialogStatus = `${this.selectedProduct.productName}(s) removed`;
        }
      }
      sessionStorage.setItem("cart", JSON.stringify(this.cart));
    },
    viewCart: function() {
      this.$router.push({
        name: "cart"
      });
    }
  }
};
</script>