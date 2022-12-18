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

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        // Get Score
        score = spawner.GetComponent<Spawner>().score;
        textMesh.text = "" + score;

        if (VRrig.GetComponent<Restart>().restartButton)
        {
            spawner.GetComponent<Spawner>().score = 0;
        }
    }
}
