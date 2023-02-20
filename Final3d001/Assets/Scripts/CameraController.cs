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
        if (!IsClient)
        {
            return;
        };
        
        if (IsOwner)
        {
            mainCamera.SetActive(true);
        }
        else
        {
            mainCamera.SetActive(false);
            // mainCamera.transform.GetChild(0).transform.parent = transform;
            
            // FIXME: foreach only works on the first child
            // foreach (Transform childObject in mainCamera.transform)
            // {
            //     print($"Moving {childObject.name} to {transform.name}");
            //     childObject.transform.parent = transform;
            //     childObject.transform.position = transform.position;
            //     childObject.gameObject.SetActive(true);
            // }
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
