using System;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class PlayerController : Entity
{
    [Header("Player Camera")]
    public Camera playerCamera;

    [Header("Graphics Functions")]
    public Image UIhealthBar;
    public TextMeshProUGUI UIhealthText;
    public Image UIenergyBar;
    public TextMeshProUGUI UIenergyText;

    //-----------------------------------------------------------------------------------//
    //PlayerController Initialization and Update
    //-----------------------------------------------------------------------------------//
    public override void Start()
    {
        base.Start();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void Update()
    {
        GetPlayerInput();
        Look();
        base.Update();
    }    

    private void GetPlayerInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        mouseHorizontal = Input.GetAxis("Mouse X");
        mouseVertical = Input.GetAxis("Mouse Y");
        jumping = Input.GetButtonDown("Jump");
        dashLeft = Input.GetButtonDown("Dash Left");
        dashRight = Input.GetButtonDown("Dash Right");

        shoot = Input.GetMouseButtonDown(0);
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Rotation Functions
    //-----------------------------------------------------------------------------------//
    public override void RotateGun()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        gun.transform.rotation = Quaternion.LookRotation(ray.direction, transform.up);
    }

    private void Look()
    {
        float horizontalLook = mouseHorizontal * sensitivity * Time.deltaTime;
        float verticalLook = mouseVertical * sensitivity * Time.deltaTime;

        // Calculate Rotations
        Vector3 rotation = playerCamera.transform.localRotation.eulerAngles;
        float yRotation = rotation.y + horizontalLook;

        xRotation -= verticalLook;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        //Perform the rotations
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * yRotation);
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Damage Functions
    //-----------------------------------------------------------------------------------//
    public override void DealDamage(int damage)
    {
        base.DealDamage(damage);
        UIhealthBar.fillAmount = (float)entityData.health / (float)entitySO.health;
        UIhealthText.text = String.Format("{0} / {1}", entityData.health, entitySO.health);
    }
    //-----------------------------------------------------------------------------------//

}
