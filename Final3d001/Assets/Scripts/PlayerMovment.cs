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
    NetworkVariable<Vector3> moveVectorNetwork = new NetworkVariable<Vector3>();
    Vector3 oldMoveVector = Vector3.zero;
    Vector3 moveVector = Vector3.zero;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        //Player Rotation
        lookSpeed = 200f;
        cameraX = 0f;

        //Player Movement
        cc = GetComponent<CharacterController>();

        //Gravity
        isGrounded = false;
        radius = 0.6f;
        gravity = -9.81f;

         transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        PlayerRotation();
        Gravity();

        print($"{moveVectorNetwork.Value}");

        if (IsServer)
        {
            GameObject.Find("IsServer").GetComponent<TextMeshProUGUI>().text = "IS SERVER";
            moveVector = moveVectorNetwork.Value;
            PlayerMove();
            cc.Move(moveVector);
        }

        if (IsClient && IsOwner)
        {
            GameObject.Find("IsClient").GetComponent<TextMeshProUGUI>().text = "IS CLIENT + OWNER";
            if (oldMoveVector != moveVector)
            {
                oldMoveVector = moveVector;
                PlayerMove();
                UpdateClientPositionServerRpc(moveVector);
            }
        }
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(Vector3 moveVector)
    {
        moveVectorNetwork.Value = moveVector;
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
        xAxis = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        zAxis = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        v = transform.forward * zAxis + transform.right * xAxis;

        moveVector = v * moveSpeed * Time.deltaTime;
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
