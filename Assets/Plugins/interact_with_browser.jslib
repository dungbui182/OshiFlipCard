mergeInto(LibraryManager.library, {

  ReloadPage: function () {
	  location.reload();
  },
  
  IsMobileBrowser: function () {
    return (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent));
  }
  
});