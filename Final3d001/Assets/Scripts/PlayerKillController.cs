using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerKillController : NetworkBehaviour
{
    public int health = 100;
    int killCount = 0;
    private TimeShiftableObject shiftableWeapon;

    void Start()
    {
        if (IsClient && IsOwner)
        {
            // shiftableWeapon = GameObject.Find("Weapon").GetComponent<TimeShiftableObject>();
        }
    }

    void Update()
    {
        if (!IsClient || !IsOwner)
        {
            return;
        }
        
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
            };
            
            // if (hit.collider.gameObject.CompareTag("Target"))
            // {
            //     Destroy(hit.collider.gameObject);
            //     shiftableWeapon.TimeShift();
            //     OnKill();
            // }
                
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                PlayerKillController hitPlayer = hit.collider.gameObject.GetComponent<PlayerKillController>();
                hitPlayer.OnHit();

                if (hitPlayer.health <= 0)
                {
                    OnKill();
                }
            }
        }
    }

    public void OnHit()
    {
        health -= 10;
        print("Player hit! Health: " + health);

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
