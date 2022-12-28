using UnityEngine;
using System.Runtime.InteropServices;

public class FrameController : MonoBehaviour
{   
    [SerializeField] string subdomain;

    private static bool is_setup = false;
    private static bool is_open = false;
    
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void SetupAvaturnIframeJS(string subdomain);
    
    [DllImport("__Internal")]
    private static extern void ShowAvaturnIframeJS();
    
    [DllImport("__Internal")]
    private static extern void HideAvaturnIFrameJS();
#endif


    public void ChangeVisibility() { 
        if (is_open) {
            Hide();
            
        } else { 
            Show();
            
        }
    }

    public void Show()
    {
        // Bridge.OpenIframe(subdomain);
#if !UNITY_EDITOR && UNITY_WEBGL
        SetupIframe(subdomain);
        ShowAvaturnIframeJS();
        is_open = true;
#else
        Debug.Log("Iframe can't be open in editor. Build the project for WebGL to open iframe.");
#endif
    }

    public void Hide()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        HideAvaturnIFrameJS();
        is_open = false;
#else
        Debug.Log("Iframe does not work in editor. Build the project for WebGL to open iframe.");
#endif
    }

    public static void SetupIframe(string subdomain)
    {   
        if(is_setup){
            return;
        }
       
#if !UNITY_EDITOR && UNITY_WEBGL
        SetupAvaturnIframeJS(subdomain);
#endif
        is_setup = true;
    }
}
