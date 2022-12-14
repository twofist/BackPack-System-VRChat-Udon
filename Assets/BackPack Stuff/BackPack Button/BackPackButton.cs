
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class BackPackButton : UdonSharpBehaviour
{
    [HideInInspector] public int weight = 0;
    [HideInInspector] public ItemManager manager;
    public TMPro.TextMeshProUGUI txt;
    [HideInInspector] public string customName;
    public Image image;
    [HideInInspector] public Sprite sprite;
    void Start()
    {
        if (customName != null)
        {
            txt.text = customName;
        }
        if (sprite != null)
        {
            image.sprite = sprite;
        }
    }

    public void OnButtonPress()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        manager.InitiateTakeOut(transform.GetSiblingIndex());
    }

}
