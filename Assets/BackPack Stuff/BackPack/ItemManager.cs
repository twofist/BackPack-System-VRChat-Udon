
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ItemManager : UdonSharpBehaviour
{
    public Transform itemHolder;
    public Transform UIContent;
    public GameObject UIButtonPrefab;
    public Transform itemSpawn;
    [HideInInspector][UdonSynced] public string itemName;
    [HideInInspector][UdonSynced] public int itemIndex;
    [HideInInspector][UdonSynced] public int buttonIndex;
    [HideInInspector] public WorldManager worldManager;
    public BackPackManager backPackManager;

    public AudioClip audioAdd;
    public AudioClip audioRemove;
    public AudioSource audioSource;
    void Start()
    {

    }

    public void HandleItemTouched(GameObject obj)
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        itemName = obj.name;
        RequestSerialization();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "AddItemToBackPack");
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
        backPackManager.ChangeWeight(backPackItem.weight);
        audioSource.clip = audioAdd;
        audioSource.Play();
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
        backPackManager.ChangeWeight(-backPackItem.weight);
        Destroy(UIContent.GetChild(buttonIndex).gameObject);
        audioSource.clip = audioRemove;
        audioSource.Play();
    }

    public override void OnDeserialization()
    {
        base.OnDeserialization();
        AddItemToBackPack();
    }
}
