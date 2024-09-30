
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.AI;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class SafetyRespawn : UdonSharpBehaviour
{
    [Header("Tracks the player's last good position. Call ResetPlayerPosition to respawn them there.")]
    public LayerMask groundLayer;
    public float maxAngle = 45f;
    private Vector3 lastSafePosition;
    private Quaternion lastSafeRotation;
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
    }

    // Deliberately does not replace the base respawn function. 

    void Update()
    {
        if (!localPlayerCached) return;
        if (!Utilities.IsValid(localPlayer)) return;

        Vector3 localPlayerPosition = localPlayer.GetPosition();
        Quaternion localPlayerRotation = localPlayer.GetRotation();
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(localPlayerPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            if (angle <= maxAngle)
            {
                lastSafePosition = localPlayerPosition;
                lastSafeRotation = localPlayerRotation;
            }
        }
    }

    public void ResetPlayerPosition()
    {
		localPlayer.TeleportTo(lastSafePosition + new Vector3(0f, 0.01f, 0f), lastSafeRotation);
    }
}
