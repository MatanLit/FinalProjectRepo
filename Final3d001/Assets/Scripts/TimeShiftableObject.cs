using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// TODO: GameManager should inherit from NetworkBehaviour and transfer data to all objects
public class TimeShiftableObject : MonoBehaviour
{
    [SerializeField] int timeShiftLevel = 0;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == timeShiftLevel);
        }
    }

    public void TimeShift()
    {
        if (timeShiftLevel + 1 >= transform.childCount)
        {
            timeShiftLevel = 0;
        }
        else
        {
            timeShiftLevel++;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            bool isActive = i == timeShiftLevel;
            transform.GetChild(i).gameObject.SetActive(isActive);
        }
        
        // OnTimeShiftServerRpc(timeShiftLevel);
    }
    
    // [ServerRpc (RequireOwnership = false)]
    // void OnTimeShiftServerRpc(int serverTimeShiftLevel, ServerRpcParams serverRpcParams = default)
    // {
    //     var clientId = serverRpcParams.Receive.SenderClientId;
    //     if (!NetworkManager.ConnectedClients.ContainsKey(clientId))
    //     {
    //         return;
    //     }
    //     
    //     
    //     var client = NetworkManager.ConnectedClients[clientId];
    //     TimeShiftableObject shiftableWeapon = client.PlayerObject.GetComponentInChildren<TimeShiftableObject>();
    //     print($"server time shift {shiftableWeapon} for client id {clientId}");
    //     shiftableWeapon.TimeShiftClientRpc(serverTimeShiftLevel);
    // }
    //
    // [ClientRpc]
    // void TimeShiftClientRpc(int serverTimeShiftLevel)
    // {
    //     print($"client time shift {serverTimeShiftLevel} for client id {NetworkManager.LocalClientId}");
    //     for (int i = 0; i < transform.childCount; i++)
    //     {
    //         transform.GetChild(i).gameObject.SetActive(i == serverTimeShiftLevel);
    //     }
    // }
}
