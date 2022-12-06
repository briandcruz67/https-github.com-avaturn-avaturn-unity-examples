function setupVtoFrame(subdomain) {
    vtoFrame.src = "https://avaturn.me/iframe";
    window.addEventListener("message", subscribe);
    document.addEventListener("message", subscribe);

    function subscribe(event) {
        /* Here we process the events from the iframe */
        const json = parse(event);

        if (gameInstance == null ||
            json?.source !== "avaturn" ||
            json?.eventName == null) {
            return;
        }

        // Get avatar GLB URL
        if (json.eventName === 'v1.avatar.exported') {
            let url = window.URL.createObjectURL(dataURItoBlob(json.data.blobURI));
            console.log(`Avatar URL: ${url}`);
            gameInstance.SendMessage(
                "AvatarReceiver",
                "GetFromVtoFrame",
                url
            );
            vtoContainer.style.display = "none";
        }
    }
    
    function parse(event) {
        try {
            return JSON.parse(event.data);
        } catch (error) {
            return null;
        }
    }
    
    function dataURItoBlob(dataURI) {
        var mime = dataURI.split(',')[0].split(':')[1].split(';')[0];
        var binary = atob(dataURI.split(',')[1]);
        var array = [];
        for (var i = 0; i < binary.length; i++) {
            array.push(binary.charCodeAt(i));
        }
        return new Blob([new Uint8Array(array)], {
            type: mime
        });
    }
}

function displayVto() {
    console.log("Display");
    vtoContainer.style.display = "block";
}

function hideVto() {
    console.log("Hide");
    vtoContainer.style.display = "none";
}