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
    private bool dashLeft;
    private bool dashRight;

    [Header("Movement Variables")]
    public float speed;
    private Vector3 velocity;

    [Header("Rotation Variables")]
    private float xRotation;
    private float sensitivity = 200f;

    [Header("Jump Variables")]
    public float jumpForce = 500f;

    [Header("Grounded Variables")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool grounded = true;

    [Header("Dash Variables")]
    public float dashCooldown = 1;
    public float dashSpeed = 5;
    private bool readyToDash = true;

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

        dashLeft = Input.GetButton("Dash Left");
        dashRight = Input.GetButton("Dash Right");
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Movement Functions
    //-----------------------------------------------------------------------------------//
    private void Move()
    {
        if (jumping)
        {
            Jump();
        }

        CalculateVelocity();

        // Applies velocity movement while also handling collisions
        playerRigidBody.MovePosition(transform.position + velocity);
    }

    private void Jump()
    {
        if (!grounded) return;
        
        grounded = false;

        // Apply jump forces
        playerRigidBody.AddForce(Vector3.up * jumpForce * 1.5f);
        jumping = false;   
    }

    private Vector3 Dash()
    {
        if (!readyToDash || (!dashLeft && !dashRight))
        {
            return Vector3.zero;
        }
        readyToDash = false;

        Vector3 dashVector = Vector3.zero;
        if (dashLeft)
        {
            dashVector = -transform.right * dashSpeed;
        }
        else if (dashRight)
        {
            dashVector = transform.right * dashSpeed;
        }

        Invoke(nameof(ResetDash), dashCooldown);
        return dashVector;
    }

    private void CalculateVelocity()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Vector3 dash = Dash();

        // Apply Velocity to the rigid body while also handling collisions
        Vector3 direction = (transform.forward * y) + (transform.right * x);
        velocity = (direction * speed * Time.fixedDeltaTime) + dash;
    }

    private void ResetDash()
    {
        readyToDash = true;
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
