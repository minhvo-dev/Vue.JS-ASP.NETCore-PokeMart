<template>
  <v-container class="purple--text text--darken-4">
    <v-row justify="center" class="text-center display-2" style="margin-top:.5em;">Register</v-row>
    <v-row flex align-center>
      <v-col xs12 sm4 elevation-6>
        <v-card style="height:300px;">
          <v-card-text class="pt-4">
            <div>
              <v-form v-model="isValid" ref="form">
                <v-text-field
                  color="primary"
                  label="Enter your first name"
                  v-model="firstname"
                  :rules="[rules.required]"
                  required
                ></v-text-field>
                <v-text-field
                  color="primary"
                  label="Enter your surname"
                  v-model="lastname"
                  :rules="[rules.required]"
                  required
                ></v-text-field>
                <v-text-field
                  color="primary"
                  label="Enter your e-mail address"
                  v-model="email"
                  :rules="[rules.required]"
                  required
                ></v-text-field>
                <v-text-field
                  color="primary"
                  label="Enter your password"
                  v-model="password"
                  min="4"
                  :append-icon="showPW ? 'mdi-eye' : 'mdi-eye-off'"
                  :rules="[rules.required, rules.min]"
                  :type="showPW ? 'text' : 'password'"
                  required
                  @click:append="showPW = !showPW"
                ></v-text-field>
              </v-form>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-btn
        @click="register"
        :class=" { 'primary white--text': isValid, disabled: !isValid }"
        :disabled="isValid===false"
      >Register</v-btn>
    </v-row>
    <v-row class="green--text" align="center" justify="center" style="margin-top:5vh;">
      {{status}}
    </v-row>
  </v-container>
</template>

<script>
import fetcher from "../mixins/fetcher";

export default {
  name: "Register",
  data() {
    return {
      email: "",
      password: "",
      firstname: "",
      lastname: "",
      showPW: false,
      rules: {
        required: value => !!value || "Required.",
        min: v => v.length >= 4 || "Min 4 characters"
      },
      isValid: false,
      status: ""
    };
  },
  methods: {
    register: async function() {
      try {
        this.status = "registering with server";
        let userHelper = {
          firstname: this.firstname,
          lastname: this.lastname,
          email: this.email,
          password: this.password
        };
        let payload = await this.$_postdata("register", userHelper); // in mixin
        this.status = payload.token;
      } catch (err) {
        console.log("here" + err);
        this.status = `Error has occured: ${err}`;
      }
    }
  },
  mixins: [fetcher]
};
</script>