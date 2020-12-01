using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    public Difficulty difficulty = Difficulty.Easy;

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
    }
    //-----------------------------------------------------------------------------------//
}
