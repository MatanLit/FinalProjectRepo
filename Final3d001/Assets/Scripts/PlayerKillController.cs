using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerKillController : NetworkBehaviour
{
    int killCount = 0;
    public int health = 100;
    [SerializeField] TimeShiftableObject shiftableWeapon;
    bool oldIsBeingHit = false;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        gameObject.name = $"Player-{NetworkObject.OwnerClientId}";
        
        if (!IsOwner)
        {
            Destroy(this);
        }
    }

    void Update()
    {
        if (IsOwner)
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                shiftableWeapon.TimeShift();
            }

            // TODO: Should be in the weapon script since raycast props should change based on weapon
            if (Input.GetButtonDown("Fire1"))
            {
                if (!Physics.Raycast(transform.position, transform.forward, out var hit, 100))
                {
                    return;
                }

                // if (hit.collider.gameObject.CompareTag("Target"))
                // {
                //     Destroy(hit.collider.gameObject);
                //     shiftableWeapon.TimeShift();
                //     OnKill();
                // }


                
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    PlayerKillController hitPlayer = hit.collider.gameObject.GetComponent<PlayerKillController>();
                    try
                    {
                        ulong clientId = hit.collider.GetComponent<NetworkObject>().OwnerClientId;
                        OnHitServerRpc(clientId);
                    } catch (Exception e)
                    {
                        print($"collide id error {e}");
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print($"Trigger enter {other.gameObject.name}");
    }

    [ServerRpc]
    void OnHitServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
    {
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            ulong senderClientId = serverRpcParams.Receive.SenderClientId; 
            NetworkClient targetClient = NetworkManager.ConnectedClients[clientId];
            
            print($"Hitting player {targetClient.PlayerObject.name} {targetClient.PlayerObject}");
            PlayerKillController playerTarget = targetClient.PlayerObject.GetComponent<PlayerKillController>();
            playerTarget.OnHitPlayerClientRpc(senderClientId);
            // announce all players
            TestClientRpc(senderClientId);
            
            // if (playerTarget.health - 10 == 0)
            // {
            //     OnKillSuccess(senderClientId);    
            // }
        }
    }

    [ClientRpc]
    void TestClientRpc(ulong killerId)
    {
        GameObject killerPlayer = GameObject.Find($"Player-{killerId}");
        print($"Searching player {killerId} -- found? {killerPlayer}");
        killerPlayer.GetComponent<PlayerKillController>().shiftableWeapon.TimeShift();
    }
    
    [ClientRpc]
    void OnHitPlayerClientRpc(ulong killerId)
    {
        health -= 10;
        print($"I was hit! Health: {health}, id: {NetworkObject.OwnerClientId}");
            
        if (health <= 0)
        {
            health = 100;
            print($"{NetworkObject.OwnerClientId} was killed by {killerId}");
            GetComponent<PlayerMovement>().Respawn();
        }
    }

    void OnKillSuccess()
    {
        killCount++;
        GameManager.GlobalKillCount++;

        if (killCount % 1 == 0)
        {
            shiftableWeapon.TimeShift();
            // ShiftWeaponServerRpc(killerId);
        }
    }

    [ServerRpc]
    void ShiftWeaponServerRpc(ulong killerId, ServerRpcParams serverRpcParams = default)
    {
        // ulong clientId = serverRpcParams.Receive.SenderClientId;
        if (NetworkManager.ConnectedClients.ContainsKey(killerId))
        {
            var client = NetworkManager.ConnectedClients[killerId];
            print($"server time shift for client id {killerId}");
            client.PlayerObject.GetComponent<PlayerKillController>().ShiftWeaponClientRpc();
        }
    }
    
    [ClientRpc]
    void ShiftWeaponClientRpc()
    {
        shiftableWeapon.TimeShift();
    }
}
