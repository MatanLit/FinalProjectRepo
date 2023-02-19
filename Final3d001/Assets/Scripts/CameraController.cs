using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public GameObject mainCamera;
    [SerializeField] Transform playerTransform;

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
                foreach (Transform childObject in transform)
                {
                    childObject.transform.parent = playerTransform;
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
