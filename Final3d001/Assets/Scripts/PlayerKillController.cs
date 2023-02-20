using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerKillController : NetworkBehaviour
{
    int killCount = 0;
    bool oldIsBeingHit = false;
    [SerializeField] TimeShiftableObject shiftableWeapon;
    private NetworkVariable<PlayerWeaponData> weaponState = new NetworkVariable<PlayerWeaponData>();
    private NetworkVariable<Vector3> playerPosition = new NetworkVariable<Vector3>();
    public NetworkVariable<int> health = new NetworkVariable<int>(100);

    private void Awake()
    {
        playerPosition.OnValueChanged += OnPlayerPositionChanged;
        weaponState.OnValueChanged += OnWeaponStateChanged;
        health.OnValueChanged += OnHealthChanged;
    }

    void Update()
    {
        // TODO: Should be in the weapon script since raycast props should change based on weapon
        if (Input.GetButtonDown("Fire1"))
        {
            if (!Physics.Raycast(transform.position, transform.forward, out var hit, 100))
            {
                return;
            }


            if (hit.collider.gameObject.CompareTag("Player") && IsOwner)
            {
                PlayerKillController hitPlayerController = hit.collider.gameObject.GetComponent<PlayerKillController>();
                hitPlayerController.TakeAHitServerRpc();
                print($"hit player {hitPlayerController.health.Value}");
                
                if (hitPlayerController.health.Value - 10 == 0)
                {
                    transmitWeaponIndexServerRpc();
                    shiftableWeapon.TimeShift();
                }
            }
        }
    }
    
    private void OnHealthChanged(int oldHealth, int newHealth)
    {
        if (IsOwner)
        {
            print($"Health: {newHealth} ");
            if (newHealth == 0)
            {
                health.Value = 100;
                Respawn();
            }
        }
    }
    
    private void OnPlayerPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        transform.position = newPosition;
    }
    
    private void OnWeaponStateChanged(PlayerWeaponData oldWeaponData, PlayerWeaponData newWeaponData)
    {
        print($"weapon state changed {oldWeaponData.playerId} {newWeaponData.playerId} {NetworkManager.LocalClientId}");
        
        try
        {
            if (IsServer)
            {
                NetworkManager.ConnectedClients[newWeaponData.playerId].PlayerObject.GetComponent<PlayerKillController>().shiftableWeapon.TimeShift();
            }
        }
        catch (Exception e)
        {
            print($"error {e}");
        }
    }

    [ServerRpc (RequireOwnership = false)]
    void TakeAHitServerRpc()
    {
        health.Value -= 10;
        if (health.Value <= 0)
        {
            health.Value = 100;
            Respawn();
        }
    }

    void Respawn()
    {
        print("Respawning");
        transmitPlayerPositionServerRpc(PlayerMovement.spawnPosition);
        transform.position = PlayerMovement.spawnPosition;
    }
    
    [ServerRpc (RequireOwnership = false)]
    void transmitPlayerPositionServerRpc(Vector3 position)
    {
        playerPosition.Value = position;
    }
    
    [ServerRpc (RequireOwnership = false)]
    void transmitWeaponIndexServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong senderClientId = serverRpcParams.Receive.SenderClientId;
        if (!NetworkManager.ConnectedClients.ContainsKey(senderClientId))
        {
            return;
        }
        
        weaponState.Value = new PlayerWeaponData()
        {
            weaponIndex = weaponState.Value.weaponIndex + 1,
            playerId = senderClientId
        };
    }
    
    struct PlayerWeaponData: INetworkSerializable
    {
        public int weaponIndex;
        public ulong playerId;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T: IReaderWriter
        {
            serializer.SerializeValue(ref weaponIndex);
            serializer.SerializeValue(ref playerId);
        }
    }

    // [ServerRpc]
    // void OnHitServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
    // {
    //     if (NetworkManager.ConnectedClients.ContainsKey(clientId))
    //     {
    //         ulong senderClientId = serverRpcParams.Receive.SenderClientId;
    //         NetworkClient targetClient = NetworkManager.ConnectedClients[clientId];
    //
    //         print($"Hitting player {targetClient.PlayerObject.name} {targetClient.PlayerObject}");
    //         PlayerKillController playerTarget = targetClient.PlayerObject.GetComponent<PlayerKillController>();
    //         playerTarget.OnHitPlayerClientRpc(senderClientId);
    //         // announce all players
    //         TestClientRpc(senderClientId);
    //
    //         // if (playerTarget.health - 10 == 0)
    //         // {
    //         //     OnKillSuccess(senderClientId);    
    //         // }
    //     }
    // }
    //
    // [ClientRpc]
    // void TestClientRpc(ulong killerId)
    // {
    //     GameObject killerPlayer = GameObject.Find($"Player-{killerId}");
    //     print($"Searching player {killerId} -- found? {killerPlayer}");
    //     killerPlayer.GetComponent<PlayerKillController>().shiftableWeapon.TimeShift();
    // }
    //
    // [ClientRpc]
    // void OnHitPlayerClientRpc(ulong killerId)
    // {
    //     health -= 10;
    //     print($"I was hit! Health: {health}, id: {NetworkObject.OwnerClientId}");
    //
    //     if (health <= 0)
    //     {
    //         health = 100;
    //         print($"{NetworkObject.OwnerClientId} was killed by {killerId}");
    //         // GetComponent<PlayerMovement>().Respawn();
    //     }
    // }
    //
    // void OnKillSuccess()
    // {
    //     killCount++;
    //     GameManager.GlobalKillCount++;
    //
    //     if (killCount % 1 == 0)
    //     {
    //         shiftableWeapon.TimeShift();
    //         // ShiftWeaponServerRpc(killerId);
    //     }
    // }
    //
    // [ServerRpc]
    // void ShiftWeaponServerRpc(ulong killerId, ServerRpcParams serverRpcParams = default)
    // {
    //     // ulong clientId = serverRpcParams.Receive.SenderClientId;
    //     if (NetworkManager.ConnectedClients.ContainsKey(killerId))
    //     {
    //         var client = NetworkManager.ConnectedClients[killerId];
    //         print($"server time shift for client id {killerId}");
    //         client.PlayerObject.GetComponent<PlayerKillController>().ShiftWeaponClientRpc();
    //     }
    // }

    // [ClientRpc]
    // void ShiftWeaponClientRpc()
    // {
    //     shiftableWeapon.TimeShift();
    // }
}