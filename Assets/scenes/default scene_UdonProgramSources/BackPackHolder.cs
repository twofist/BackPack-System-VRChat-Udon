
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BackPackHolder : UdonSharpBehaviour
{
    //public GameObject backPack;

    //[UdonSynced] public string backPackName;
    public VRCPlayerApi user;
    public WorldManager worldManager;

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
        /*
        if (backPack != null)
        {
            backPack.transform.position = transform.position;
            backPack.transform.rotation = transform.rotation;
        }*/
    }
    /*
        public void RemoveBackPack()
        {
            if (backPack != null)
            {
                backPack.GetComponent<BackPackManager>().followHolder = null;
            }
            backPack = null;
            backPackName = "";
            if (!worldManager.isSpeedAdjustmentByWeight) return;
            user.SetWalkSpeed();
            user.SetRunSpeed();
        }
        public void AddBackPack()
        {
            Debug.Log("addbackpack - " + backPackName);
            backPack = GameObject.Find(backPackName);
            if (backPack != null)
            {
                backPack.GetComponent<BackPackManager>().ChangeWeight(0);
            }
        }

        public bool AllowRemovalOfBackPack()
        {
            if (backPack == null) return false;
            return true;
        }
        public bool AllowAddingBackPack()
        {
            if (backPack != null) return false;
            return true;
        }

        public override void OnDeserialization()
        {
            base.OnDeserialization();
            AddBackPack();
        }

        public void Add(string objectName)
        {
            Debug.Log("addingbackpack");
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            backPackName = objectName;
            RequestSerialization();
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "AddBackPack");
        }

        /*
            private void OnTriggerEnter(Collider other)
            {
                Debug.Log(other.gameObject.transform.parent + " - holder collider enter");
                BackPackManager manager = other.GetComponent<BackPackManager>();
                if (manager == null) return;
                if (backPack == manager.gameObject) return;
                if (backPack != null) return;
                backPack = manager.gameObject;
                manager.transform.SetParent(transform);
                manager.transform.position = new Vector3(0, 0, 0);
                manager.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                // cldr.enabled = false;
            }
        */
    /*
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.transform.parent + " - holder collider exit");
        BackPackManager manager = other.GetComponent<BackPackManager>();
        if (manager == null) return;
        if (manager.gameObject != backPack) return;
        backPack.transform.SetParent(null);
        backPack = null;
        // cldr.enabled = true;
    }
    */
}
