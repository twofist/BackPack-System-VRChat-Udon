
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WorldManager : UdonSharpBehaviour
{
    public GameObject backPackHolder;
    public bool isSpeedAdjustmentByWeight = false;
    void Start()
    {

    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        GameObject holder = Instantiate(backPackHolder);
        holder.name = holder.name + " (" + player.playerId + ")";
        holder.transform.position = player.GetPosition();
        BackPackHolder holderScript = holder.GetComponent<BackPackHolder>();
        holderScript.user = player;
    }
}


