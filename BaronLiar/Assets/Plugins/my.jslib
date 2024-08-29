mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
	console.log("Hello, world!");
  },

  HelloString: function (str) {
    window.alert(UTF8ToString(str));
  },
});