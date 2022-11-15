using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    public GameObject VRRig;

    public void buttonShot()
    {
        if (gameObject.name.Equals("Restart"))
        {
            VRRig.GetComponent<Restart>().restartButton = true;
        }
        else if (gameObject.name.Equals("Start"))
        {
            VRRig.GetComponent<Restart>().startButton = true;
        }
    }
}
