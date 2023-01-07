using UnityEngine;
using UnityEngine.Android;

public class MobileWebViewController : MonoBehaviour
{
    private UniWebView webView;
    [SerializeField] private GameObject webViewGameObject;
    [SerializeField] private RectTransform webViewFrame;
    [SerializeField] private AvatarReceiver _avatarReceiver;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        Permission.RequestUserPermission(Permission.Camera);
        Permission.RequestUserPermission(Permission.Microphone);
#endif
#if UNITY_IOS || UNITY_ANDROID
        UniWebView.SetAllowAutoPlay(true);
        UniWebView.SetAllowInlinePlay(true);
        webView = webViewGameObject.AddComponent<UniWebView>();
        webView.Load("https://demo.avaturn.dev/iframe");
        webView.ReferenceRectTransform = webViewFrame;

        _avatarReceiver.SetWebView(webView);
        webView.AddPermissionTrustDomain("scan.in3d.io");
        webView.SetUserAgent("Mozilla/5.0 (Linux; Android 12) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.5359.128 Mobile Safari/537.36");
        webView.OnPageFinished += WebViewOnOnPageFinished;
#endif
    }

    private void WebViewOnOnPageFinished(UniWebView webview, int statuscode, string url)
    {
        //webView.OnPageFinished -= WebViewOnOnPageFinished;


        string jsCode = @"
        if (typeof isListnerAttached !== 'undefined') {
            return;
        }

        isListnerAttached = true;
        var native_app  = window;
        window.addEventListener('message', subscribe);
        document.addEventListener('message', subscribe);
        alert('finished');

        function subscribe(event) {
            alert('sub started');
            const json = parse(event);
            if (json?.source !== 'avaturn' || json?.eventName == null) {
                alert('sub failed');
                return;
            }
            if (json.eventName === 'v2.avatar.exported') {
                
                let url = 'https://assets.hub.in3d.io/model_2022_12_22_T182855_939_9f0cea445f.glb';
                alert('sub done');
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
