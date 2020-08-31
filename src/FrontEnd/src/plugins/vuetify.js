import Vue from 'vue';
import Vuetify from 'vuetify/lib';
import colors from "vuetify/lib/util/colors" // import color lib

Vue.use(Vuetify);

export default new Vuetify({
    theme: {
        themes: {
            light: {
                primary: colors.purple.darken4, // #E53935
                secondary: colors.red.lighten4, // #FFCDD2
                accent: colors.deepPurple.lighten5,
                error: colors.red.darken2,
                success: colors.green.darken2
            }
        }
    }
});
