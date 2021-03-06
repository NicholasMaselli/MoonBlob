﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

public class HomeManager : MonoBehaviour
{
    public static HomeManager instance;

    [Header("Trophy Variables")]
    public BetterButton easyButton;
    public BetterButton normalButton;
    public BetterButton hardButton;
    public BetterButton impossibleButton;

    [Header("Trophy Variables")]
    public Image easyTrophy;
    public Image normalTrophy;
    public Image hardTrophy;
    public Image impossibleTrophy;

    [Header("Options Menu")]
    public GameObject optionsMenu;

    [Header("Mystery Blob")]
    public Image oobbyImage;
    public TextMeshProUGUI oobbyText;
    public Image roobooImage;
    public TextMeshProUGUI roobooText;
    public Image broogrImage;
    public TextMeshProUGUI broogrText;
    public Image bluubImage;
    public TextMeshProUGUI bluubText;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("HomeManager can only exist once in the scene");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        PlayerData playerData = SaveSystem.LoadPlayer();
        if (playerData != null)
        {
            if (playerData.easy != 0)
            {
                easyTrophy.gameObject.SetActive(true);
                easyTrophy.sprite = StateManager.instance.dataDB.trophySprites[playerData.easy - 1];
            }

            if (playerData.normal != 0)
            {
                normalTrophy.gameObject.SetActive(true);
                normalTrophy.sprite = StateManager.instance.dataDB.trophySprites[playerData.normal - 1];
            }

            if (playerData.hard != 0)
            {
                hardTrophy.gameObject.SetActive(true);
                hardTrophy.sprite = StateManager.instance.dataDB.trophySprites[playerData.hard - 1];

                // If hard mode is complete, show impossible mode
                impossibleButton.gameObject.SetActive(true);
            }

            if (playerData.impossible != 0)
            {
                impossibleTrophy.gameObject.SetActive(true);
                impossibleTrophy.sprite = StateManager.instance.dataDB.trophySprites[playerData.impossible - 1];
            }
            RevealBlobs(playerData);
            StateManager.instance.playerData = playerData;
        }
        else
        {
            StateManager.instance.playerData = new PlayerData(0, 0, 0, 0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            ToggleOptions();
        }
    }

    public void Play(int difficulty)
    {
        StateManager.instance.difficulty = (Difficulty)difficulty;
        SceneManager.LoadScene("MoonBlob");
    }

    public void ToggleOptions()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void RevealBlobs(PlayerData playerData)
    {
        if (playerData.impossible > 0)
        {
            bluubImage.gameObject.SetActive(false);
            bluubText.gameObject.SetActive(true);
        }

        if (playerData.hard > 0)
        {
            broogrImage.gameObject.SetActive(false);
            broogrText.gameObject.SetActive(true);
        }

        if (playerData.normal > 0)
        {
            roobooImage.gameObject.SetActive(false);
            roobooText.gameObject.SetActive(true);
        }

        if (playerData.easy > 0)
        {
            oobbyImage.gameObject.SetActive(false);
            oobbyText.gameObject.SetActive(true);
        }
    }
}
