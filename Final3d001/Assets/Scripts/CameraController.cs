using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public GameObject mainCamera;

    private void Start()
    {
        if (IsClient && IsOwner)
        {
            mainCamera.SetActive(true);
        }
    }

    private void Update()
    {
        if (mainCamera.activeSelf == false)
        {
            return;
        }

        mainCamera.transform.position = transform.position;
    }
}
