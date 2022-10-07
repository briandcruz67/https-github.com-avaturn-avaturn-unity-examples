mergeInto(LibraryManager.library, {

    ShowVtoFrame: function () {
        displayVto();
    },
  
    HideVtoFrame: function () {
        hideVto();
    },
        
    SetupVto: function (partner){
        setupVtoFrame(UTF8ToString(partner));
    },
});