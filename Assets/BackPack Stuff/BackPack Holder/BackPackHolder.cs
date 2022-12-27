
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BackPackHolder : UdonSharpBehaviour
{
    // [HideInInspector][UdonSynced] public bool hasBackPack = false;
    [HideInInspector] public VRCPlayerApi user;
    [HideInInspector] public WorldManager worldManager;

    void Start()
    {
        worldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
    }

    private void Update()
    {
        if (user != null)
        {
            Vector3 pivot = user.GetBonePosition(HumanBodyBones.Spine);
            Vector3 point = pivot + Vector3.forward * -.3f;
            Quaternion rotation = user.GetBoneRotation(HumanBodyBones.Spine);
            Vector3 dir = rotation * (point - pivot);
            transform.position = dir + pivot;
            transform.rotation = rotation;
        }
    }
}
