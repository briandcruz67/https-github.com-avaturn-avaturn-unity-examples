using System;
using UnityEngine;

[CreateAssetMenu(menuName = "in3DEvents/DownloadEvents")]
public class DownloadAvatarEvents : ScriptableObject
{
   public Action<Transform> OnSuccess;
}
