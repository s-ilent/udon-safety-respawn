
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class SafetyRespawner : UdonSharpBehaviour
{
    [Header("Respawns the player on contact.")]
    [Tooltip("Set the main Safety Respawner object here.")]
    public SafetyRespawn safetyRespawn;
    
    // VRC stuff
    private VRCPlayerApi localPlayer;
    private bool localPlayerCached = false;

    public void Start()
    {
        localPlayer = Networking.LocalPlayer;
        if (localPlayer != null)
        {
            localPlayerCached = true;
        }
        // If no coordinator script, die
        if (safetyRespawn == null) this.enabled = false;
    }

    // Used if the collider is a trigger.
    public override void OnPlayerTriggerEnter(VRC.SDKBase.VRCPlayerApi player)
    {
        applyRespawn(player);
    }
    // Used if the collider is not a trigger.
    // As far as I can tell, players don't really "enter" solid colliders. 
    public override void OnPlayerCollisionEnter(VRC.SDKBase.VRCPlayerApi player)
    {
        applyRespawn(player);
    }
    // No reason to handle OnCollisionEnter for non-players yet
    private void applyRespawn(VRC.SDKBase.VRCPlayerApi player)
    {
        if (!(localPlayerCached && Utilities.IsValid(localPlayer))) return;
        if (player != localPlayer) return;
        safetyRespawn.ResetPlayerPosition();
    }
}
