using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovment : NetworkBehaviour
{
    //Player Roataion
    float mouseX;
    public float lookSpeed;
    public Transform cameraTurn;
    float mouseY;
    float rotationX;

    //Player Movement
    float xAxis;
    float zAxis;
    public float moveSpeed;
    CharacterController cc;
    Vector3 v;

    //Gravity
    public bool isGrounded;
    float radius;
    public LayerMask groundLayerMask;
    public Transform groundCheck;
    float gravity;
    Vector3 velocity; // TODO: v & velocity is redundancy

    // TODO: Abstraction for Network class
    // Server
    NetworkVariable<Vector3> moveVectorNetwork = new NetworkVariable<Vector3>();
    NetworkVariable<Quaternion> localRotationNetwork = new NetworkVariable<Quaternion>();
    Vector3 oldMoveVector = Vector3.zero;
    Vector3 moveVector = Vector3.zero;

    Quaternion oldLocalRotation = Quaternion.identity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Player Rotation
        lookSpeed = 500f;
        rotationX = 0f;

        //Player Movement
        moveSpeed = 20f;
        cc = GetComponent<CharacterController>();

        //Gravity
        isGrounded = false;
        radius = 0.6f;
        gravity = -9.81f;

        transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {

        if (IsServer)
        {
            UpdateServer();
        }

        if (IsClient && IsOwner)
        {
            UpdateClient();
        }
    }

    void UpdateServer()
    {
        //transform.Rotate(0, localRotationNetwork.Value.y, 0);

        // TODO: I think the delay bug is because this is just an update to the server. We
        // need move in the client as well
        cc.Move(moveVectorNetwork.Value);
    }

    void UpdateClient()
    {
        PlayerRotation();

        // TODO: Put this back in a move method
        xAxis = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        zAxis = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        v = transform.forward * zAxis + transform.right * xAxis;
        moveVector = v * moveSpeed * Time.deltaTime;

        if (oldMoveVector != moveVector || transform.localRotation != oldLocalRotation)
        {
            oldMoveVector = moveVector;
            oldLocalRotation = transform.localRotation;
            UpdateClientPositionServerRpc(moveVector, transform.localRotation);
        }
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(Vector3 vector, Quaternion localRotation)
    {
        moveVectorNetwork.Value = vector;
        localRotationNetwork.Value = localRotation;
    }

    void PlayerRotation()
    {
        mouseX = Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        transform.Rotate(0, mouseX, 0);
        cameraTurn.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    void Gravity()
    {

        if (Physics.CheckSphere(groundCheck.position, radius, groundLayerMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded == false)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0;
        }

        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            velocity.y += 6f;
        }

        cc.Move(velocity * Time.deltaTime);
    }
}
