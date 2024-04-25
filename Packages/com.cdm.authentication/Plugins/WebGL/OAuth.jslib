mergeInto(LibraryManager.library, {
  StartAuth: function (url, callback) {
    const stringUrl = UTF8ToString(url);
    const windowFeatures = 'toolbar=no, menubar=no, width=600, height=700, top=100, left=100';

    window.popupCompleted = (redirectUrl) => {
      let bufferUrl = stringToNewUTF8(stringUrl);
      let bufferRedirectUrl = stringToNewUTF8(redirectUrl);

      {{{ makeDynCall('vii', 'callback') }}}(bufferUrl, bufferRedirectUrl);
      _free(bufferUrl);
      _free(bufferRedirectUrl);
    }

    windowReference = window.open(stringUrl, "Authentication", windowFeatures);
    windowReference.focus();
  },
});