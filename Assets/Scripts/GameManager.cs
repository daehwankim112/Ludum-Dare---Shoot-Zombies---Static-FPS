using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Game Manager - Controlling the flow of your game [Unity Tutorial] by Tarodev
public class GameManager : MonoBehaviour
{
    private string gameState;
    public GameObject restart;

    void Start()
    {
        gameState = "Beggining_Beggining";
    }

    private void Update()
    {
        if (gameState.Equals("Beggining_Beggining"))
        {
            Beggining_Beggining();
        }
        else if (gameState.Equals("Gameplay_Gameplay"))
        {
            Gameplay_Gameplay();
        }
    }

    private void Gameplay_Gameplay()
    {

    }

    private void Beggining_Beggining()
    {
        restart.GetComponent<Restart>().InitiateScene();
        gameState = "Gameplay_Gameplay";
    }


}

