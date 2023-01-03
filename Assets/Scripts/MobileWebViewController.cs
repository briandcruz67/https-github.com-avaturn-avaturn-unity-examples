using UnityEngine;

public class MobileWebViewController : MonoBehaviour
{
    [SerializeField] private UniWebView webView;

    // Start is called before the first frame update
    void Start()
    {
        webView.OnPageFinished += WebViewOnOnPageFinished;
    }

    private void WebViewOnOnPageFinished(UniWebView webview, int statuscode, string url)
    {
        webView.OnPageFinished -= WebViewOnOnPageFinished;


        string jsCode = @"
        window.addEventListener('message', subscribe);
        document.addEventListener('message', subscribe);
        function subscribe(event) {
            const json = parse(event);
            if (json?.source !== 'avaturn' || json?.eventName == null) {
                return;
            }
            if (json.eventName === 'v2.avatar.exported') {
                
                if(json_data.urlType == 'dataURL') {
                    alert('Can't use dataURL for mobile embed, please use httpURL, reach out to Avaturn team to setup.');
                } 

                let url = json_data.url;
                
                location.href = 'uniwebview://action?avatar_link=' + encodeURIComponent(url);
            }

            function parse(event) {
                try {
                    return JSON.parse(event.data);
                } catch (error) {
                    return null;
                }
            }
        }
        ";
        
        webView.AddJavaScript(jsCode, (payload) => {
            if (payload.resultCode.Equals("0")) {
                print("Adding JavaScript Finished without error.");
            }
        });
    }

    public void ShowView(bool show)
    {
        if (show) webView.Show();
        else webView.Hide();
    }
}
