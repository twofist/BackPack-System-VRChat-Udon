
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


public class BackPackManager : UdonSharpBehaviour
{
    [HideInInspector][UdonSynced] public int combinedWeight = 0;
    [HideInInspector] public WorldManager worldManager;
    BackPackHolder followHolder;
    [HideInInspector][UdonSynced] public string followHolderName;
    [HideInInspector][UdonSynced] public string previousFollowHolderName;
    [HideInInspector][UdonSynced] public int interactorID = 0;
    public ItemManager itemManager;
    void Start()
    {
        worldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
    }

    private void Update()
    {
        if (followHolder != null && followHolder.hasBackPack)
        {
            transform.position = followHolder.transform.position;
            transform.rotation = followHolder.transform.rotation;
        }
        if (previousFollowHolderName != followHolderName)
        {
            OnNewFollowHolder();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null)
        {
            BackPackHolder holder = other.transform.parent.gameObject.GetComponent<BackPackHolder>();
            if (holder != null && !holder.hasBackPack)
            {
                Networking.SetOwner(holder.user, gameObject);
                followHolderName = holder.gameObject.name;
                interactorID = holder.user.playerId;
                // RequestSerialization();
                // SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "AttachBackPackToHolder");
            }
        }
        if (other.gameObject.GetComponent<BackPackItem>())
        {
            itemManager.HandleItemTouched(other.gameObject);
        }
    }

    public void ChangeWeight(int amount)
    {
        combinedWeight += amount;
        if (!worldManager.isSpeedAdjustmentByWeight) return;
        if (followHolder == null) return;
        if (followHolder.user == null) return;
        followHolder.user.SetWalkSpeed(followHolder.user.GetWalkSpeed() - (combinedWeight / 100));
        followHolder.user.SetRunSpeed(followHolder.user.GetRunSpeed() - (combinedWeight / 100));
    }

    public override void OnPickup()
    {
        if (followHolder != null && followHolder.hasBackPack)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            interactorID = Networking.LocalPlayer.playerId;
            // RequestSerialization();
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "RemoveBackPackFromHolder");
        }

        base.OnPickup();
    }

    bool CanRemoveBackPack()
    {
        if (followHolder == null) return false;
        if (followHolder.user.playerId != interactorID) return false;
        return true;
    }

    public void RemoveBackPackFromHolder()
    {
        if (!CanRemoveBackPack()) return;
        if (worldManager.isSpeedAdjustmentByWeight)
        {
            if (followHolder != null && followHolder.user != null)
            {
                followHolder.user.SetWalkSpeed();
                followHolder.user.SetRunSpeed();
            }
        }
        if (followHolder != null)
        {
            followHolder.hasBackPack = false;
        }
        followHolderName = null;
    }

    void OnNewFollowHolder()
    {
        GameObject obj = GameObject.Find(followHolderName);
        if (obj != null)
        {
            followHolder = obj.GetComponent<BackPackHolder>();
            if (followHolder != null)
            {
                followHolder.hasBackPack = true;
            }
        }
        else
        {
            followHolder = null;
        }
        ChangeWeight(0);
        previousFollowHolderName = followHolderName;
    }
}
