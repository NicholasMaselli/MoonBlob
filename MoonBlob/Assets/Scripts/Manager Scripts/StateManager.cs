using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    [Header("Game Data")]
    public Difficulty difficulty = Difficulty.Easy;
    public DataDB dataDB;
    public PlayerData playerData;

    //-----------------------------------------------------------------------------------//
    //Initialization and Update Functions
    //-----------------------------------------------------------------------------------//
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        //Keep this StateManager if it is the only one in the scene
        DontDestroyOnLoad(this.gameObject);
        instance = this;

        dataDB.Initialize();
    }
    //-----------------------------------------------------------------------------------//
}
