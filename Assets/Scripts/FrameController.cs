using UnityEngine;

public class FrameController : MonoBehaviour
{
    public void Show()
    {
        Bridge.SetIFrameVisibility(true);
    }

    public void Hide()
    {
        Bridge.SetIFrameVisibility(false);
    }
}
