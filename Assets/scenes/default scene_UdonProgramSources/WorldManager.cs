
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WorldManager : UdonSharpBehaviour
{
    public GameObject backPackHolder;
    public bool isSpeedAdjustmentByWeight = true;
    int counter = 0;
    void Start()
    {

    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        GameObject holder = Instantiate(backPackHolder);
        holder.name = holder.name + " (" + counter + ")";
        counter++;
        holder.transform.position = player.GetPosition();
        BackPackHolder holderScript = holder.GetComponent<BackPackHolder>();
        holderScript.user = player;
    }
}


