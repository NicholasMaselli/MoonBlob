﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    public static HomeManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("HomeManager can only exist once in the scene");
            return;
        }
        instance = this;
    }

    public void Play(int difficulty)
    {
        Debug.Log(difficulty);
    }
}