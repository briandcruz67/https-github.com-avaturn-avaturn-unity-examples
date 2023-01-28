using System.Threading.Tasks;
using UnityEngine;
using BKUnity;

[RequireComponent(typeof(Animator))]
public class PrepareAvatar : MonoBehaviour
{
    [SerializeField] private DownloadAvatarEvents events;
    [Tooltip("Clear root gameObject out of prev avatar gameObjects. Start from that child index")]
    [SerializeField] private int _clearRootFromIndex;
    [SerializeField] private bool _prepareOnStart;

    private Animator _animator;
    
    private void Start()
    {
        events.OnSuccess += PrepareModel;
        _animator = GetComponent<Animator>();

        if (_prepareOnStart)
        {
            _animator.avatar = HumanoidAvatarBuilder.Build(gameObject);
        }
    }


    private void PrepareRootModel()
    {
        PrepareModel(transform);
    }

    private async void PrepareModel(Transform downloadedModel)
    {
        //delete prev model and skeleton
        for (int i = _clearRootFromIndex; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        await Task.Yield();
        
        //go trough hierarchy and move model from downloaded avatar to base
        var root = downloadedModel.transform.GetChild(0);
        
        if (!root)
        {
            Debug.LogWarning("Prepare failed. Can't find root object");
            return;
        }
        if (root.childCount != 1)
        {
            Debug.LogWarning("Prepare failed. Wrong number of children in root object");
            return;
        }
        var armatureRoot = root.transform.GetChild(0);
        if (!armatureRoot)
        {
            Debug.LogWarning("Prepare failed. Can't find group object");
            return;
        }

        var valid = HasValidBoneNames(armatureRoot, out var hips);
        if (valid == null)
        {
            Debug.LogWarning("Prepare failed. Can't find Hips");
            return;
        }
        if (valid == false) RenameBones(hips);
        
        //_animator.avatar = HumanoidAvatarBuilder.Build(armatureRoot.gameObject);

        var childCount = armatureRoot.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var child = armatureRoot.GetChild(0);
            child.SetParent(transform);
            //child.localEulerAngles
        }
        
        await Task.Yield();
        Destroy(root.gameObject);
        
        _animator.avatar = HumanoidAvatarBuilder.Build(gameObject);
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