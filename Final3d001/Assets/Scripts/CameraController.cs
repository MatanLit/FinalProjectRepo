using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public GameObject mainCamera;

    private void Start()
    {
        if (IsClient)
        {
            if (IsOwner)
            {
                mainCamera.SetActive(true);
            }
            else
            {
                foreach (Transform childObject in mainCamera.transform)
                {
                    childObject.transform.parent = transform;
                }
            }
        }
    }

    private void Update()
    {
        if (mainCamera.activeSelf == false)
        {
            return;
        }
    }
}
