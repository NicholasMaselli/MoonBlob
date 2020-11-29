using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GravityAttractor initialMoon;
    public PlayerController localPlayer;

    // Game Data
    [Header("Game Data")]
    public DataDB dataDB;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("GameManager can only exist once in the scene");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        dataDB.Initialize();
    }
}
