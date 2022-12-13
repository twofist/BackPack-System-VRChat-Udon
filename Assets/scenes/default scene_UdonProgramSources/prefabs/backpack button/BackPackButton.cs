
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class BackPackButton : UdonSharpBehaviour
{
    [HideInInspector] public int index = 0;
    [HideInInspector] public int weight = 0;
    [HideInInspector] public ItemManager manager;
    public TMPro.TextMeshProUGUI txt;
    [HideInInspector] public string customName;
    public Image image;
    [HideInInspector] public Sprite sprite;
    void Start()
    {

    }

    public void OnButtonPress()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        txt.text = customName;
        if (sprite != null)
        {
            image.sprite = sprite;
        }
        manager.InitiateTakeOut(index, transform.GetSiblingIndex());
    }

}
