using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AvatarReceiver : MonoBehaviour
{
    [Serializable] class OnReceived : UnityEvent<string> {}

    [SerializeField] private string parameterKey;
#if UNITY_IOS || UNITY_ANDROID
    [SerializeField] private UniWebView webView;
#endif
    [SerializeField] private UnityEvent startReceiving;
    [SerializeField] private OnReceived received;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL
        // Bridge.SetupVtoFrame();
#elif UNITY_IOS || UNITY_ANDROID
        webView.OnMessageReceived += GetFromWebView;
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
                startReceiving.Invoke();
                return;
            case "end":
                received?.Invoke("bytes");
                return;
        }

        byte[] glb = Convert.FromBase64String(url);
        GlbBytes.AddRange(glb);
    }

    public void GetAvatarLink(string url)
    {
        received?.Invoke(url);
    }
}
