
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BackPackLid : UdonSharpBehaviour
{
    public GameObject canvas;
    bool isOpen = false;
    public Animator animator;
    void Start()
    {

    }

    public override void Interact()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "LidInteraction");
        canvas.SetActive(isOpen);
    }

    public void LidInteraction()
    {
        isOpen = !isOpen;
        if (!isOpen)
        {
            InteractionText = "open";

        }
        else
        {
            InteractionText = "close";
        }
        animator.SetBool("isOpen", isOpen);
    }
}
