using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerKillController : NetworkBehaviour
{
    NetworkVariable<bool> shouldUpgradeNetworkVariable = new NetworkVariable<bool>();
    bool oldShouldUpgrade = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (oldShouldUpgrade != shouldUpgradeNetworkVariable.Value)
        {
            GameManager.ShiftTimeForAllUpgradeableObjects();
            UpgradeAllClientsServerRpc();
        }

        if (IsClient && IsOwner)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                GameManager.ShiftTimeForAllUpgradeableObjects();
            }
        }
    }

    [ServerRpc]
    void UpgradeAllClientsServerRpc()
    {
        shouldUpgradeNetworkVariable.Value = !shouldUpgradeNetworkVariable.Value;
    }
}
