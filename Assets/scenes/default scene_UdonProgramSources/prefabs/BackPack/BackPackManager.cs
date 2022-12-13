
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;


public class BackPackManager : UdonSharpBehaviour
{
    [UdonSynced] public int combinedWeight = 0;
    public Transform itemHolder;
    public Transform UIContent;
    public GameObject UIButtonPrefab;
    public Transform itemSpawn;
    [UdonSynced] public string itemName;
    [UdonSynced] public int itemIndex;
    [UdonSynced] public int buttonIndex;
    public WorldManager worldManager;
    BackPackHolder followHolder;
    [UdonSynced] public int holderID = 0;
    [UdonSynced] public string holderName;
    [UdonSynced] public int interactorID = 0;
    void Start()
    {
        worldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
    }

    private void Update()
    {
        if (followHolder != null)
        {
            transform.position = followHolder.transform.position;
            transform.rotation = followHolder.transform.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null)
        {
            BackPackHolder holder = other.transform.parent.gameObject.GetComponent<BackPackHolder>();
            if (holder != null && CanAttachBackPack())
            {
                Networking.SetOwner(Networking.LocalPlayer, gameObject);
                holderName = holder.gameObject.name;
                interactorID = Networking.LocalPlayer.playerId;
                // RequestSerialization();
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "AttachBackPackToHolder");
            }
        }
        if (other.gameObject.GetComponent<BackPackItem>())
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            itemName = other.gameObject.name;
            RequestSerialization();
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "AddItemToBackPack");
        }
    }

    public void AddItemToBackPack()
    {
        GameObject item = GameObject.Find(itemName);
        if (item == null) return;
        BackPackItem backPackItem = item.gameObject.GetComponent<BackPackItem>();
        if (backPackItem == null) return;
        item.transform.SetParent(itemHolder);
        item.transform.position = new Vector3(0, 0, 0);
        item.gameObject.SetActive(false);
        GameObject button = Instantiate(UIButtonPrefab, UIContent);
        BackPackButton bpButton = button.GetComponent<BackPackButton>();
        bpButton.index = itemHolder.childCount - 1;
        bpButton.customName = backPackItem.customName;
        bpButton.sprite = backPackItem.sprite;
        bpButton.manager = this;
        ChangeWeight(backPackItem.weight);
        // itemName = "";
    }
    public override void OnDeserialization()
    {
        base.OnDeserialization();
        AddItemToBackPack();
        // AttachBackPackToHolder();
        // RemoveBackPackFromHolder();
    }

    public void InitiateTakeOut(int index, int siblingIndex)
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        itemIndex = index;
        buttonIndex = siblingIndex;
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "TakeItemOutOfBackPack");
    }

    public void TakeItemOutOfBackPack()
    {
        Transform item = itemHolder.GetChild(itemIndex);
        if (item == null) return;
        BackPackItem backPackItem = item.gameObject.GetComponent<BackPackItem>();
        if (backPackItem == null) return;
        item.position = item.TransformPoint(itemSpawn.position);
        item.SetParent(null);
        item.gameObject.SetActive(true);
        ChangeWeight(-backPackItem.weight);
        Destroy(UIContent.GetChild(buttonIndex).gameObject);
    }

    void ChangeWeight(int amount)
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
        if (CanRemoveBackPack())
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            interactorID = Networking.LocalPlayer.playerId;
            // RequestSerialization();
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "RemoveBackPackFromHolder");
        }
        base.OnPickup();
    }

    public void AttachBackPackToHolder()
    {
        if (!CanAttachBackPack()) return;
        GameObject playerHolder = GameObject.Find(holderName);
        Debug.Log(playerHolder + " - attach backpack");
        if (playerHolder == null) return;
        BackPackHolder newHolder = playerHolder.GetComponent<BackPackHolder>();
        if (newHolder == null) return;
        Debug.Log(followHolder + " - " + holderID + " - attach big");
        followHolder = newHolder;
        // followHolder.hasBackPack = true;
        // newHolder.backPack = gameObject;
        holderID = newHolder.user.playerId;
        ChangeWeight(0);
        // holderName = "";
        // interactorID = 0;
    }

    bool CanAttachBackPack()
    {
        if (followHolder != null) return false;
        if (holderID != 0) return false;
        Debug.Log("attaching");
        return true;
    }

    bool CanRemoveBackPack()
    {
        if (followHolder == null) return false;
        // if (!followHolder.hasBackPack) return false;
        if (holderID == 0) return false;
        if (holderID != interactorID) return false;
        Debug.Log("removing");
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
        // followHolder.hasBackPack = false;
        // followHolder.backPack = null;
        followHolder = null;
        holderID = 0;
        interactorID = 0;
    }
}
