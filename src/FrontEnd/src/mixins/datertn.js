// mixin that format the date to "yyyy-MM-dd hh:mm"
export default {
  methods: {
    formatDate(date) {
      let d;
      // check if date is coming from server
      if (date === undefined) {
        d = new Date();
      }
      else {
        // parse date string to date object
        d = new Date(Date.parse(date));
      }
      // get parts of date to display
      let _day = d.getDate();
      if (_day < 10) {
        _day = "0" + _day;
      }
      let _month = d.getMonth() + 1;
      if (_month < 10) {
        _month = "0" + _month;
      }
      let _year = d.getFullYear();
      let _hour = d.getHours();
      if (_hour < 10) {
        _hour = "0" + _hour;
      }
      let _min = d.getMinutes();
      if (_min < 10) {
        _min = "0" + _min;
      }

      return _year + "-" + _month + "-" + _day + " " + _hour + ":" + _min;
    }
  }
};