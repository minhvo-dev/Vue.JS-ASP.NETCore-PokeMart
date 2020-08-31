<template>
  <nav>
    <v-toolbar color="primary" class="white--text">
      <v-app-bar-nav-icon @click="open = !open" class="white--text"></v-app-bar-nav-icon>
      <v-toolbar-title align="center" justify="center" class="text-center">
        <span>Menu</span>
      </v-toolbar-title>
      <v-spacer></v-spacer>
    </v-toolbar>
    <v-menu v-model="open" app class="secondary">
      <v-list>
        <v-list-item v-for="link in links" :key="link.text" router :to="link.route">
          <v-list-item-content>
            <v-list-item-title>{{ link.text }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-menu>
  </nav>
</template>

<script>
export default {
  data: () => ({
    open: false,
    links: [
      { text: "Home", route: "/" },
      { text: "Regions", route: "/brands" },
      { text: "Order History", route: "/orderlist" },
      { text: "Map", route: "/branches" },
      { text: "Register", route: "/register" },
      { text: "Login", route: "/login" },
      { text: "Logout", route: "/logout" }
    ]
  })
};
</script>