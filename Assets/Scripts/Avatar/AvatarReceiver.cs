using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

public class AvatarReceiver : MonoBehaviour
{
    [Serializable] class OnReceived : UnityEvent<string> {}

    [SerializeField] private UnityEvent startReceiving;
    [SerializeField] private OnReceived received;

    private UniWebView webView;

    // Start is called before the first frame update
    public void SetWebView(UniWebView webView)
    {
        this.webView = webView;
        webView.OnMessageReceived += GetFromWebView;
    }

    private void GetFromWebView(UniWebView webview, UniWebViewMessage message)
    {   
        string parameterKey = "avatar_link";
        var split = message.RawMessage.Split(new string[] {$"{parameterKey}="}, System.StringSplitOptions.None);
        if (split.Length != 2)
        {
            Debug.Log($"Argument {parameterKey} is empty");
            return;
        }
        var url = split[1];
        url =  System.Uri.UnescapeDataString(url);

        if (url.StartsWith("error://")) {
            Debug.Log($"Error when receiving data from Avaturn: {url.Substring(8)}");
            return;
        }

        received?.Invoke(url);
    }

    public void GetAvatarLink(string url)
    {
        received?.Invoke(url);
    }
}
