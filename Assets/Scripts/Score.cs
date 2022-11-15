using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score;
    public GameObject spawner;
    public GameObject VRrig;
    public TMP_Text textMesh;

    void Start()
    {

    }

    private void Update()
    {
        //textMesh.text = spawner.GetComponent<Spawner>().score.ToString();
        if (VRrig.GetComponent<Restart>().restartButton)
        {
            score = 0;
        }
    }
}
