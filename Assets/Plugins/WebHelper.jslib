mergeInto(LibraryManager.library, {

    ShowAvaturnIframeJS: function () {
        displayIframe();
    },

    Aewrwer: function () {

    },

    HideAvaturnIFrameJS: function () {
        hideIframe();
    },
        
    SetupAvaturnIframeJS: function (subdomain){
        //subdomain = 'demo';
        setupIframe(UTF8ToString(subdomain));
        // setupIframe(subdomain);
    },
});