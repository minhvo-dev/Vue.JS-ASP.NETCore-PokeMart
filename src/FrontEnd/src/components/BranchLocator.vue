<template>
  <v-container fluid class="purple--text text--darken-4">
    <v-row class="justify-center text-center display-2" style="margin-top:.5em;">Map</v-row>
    <v-row>
      <v-img
        :src="require('../assets/img/map.png')"
        class="mt-5"
        style="height:256px;"
        aspect-ratio="1"
        contain
      />
    </v-row>
    <v-row>
      <v-col class="justify-center text-center display-" style="margin-top:.5em;">
        Please enter your location
        <br />to search for three closest PokéMarts
      </v-col>
    </v-row>
    <v-row justify="center" align="center">
      <v-col>
        <v-text-field
          color="primary"
          label="Enter location"
          v-model="address"
          required
          hint="e.g. London, ON or N5Y-6M4"
          persistent-hint
        ></v-text-field>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-btn
        @click="getLatAndLng()"
        :class=" { 'purple darken-4 white--text' :valid, disabled: !valid }"
      >Search</v-btn>
    </v-row>

    <v-dialog v-model="dialog" v-if="valid" class="justify-center align-center">
      <v-card class="purple--text text--darken-4 pa-0 ma-0">
        <v-row class="d-flex flex-row-reverse pa-0 ma-0">
          <v-btn
            class="pa-0 ma-0 purple--text text--darken-4"
            text
            @click="dialog = false"
            style="font-size:XX-large"
          >&times;</v-btn>
        </v-row>
        <v-row class="justify-center">
          <div
            id="map"
            ref="map"
            class="googlemap"
            style="margin-bottom:2vh;height:75vh;width:80vw;min-height:300px;"
          ></div>
        </v-row>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
import fetcher from "../mixins/fetcher";

export default {
  data() {
    return {
      address: "N5Y-6M4", // my postal code
      valid: true,
      dialog: false,
      map: null
    };
  },
  mixins: [fetcher],
  methods: {
    getLatAndLng: async function() {
      try {
        // A service for converting between an address to LatLng
        let geocoder = new window.google.maps.Geocoder();
        await geocoder.geocode(
          { address: this.address },
          async (results, status) => {
            if (status === window.google.maps.GeocoderStatus.OK) {
              // only if google gives us the OK
              // get the lat and lng of the selected pos
              let lat = results[0].geometry.location.lat();
              let lng = results[0].geometry.location.lng();
              // convert lat and lng to google latlgn object
              let myLatLng = new window.google.maps.LatLng(lat, lng);
              let options = {
                zoom: 13,
                center: myLatLng,
                mapTypeId: window.google.maps.MapTypeId.ROADMAP
              };
              this.map = new window.google.maps.Map(this.$refs["map"], options);

              let center = this.map.getCenter();
              this.map.setCenter(center);
              window.google.maps.event.trigger(this.map, "resize");

              /* Add custom pins to the map */
              let infowindow = new window.google.maps.InfoWindow({
                content: ""
              });
              // pin the user location to the map
              let marker = new window.google.maps.Marker({
                position: new window.google.maps.LatLng(lat, lng),
                map: this.map,
                animation: window.google.maps.Animation.DROP,
                icon: "img/user_marker.png",
                title: "Your location",
                html: `<div>Your location</div>`
              });
              window.google.maps.event.addListener(marker, "click", () => {
                infowindow.setContent(marker.html);
                infowindow.close();
                infowindow.open(this.map, marker);
              });
              // pin the closest branches to the map
              let details = await this.$_getdata(`branch/${lat}/${lng}`);
              console.log(details);
              if (details.length != 0) {
                let iImg = 0;
                details.map(detail => {
                  ++iImg;
                  let imgPath = `/img/marker${iImg}.png`;
                  let marker = new window.google.maps.Marker({
                    position: new window.google.maps.LatLng(
                      detail.latitude,
                      detail.longitude
                    ),
                    animation: window.google.maps.Animation.DROP,
                    icon: imgPath,
                    title: `PokéMart#${detail.id} ${detail.street}, ${detail.city},${detail.region}`,
                    html: `<div>PokéMart#${detail.id}<br/>${detail.street}, ${
                      detail.city
                    }<br/>${detail.distance.toFixed(2)} km</div>`,
                    visible: true
                  });
                  window.google.maps.event.addListener(marker, "click", () => {
                    infowindow.setContent(marker.html); // added .html to the marker object
                    infowindow.close();
                    infowindow.open(this.map, marker);
                  });
                  marker.setMap(this.map);
                });
              } else {
                throw new Error("Received no stores");
              }
            }
          }
        );
        this.dialog = true;
      } catch (error) {
        console.log(error);
      }
    }
  }
};
</script>