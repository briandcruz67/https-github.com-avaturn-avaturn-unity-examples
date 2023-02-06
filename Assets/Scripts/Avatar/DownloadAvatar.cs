using System;
using System.Threading.Tasks;
using UnityEngine;

using GLTFast;
using GLTFast.Loading;
using UnityEngine.Scripting;

/// <summary>
/// This example of loading avatar model.
/// </summary>
[RequireComponent(typeof(GltfAsset))]
public class DownloadAvatar : MonoBehaviour
{
    [Serializable] public class BlobFiles
    {
        [Serializable] public class FileUrl
        {
            [Preserve] public string url;
            [Preserve] public string fileName;
        }
        [Preserve] public FileUrl[] fileUrls;
    }

    [SerializeField] private DownloadAvatarEvents _events;
    [SerializeField] private bool _downloadOnStart;
    [SerializeField] private string _startUrl;

    private void Start()
    {
        if(_downloadOnStart)
            Download(_startUrl);
    }

    public async void Download(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("Fail to download: url is empty");
            return;
        }
        Debug.Log("Start download...");
        
        // Loading via GltFast loader
        var asset = GetComponent<GltfAsset>();
        asset.ClearScenes();
        var success = await asset.Load(url);
        // Optional for animations
        if (success)
        {
            _events.OnSuccess?.Invoke(transform);
        }
        else
        {
            Debug.LogError($"Fail to download");
        }
    }
    
    public async  Task<IDownload> Request(Uri url) {
        var req = new AwaitableDownload(url);
        while (req.MoveNext()) {
            await Task.Yield();
        }
        return req;
    }
}