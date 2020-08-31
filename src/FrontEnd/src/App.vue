<template>
  <v-app>
    <v-content>
      <NavBar />
      <router-view />
      <v-footer absolute class="headline" color="primary">
        <v-col class="text-center white--text" cols="12">
          &copy;2020 — PokéMart
        </v-col>
      </v-footer>
    </v-content>
  </v-app>
</template>

<script>
import NavBar from "./components/NavBar";
export default {
  name: "App",
  components: {
    NavBar
  }
};
</script>