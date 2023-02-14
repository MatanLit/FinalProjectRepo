using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    //Player Roataion
    float mouseX;
    public float lookSpeed = 45f;
    public Transform cameraTurn;
    float mouseY;
    float cameraX;

    //Player Movement
    float xAxis;
    float zAxis;
    public float moveSpeed = 10f;
    CharacterController cc;
    Vector3 V;

    //Gravity
    public bool isGrounded;
    float radius;
    public LayerMask groundLayerMask;
    public Transform groundCheck;
    float gravity;
    Vector3 velocity;


    // Start is called before the first frame update
    void Start()
    {
        //Player Roataion
        cameraX = 0f;

        //Player Movement
        cc = GetComponent<CharacterController>();

        //Gravity
        isGrounded = false;
        radius = 0.6f;
        gravity = -9.81f;



    }

    // Update is called once per frame
    void Update()
    {

        PlayerRotation();
        PlayerMove();
        Gravity();


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
        V = transform.forward * zAxis + transform.right * xAxis;
        cc.Move(V * moveSpeed * Time.deltaTime);

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
