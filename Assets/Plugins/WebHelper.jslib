mergeInto(LibraryManager.library, {

    ShowAvaturnIframeJS: function () {
        displayIframe();
    },

    HideAvaturnIFrameJS: function () {
        hideIframe();
    },
        
    SetupAvaturnIframeJS: function (subdomain){
        setupIframe(UTF8ToString(subdomain));
    },
});