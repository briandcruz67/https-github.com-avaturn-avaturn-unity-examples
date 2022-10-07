function setupVtoFrame(subdomain) {
    const glb_url = encodeURIComponent('local/dec1_all_emb.glb');
    console.log(glb_url);
    vtoFrame.src = "https://vto.in3d.io/?avatar_link=" + glb_url + "&5model_with_hair=1";
    window.addEventListener("message", subscribe);
    document.addEventListener("message", subscribe);

    function subscribe(event) {
        /* Here we process the events from the iframe */
        const json = parse(event);

        if (gameInstance == null ||
            json?.source !== "in3D" ||
            json?.eventName == null) {
            return;
        }

        // Get avatar GLB URL
        if (json.eventName === 'avatar.exported') {
            let url = window.URL.createObjectURL(dataURItoBlob(json.blobURI));
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
    vtoContainer.style.display = "block";
}

function hideVto() {
    vtoContainer.style.display = "none";
}