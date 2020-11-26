using System;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Transforms")]
    public Rigidbody playerRigidBody;
    public Transform playerCamera;

    [Header("Input")]
    private float x;
    private float y;
    private float mouseHorizontal;
    private float mouseVertical;
    private bool jumping;
    private bool sprinting;

    [Header("Movement Variables")]
    public float speed;

    [Header("Rotation Variables")]
    private float xRotation;
    private float sensitivity = 200f;

    [Header("Jump Variables")]
    private bool readyToJump = true;
    public float jumpForce = 500f;

    //-----------------------------------------------------------------------------------//
    //PlayerController Initialization and Update
    //-----------------------------------------------------------------------------------//
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetPlayerInput();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetPlayerInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        mouseHorizontal = Input.GetAxis("Mouse X");
        mouseVertical = Input.GetAxis("Mouse Y");
        jumping = Input.GetButton("Jump");
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Movement Functions
    //-----------------------------------------------------------------------------------//
    private void Move()
    {
        if (readyToJump && jumping)
        {
            Jump();
        }

        //Apply move forces
        //playerRigidBody.AddForce(transform.forward * y * speed * Time.fixedDeltaTime);
        //playerRigidBody.AddForce(transform.right * x * speed * Time.fixedDeltaTime);

        // Apply Velocity to the rigid body while also handling collisions
        Vector3 direction = (transform.forward * y) + (transform.right * x);
        Vector3 velocity = direction * speed * Time.fixedDeltaTime;
        playerRigidBody.MovePosition(transform.position + velocity);
    }

    private void Jump()
    {
        //readyToJump = false;

        //Apply jump forces
        playerRigidBody.AddForce(Vector3.up * jumpForce * 1.5f);
        jumping = false;
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Rotation Functions
    //-----------------------------------------------------------------------------------//
    private void Look()
    {
        float horizontalLook = mouseHorizontal * sensitivity * Time.deltaTime;
        float verticalLook = mouseVertical * sensitivity * Time.deltaTime;

        // Calculate Rotations
        Vector3 rotation = playerCamera.transform.localRotation.eulerAngles;
        float yRotation = rotation.y + horizontalLook;

        xRotation -= verticalLook;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * yRotation);
    }
    //-----------------------------------------------------------------------------------//

}
