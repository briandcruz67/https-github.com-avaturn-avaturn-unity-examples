﻿using System;
using UnityEngine;
using UnityEngine.Events;

public class AvatarReceiver : MonoBehaviour
{
    [Serializable] class OnUrlReceived : UnityEvent<string> {}

    [SerializeField] private string parameterKey;
    [SerializeField] private OnUrlReceived received;
    
    // Start is called before the first frame update
    void Start()
    {
        Bridge.SetupVtoFrame();
    }
    
    public void GetFromMainPageUrl()
    {
        var pageUrl = Application.absoluteURL;
       // Uri pageUri = new Uri(pageUrl);
       string[] split = pageUrl.Split('?');
       foreach (var part in split)
       {
           if (!part.StartsWith(parameterKey + "=")) continue;
           
           string param = part.Remove(0, parameterKey.Length + 1);
           if (!Uri.TryCreate(param, UriKind.Absolute, out var uriResult) ||
               (uriResult.Scheme != Uri.UriSchemeHttp && 
                uriResult.Scheme != Uri.UriSchemeHttps && 
                uriResult.Scheme != Uri.UriSchemeFile)) 
               continue;
           received?.Invoke(param);
           return;
       }
       Debug.Log("Can't find link in page url");
    }

    public void GetFromVtoFrame(string url)
    {
        received?.Invoke(url);
    }
}
