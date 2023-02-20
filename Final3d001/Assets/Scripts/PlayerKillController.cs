using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerKillController : NetworkBehaviour
{
    public int health = 100;
    int killCount = 0;
    private TimeShiftableObject shiftableWeapon;
    private NetworkVariable<bool> damageNetwork = new NetworkVariable<bool>();
    bool oldIsBeingHit = false;

    void Start()
    {
        if (IsClient && IsOwner)
        {
            // shiftableWeapon = GameObject.Find("Weapon").GetComponent<TimeShiftableObject>();
        }
    }

    void Update()
    {
        if (IsOwner)
        {

            // if (Input.GetKeyDown(KeyCode.Alpha1))
            // {
            //     shiftableWeapon.TimeShift();
            // }

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
                        print($"collide id? {clientId}");
                        OnHitPlayerServerRpc(clientId);
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
    void OnHitPlayerServerRpc(ulong clientId)
    {
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            print($"Hit player {client.PlayerObject.name} {client.PlayerObject}");
            client.PlayerObject.GetComponent<PlayerKillController>().OnHit();
        }
    }
    
    public void OnHit()
    {
        if (!IsOwner)
        {
            return;
        }
        
        health -= 10;
        print("I was hit!" + health);
            
        if (health <= 0)
        {
            health = 100;
            GetComponent<PlayerMovement>().Respawn();
        }
    }

    void OnKill()
    {
        killCount++;
        GameManager.GlobalKillCount++;
    }
}
