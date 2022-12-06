using System.Runtime.InteropServices;
using UnityEngine;

public static class Bridge
{
    private static bool is_setup = false;
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void SetupVto();
    
    [DllImport("__Internal")]
    private static extern void ShowVtoFrame();
    
    [DllImport("__Internal")]
    private static extern void HideVtoFrame();
#endif
    
    public static void SetIFrameVisibility(bool visibility)
    {
        Debug.Log("SetVisibility");
#if !UNITY_EDITOR && UNITY_WEBGL
        if (visibility)
        {   
            SetupVtoFrame();
            ShowVtoFrame();
            return;
        }

        HideVtoFrame();
#endif
    }

    public static void SetupVtoFrame()
    {   
        if(is_setup){
            return;
        }

        Debug.Log("Setup");
        
#if !UNITY_EDITOR && UNITY_WEBGL
        SetupVto();
#endif
        is_setup = true;
    }
}
