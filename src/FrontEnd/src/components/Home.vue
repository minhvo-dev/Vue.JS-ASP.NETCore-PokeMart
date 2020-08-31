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
    <v-row>
      <v-img
        :src="require('../assets/img/pokemart.png')"
        class="py-0"
        style="height:100px;margin-top:-30px;"
        aspect-ratio="1"
        contain
      />
    </v-row>
  </v-container>
</template>
<script>
export default {};
</script>