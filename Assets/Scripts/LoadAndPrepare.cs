using System;
using System.Threading.Tasks;
using UnityEngine;

using BKUnity;
using GLTFast;
using GLTFast.Loading;
using UnityEngine.Scripting;

/// <summary>
/// This example of loading avatar model.
/// </summary>
[RequireComponent(typeof(GltfAsset))]
public class LoadAndPrepare : MonoBehaviour
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
    
    [SerializeField] private RuntimeAnimatorController controller;
    [SerializeField] private bool _prepareOnStart;

    private GameObject _lastModel;

    private void Start()
    {
        if (_prepareOnStart)
            PrepareModel(transform);
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
            if (!PrepareModel(this.transform)) 
                Debug.LogError($"Fail to prepare model");
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

    public void PrepareModelTest()
    {
        PrepareModel(transform);
    }

    private bool PrepareModel(Transform model)
    {
        if(_lastModel != null)
            Destroy(_lastModel);
        if (model.transform.childCount != 1)
        {
            Debug.LogWarning("Wrong number of children");
            return false;
        }
        var root = model.transform.GetChild(0);
        _lastModel = root.gameObject;
        if (!root)
        {
            Debug.LogWarning("Can't find root object");
            return false;
        }
        if (root.childCount != 1)
        {
            Debug.LogWarning("Wrong number of children in root object");
            return false;
        }
        var armatureRoot = root.transform.GetChild(0);
        if (!armatureRoot)
        {
            Debug.LogWarning("Can't find group object");
            return false;
        }

        var valid = HasValidBoneNames(armatureRoot, out var hips);
        if (valid == null)
        {
            Debug.LogWarning("Can't find Hips");
            return false;
        }
        if (valid == false) RenameBones(hips);
        
        Destroy(armatureRoot.GetComponent<Animation>());
        var animator = armatureRoot.gameObject.GetComponent<Animator>();
        if (!animator) animator = armatureRoot.gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = controller;
        animator.applyRootMotion = true;

        animator.avatar = HumanoidAvatarBuilder.Build(armatureRoot.gameObject);

        return true;
    }

    private bool? HasValidBoneNames(Transform root, out Transform hips)
    {
        for (int i = 0; i < root.childCount; i++)
        {
            hips = root.GetChild(i);
            var boneName = hips.name;
            if (boneName == "Hips") {
                return true;
            }
        }
        hips = null;
        return null;
    }

    private void RenameBones(Transform hips)
    {
        hips.name = "mixamorig:Hips";
        foreach (Transform bone in hips.GetComponentsInChildren<Transform>())
        {
            foreach (var validBoneName in AvatarUtils.HumanSkeletonNames.Keys)
            {
                if (validBoneName.Replace(":", "") != bone.name) continue;
                bone.name = validBoneName;
                break;
            }
        }
    }
}