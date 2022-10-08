using UnityEngine;

public class WebViewController : MonoBehaviour
{
    [SerializeField] private UniWebView webView;
    [SerializeField] [TextArea(25, 40)] private string jsCode;
    
    // Start is called before the first frame update
    void Start()
    {
        webView.OnPageFinished += WebViewOnOnPageFinished;
    }

    private void WebViewOnOnPageFinished(UniWebView webview, int statuscode, string url)
    {
        webView.OnPageFinished -= WebViewOnOnPageFinished;
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
