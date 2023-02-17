using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerKillController : NetworkBehaviour
{
    int health = 100;
    int killCount = 0;

    void Start() { }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    void OnKill()
    {
        killCount++;
        GameManager.globalKillCount++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            health -= 10;
            if (health <= 0)
            {
                GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
