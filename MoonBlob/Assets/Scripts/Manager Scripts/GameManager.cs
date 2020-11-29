using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GravityAttractor initialMoon;
    public PlayerController localPlayer;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("GameManager can only exist once in the scene");
            return;
        }
        instance = this;
    }
}
