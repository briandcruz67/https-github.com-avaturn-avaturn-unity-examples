using System.Runtime.InteropServices;

public static class Bridge
{
    [DllImport("__Internal")]
    private static extern void SetupVto();
    
    [DllImport("__Internal")]
    private static extern void ShowVtoFrame();
    
    [DllImport("__Internal")]
    private static extern void HideVtoFrame();

    public static void SetIFrameVisibility(bool isVisible)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        if (isVisible)
        {
            ShowVtoFrame();
            return;
        }

        HideVtoFrame();
#endif
    }

    public static void SetupVtoFrame()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        SetupVto();
#endif
    }
}
