using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovment : NetworkBehaviour
{
    //Player Roataion
    float mouseX;
    public float lookSpeed;
    public Transform cameraTurn;
    float mouseY;
    float cameraX;

    //Player Movement
    float xAxis;
    float zAxis;
    public float moveSpeed = 10f;
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
    NetworkVariable<float> serverHorizontalPosition = new NetworkVariable<float>();
    NetworkVariable<float> serverVerticalPosition = new NetworkVariable<float>();
    float oldHorizontalPosition = 0;
    float oldVerticalPosition = 0;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Player Rotation
        lookSpeed = 200f;
        cameraX = 0f;

        //Player Movement
        cc = GetComponent<CharacterController>();

        //Gravity
        isGrounded = false;
        radius = 0.6f;
        gravity = -9.81f;

        // transform.position = new Vector3(120, 2, 0);
    }

    void Update()
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        PlayerRotation();
        PlayerMove();
        Gravity();

        // if (IsServer)
        // {
        //     // Actual update of values of server
        //     PlayerMove();
        //     Gravity();
        // }

        // if (IsClient && IsOwner)
        // {
        //     bool isPositionChanged = oldHorizontalPosition != xAxis || oldVerticalPosition != zAxis;
        //     if (isPositionChanged)
        //     {
        //         oldVerticalPosition = zAxis;
        //         oldHorizontalPosition = xAxis;
        //         UpdateClientPositionServerRpc(xAxis, zAxis);
        //     }
        // }
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float horizontal, float vertical)
    {
        serverHorizontalPosition.Value = horizontal;
        serverVerticalPosition.Value = vertical;
    }

    void PlayerRotation()
    {

        mouseX = Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
        transform.Rotate(0, mouseX, 0);
        mouseY = Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;
        cameraTurn.Rotate(mouseY, 0, 0);
        cameraX -= mouseY;
        cameraX = Mathf.Clamp(cameraX, -90, 90);
        cameraTurn.localRotation = Quaternion.Euler(cameraX, 0, 0);

    }

    void PlayerMove()
    {
        // serverHorizontalPosition.Value + serverVerticalPosition.Value?
        xAxis = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        zAxis = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        v = transform.forward * zAxis + transform.right * xAxis;
        print($"v {v} move: {v * moveSpeed * Time.deltaTime}");
        cc.Move(v * moveSpeed * Time.deltaTime);
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
