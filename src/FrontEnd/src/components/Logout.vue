<template>
  <v-container fluid>
    <v-row align="center" justify="center">
      <v-img 
        :src="require('../assets/img/title.png')"
        class="mt-3 py-0"
        style="height:200px;"
        aspect-ratio="1"
        contain />
    </v-row>
    
    <v-row align="center" justify="center" class="mt-5">
      <div class="title purple--text text--darken-4">Have A Great Adventure, Trainer!</div> 
    </v-row>
    <v-row>
      <v-img
        :src="require('../assets/img/logout.png')"
        class="py-0 mb-5"
        style="height:100px;"
        aspect-ratio="1"
        contain
      />
    </v-row>
  </v-container>
</template>
<script>
export default {
  data() {
    return {};
  },
  created: function() {
    sessionStorage.clear();
  }
};
</script>