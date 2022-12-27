
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


public class BackPackManager : UdonSharpBehaviour
{
    [HideInInspector][UdonSynced] public int combinedWeight = 0;
    [HideInInspector] public WorldManager worldManager;
    BackPackHolder backpackHolder;
    [HideInInspector][UdonSynced] public string interactingBackpackHolderName;
    [HideInInspector][UdonSynced] public string currentBackpackHolderName;
    [HideInInspector][UdonSynced] public int interactorID = 0;
    [HideInInspector][UdonSynced] public int backpackOwnerID = 0;
    [HideInInspector][UdonSynced] public bool isAttached = false;
    public ItemManager itemManager;
    void Start()
    {
        worldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
    }

    private void Update()
    {
        if (backpackHolder != null && isAttached)
        {
            transform.position = backpackHolder.transform.position;
            transform.rotation = backpackHolder.transform.rotation;
        }
        if (HasNewBackPackHolderInteraction())
        {
            OnNewBackPackHolderInteraction();
        }
    }

    bool HasNewBackPackHolderInteraction()
    {
        if (interactingBackpackHolderName != null && interactingBackpackHolderName != currentBackpackHolderName)
        {
            return true;
        }
        return false;
    }

    void OnNewBackPackHolderInteraction()
    {
        if (canAttachToHolder())
        {
            OnNewFollowHolder();
        }
        interactingBackpackHolderName = null;
        interactorID = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null)
        {
            BackPackHolder holder = other.transform.parent.gameObject.GetComponent<BackPackHolder>();
            if (holder != null && !isAttached && !PlayerHasBackPack(holder.user.playerId))
            {
                Networking.SetOwner(holder.user, gameObject);
                interactingBackpackHolderName = holder.gameObject.name;
                interactorID = holder.user.playerId;
            }
        }
        if (other.gameObject.GetComponent<BackPackItem>() != null && !isAttached)
        {
            itemManager.HandleItemTouched(other.gameObject);
        }
    }

    public void ChangeWeight(int amount)
    {
        combinedWeight += amount;
        if (!worldManager.isSpeedAdjustmentByWeight) return;
        if (backpackHolder == null) return;
        if (!isAttached) return;
        backpackHolder.user.SetWalkSpeed(backpackHolder.user.GetWalkSpeed() - (combinedWeight / 100));
        backpackHolder.user.SetRunSpeed(backpackHolder.user.GetRunSpeed() - (combinedWeight / 100));

    }

    public override void OnPickup()
    {
        if (isAttached)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            interactorID = Networking.LocalPlayer.playerId;
            if (CanRemoveBackPack())
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "RemoveBackPackFromHolder");
            }
        }

        base.OnPickup();
    }

    bool CanRemoveBackPack()
    {
        if (backpackOwnerID != 0 && backpackOwnerID == interactorID) return true;
        return true;
    }

    public void RemoveBackPackFromHolder()
    {
        if (worldManager.isSpeedAdjustmentByWeight)
        {
            backpackHolder.user.SetWalkSpeed();
            backpackHolder.user.SetRunSpeed();
        }
        currentBackpackHolderName = null;
        interactingBackpackHolderName = null;
        interactorID = 0;
        backpackHolder = null;
        isAttached = false;
        backpackOwnerID = 0;
        gameObject.name = "backpack";
    }
    bool canAttachToHolder()
    {
        if (!isAttached && !PlayerHasBackPack(interactorID)) return true;
        return false;
    }

    void OnNewFollowHolder()
    {
        GameObject obj = GameObject.Find(interactingBackpackHolderName);
        if (obj != null)
        {
            backpackHolder = obj.GetComponent<BackPackHolder>();
            if (backpackHolder != null)
            {
                backpackOwnerID = backpackHolder.user.playerId;
                gameObject.name = "backpack (" + backpackHolder.user.playerId + ")";
            }
        }
        else
        {
            backpackHolder = null;
        }
        ChangeWeight(0);
        currentBackpackHolderName = interactingBackpackHolderName;
        interactingBackpackHolderName = null;
        isAttached = true;
        interactorID = 0;
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        base.OnPlayerLeft(player);
        if (GetPlayerAttachedBackPack(player.playerId).name == gameObject.name)
        {
            RemoveBackPackFromHolder();
        }
        GameObject obj = GameObject.Find("BackPackHolder (" + player.playerId + ")");
        if (obj != null)
        {
            Destroy(obj);
        }
    }

    GameObject GetPlayerAttachedBackPack(int id)
    {
        GameObject obj = GameObject.Find("backpack (" + id + ")");
        if (obj != null)
        {
            return obj;
        }
        return null;
    }

    bool PlayerHasBackPack(int id)
    {
        if (GetPlayerAttachedBackPack(id) != null)
        {
            return true;
        }
        return false;
    }
}
