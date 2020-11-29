using System;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class PlayerController : Entity
{
    [Header("Player Camera")]
    public Camera playerCamera;

    [Header("Energy Variables")]
    protected float energyGainTime = 1.0f;
    protected float energyGainElapsedTime = 0.0f;

    [Header("Graphics Variables")]
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

        UIhealthBar.fillAmount = (float)entityData.health / (float)entitySO.health;
        UIhealthText.text = String.Format("{0} / {1}", (int)entityData.health, (int)entitySO.health);
        UIenergyBar.fillAmount = (float)entityData.energy / (float)entitySO.energy;
        UIenergyText.text = String.Format("{0} / {1}", (int)entityData.energy, (int)entitySO.energy);
    }

    protected override void Update()
    {
        GetPlayerInput();
        Look();
        base.Update();

        energyGainElapsedTime += Time.deltaTime;
        if (energyGainElapsedTime > energyGainTime)
        {
            UpdateEnergy(entityData.energyGainRate);
            energyGainElapsedTime = 0.0f;
        }
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
    //Movement Functions
    //-----------------------------------------------------------------------------------//
    protected override void Dash()
    {
        if (RequiredEnergy(entityData.dashEnergy))
        {
            base.Dash();
            UpdateEnergy(-entityData.dashEnergy);
        }
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Rotation Functions
    //-----------------------------------------------------------------------------------//
    protected override void RotateGun()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        gun.transform.rotation = Quaternion.LookRotation(ray.direction, transform.up);
    }

    protected void Look()
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
    //Shooting Functions
    //-----------------------------------------------------------------------------------//
    protected override void Shoot()
    {
        if (RequiredEnergy(entityData.shootEnergy))
        {
            base.Shoot();
            UpdateEnergy(-entityData.shootEnergy);
        }
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Damage Functions
    //-----------------------------------------------------------------------------------//
    public override void DealDamage(int damage)
    {
        base.DealDamage(damage);
        UIhealthBar.fillAmount = (float)entityData.health / (float)entitySO.health;
        UIhealthText.text = String.Format("{0} / {1}", (int)entityData.health, (int)entitySO.health);
    }
    //-----------------------------------------------------------------------------------//

    //-----------------------------------------------------------------------------------//
    //Energy Functions
    //-----------------------------------------------------------------------------------//
    public void UpdateEnergy(float amount)
    {
        entityData.energy += amount;
        if (entityData.energy > entitySO.energy)
        {
            entityData.energy = entitySO.energy;
        }
        else if (entityData.energy <= 0)
        {
            entityData.energy = 0;
        }

        UIenergyBar.fillAmount = (float)entityData.energy / (float)entitySO.energy;
        UIenergyText.text = String.Format("{0} / {1}", (int)entityData.energy, (int)entitySO.energy);
    }

    public bool RequiredEnergy(float amount)
    {
        if (entityData.energy < amount)
        {
            return false;
        }
        return true;
    }
    //-----------------------------------------------------------------------------------//

}
