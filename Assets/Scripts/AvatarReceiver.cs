using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AvatarReceiver : MonoBehaviour
{
    [Serializable] class OnUrlReceived : UnityEvent<string> {}

    [SerializeField] private string parameterKey;
#if UNITY_IOS || UNITY_ANDROID
    [SerializeField] private UniWebView webView;
#endif
    [SerializeField] private OnUrlReceived received;
    
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL
        Bridge.SetupVtoFrame();
#elif UNITY_IOS || UNITY_ANDROID
        webView.OnMessageReceived += GetFromWebView;
        webView.SetSupportMultipleWindows(true, false);
        webView.OnFileDownloadStarted += (view, remoteUrl, fileName) => {
            Debug.Log(string.Format("Download Started. From '{0}', file name '{1}'", remoteUrl, fileName));
        };

        webView.OnFileDownloadFinished += (view, errorCode, remoteUrl, diskPath) => {
            if (errorCode == 0) { // Success
                Debug.Log(string.Format("Download Finished. From '{0}', to '{1}'", remoteUrl, diskPath));
                received?.Invoke(diskPath);
            } else {
                Debug.LogError("Download error: " + errorCode);
            }
        };
#endif
    }

    public static readonly List<byte> GlbBytes = new List<byte>();
    private void GetFromWebView(UniWebView webview, UniWebViewMessage message)
    {
        var split = message.RawMessage.Split(new string[] {$"{parameterKey}="}, System.StringSplitOptions.None);
        if (split.Length != 2)
        {
            Debug.Log($"Argument {parameterKey} is empty");
            return;
        }
        var url = split[1];
        switch (url)
        {
            case "begin":
                GlbBytes.Clear();
                return;
            case "end":
                received?.Invoke("bytes");
                return;
        }

        byte[] glb = Convert.FromBase64String(url);
        GlbBytes.AddRange(glb);
    }

    public void GetFromMainPageUrl()
    {
#if UNITY_WEBGL
        var pageUrl = Application.absoluteURL;
        string[] split = pageUrl.Split(new string[] {$"{parameterKey}="}, System.StringSplitOptions.None);
        if (split.Length != 2)
        {
            Debug.Log("Can't find link in page url");
            return;
        }

        var url = split[1];
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult) ||
            (uriResult.Scheme != Uri.UriSchemeHttp &&
             uriResult.Scheme != Uri.UriSchemeHttps &&
             uriResult.Scheme != Uri.UriSchemeFile))
        {
            Debug.Log("Failed link have wrong format");
            return;
        }
        received?.Invoke(url);
#endif
    }

    public void GetFromVtoFrame(string url)
    {
        received?.Invoke(url);
    }
}
