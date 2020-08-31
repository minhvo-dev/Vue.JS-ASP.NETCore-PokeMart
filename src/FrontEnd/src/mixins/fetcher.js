// define a mixin object
export default {
  data() {
    return {
      // SERVERURL: `https://localhost:44313/api/` // local host
      SERVERURL: "/api/" // remote
    };
  },
  methods: {
    // GET data from the server, use fetch method with a given api
    $_getdata: async function (apicall) {
      let payload = {};
      let headers = this.buildHeaders();
      try {
        let response = await fetch(`${this.SERVERURL}${apicall}`, {
          method: "GET",
          headers: headers
        });
        payload = await response.json();
        //console.log(payload);
      } catch (err) {
        console.log(err);
        payload = err;
      }
      return payload;
    },
    // POST data to the server, use fetch method with a given api and data
    $_postdata: async function (apicall, data) {
      let payload = JSON.stringify(data);
      let headers = {};
      if (apicall === "register" || apicall == "login") {
        headers = { "Content-Type": "application/json; charset=utf-8" };
      }
      else {
        headers = this.buildHeaders();
      }
      try {
        let response = await fetch(`${this.SERVERURL}${apicall}`, {
          method: "POST",
          headers: headers,
          body: payload
        });
        payload = await response.json();
      } catch (error) {
        payload = error;
      }
      return payload;
    },
    // Build the header which contains the token that received from the server
    buildHeaders: function () { // build a header with authorized token
      const myHeaders = new Headers();
      const user = JSON.parse(sessionStorage.getItem("user")); // get the token
      myHeaders.append("Content-Type", "application/json");
      myHeaders.append("Authorization", "Bearer " + user.token);
      return myHeaders;
    },
    // Concatenate string to build a route without the '/' character at the end
    $_buildRouteWithParams: function (...params) {
      let route = "";
      params.forEach(param=>route += `${param}/`); // route = param1/param2/param3/
      return route.slice(0,-1); // remove the last '/' char
    }
  }
};