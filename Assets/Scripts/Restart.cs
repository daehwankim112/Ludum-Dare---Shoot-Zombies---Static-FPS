using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manage beggining of game states

public class Restart : MonoBehaviour
{
    public bool startButton;
    public bool restartButton;
    private bool veryFirstIteration;
    private bool phase0;
    private bool phase1;
    private bool phase2;
    private bool phase3;
    private bool phase4;
    private bool phase5;
    private bool phase6;
    private float lastTime1;
    private float lastTime2;
    private int numberOfFire;
    private bool fired;
    private bool exploded;
    private float deltaTimeCumulated;
    private bool warSirenPlayed;
    public GameObject gunLights;
    public GameObject lights;
    public GameObject explosionParticles;
    public GameObject VRRig;
    public GameObject lightTowerLights;
    public GameObject startUI;
    public GameObject restartUI;
    public GameObject score;
    public AudioSource audioSource1;
    public AudioClip gunFireAudio;
    public AudioClip explosionAudio;
    public AudioClip warSirenAudio;


    public void InitiateScene()
    {
        lastTime1 = Time.deltaTime;
        lastTime2 = Time.deltaTime;
        phase0 = true;
        phase1 = true;
    }

    private void Start()
    {
        veryFirstIteration = true;
        startButton = false;
        restartButton = false;
        phase0 = false;
        phase1 = false;
        phase2 = false;
        phase3 = false;
        phase4 = false;
        phase5 = false;
        phase6 = false;
        warSirenPlayed = false;
        numberOfFire = 6;
        fired = false;
        exploded = false;
        deltaTimeCumulated = 0f;
        startUI.SetActive(false);
        restartUI.SetActive(false);
    }

    private void Update()
    {
        deltaTimeCumulated += Time.deltaTime;
        if (phase0)
        {
            if (! warSirenPlayed)
            {
                warSirenPlayed = true;
                audioSource1.PlayOneShot(warSirenAudio, 0.1f);
            }
            if (phase1) // Firing
            {
                //Debug.Log("Phase1");
                //lights.SirenON();
                lightTowerLights.SetActive(true);
                if (numberOfFire > 0 && deltaTimeCumulated - lastTime1 > 0.1f && !fired)
                {
                    lastTime2 = deltaTimeCumulated;
                    fired = true;
                    gunLights.SetActive(true);
                    numberOfFire--;
                    audioSource1.PlayOneShot(gunFireAudio);
                }
                else if (numberOfFire > 0 && deltaTimeCumulated - lastTime2 > 0.1f && fired)
                {
                    lastTime1 = deltaTimeCumulated;
                    fired = false;
                    gunLights.SetActive(false);
                }
                else if (numberOfFire == 0)
                {
                    phase1 = false;
                    phase2 = true;
                    lastTime2 = deltaTimeCumulated;
                    gunLights.SetActive(false);
                }
            }
            else if (phase2) // Hold for a moment
            {
                //Debug.Log("Phase2");
                warSirenPlayed = false;
                if (deltaTimeCumulated - lastTime2 > 1.5f)
                {
                    phase2 = false;
                    phase3 = true;
                }
            }
            else if (phase3) // explosion, lights off
            {
                //Debug.Log("Phase3");
                if (deltaTimeCumulated - lastTime2 > 10f)
                {
                    phase3 = false;
                    phase4 = true;
                }
                if (!exploded)
                {
                    exploded = true;
                    GameObject spawnBulletImpacts = Instantiate(explosionParticles, new Vector3(0f, 0f, 8.447f), new Quaternion(0f, 0f, 0f, 0f));
                    audioSource1.PlayOneShot(explosionAudio);
                    Destroy(spawnBulletImpacts, 2);
                    lightTowerLights.SetActive(false);
                }

            }
            else if (phase4) // Set start UI visible
            {
                //Debug.Log("Phase4");
                exploded = false;
                if ( veryFirstIteration )
                {
                    startUI.SetActive(true);
                }
                else
                {
                    restartUI.SetActive(true);
                }
                

                if (deltaTimeCumulated - lastTime2 > 12f && ( startButton || restartButton ) )
                {
                    phase4 = false;
                    phase5 = true;
                }
                else
                {
                    lastTime2 = deltaTimeCumulated - 12.1f;
                }
            }
            else if (phase5) // UI invisible, zoom out
            {
                startUI.SetActive(false);
                restartUI.SetActive(false);
                veryFirstIteration = false;
                startButton = false;
                restartButton = false;
                //Debug.Log("Phase5");
                //Lerp should be used
                //New terrain generation
                //VRRig.GetComponent<Zoom>().ZoomOut();
                if (deltaTimeCumulated - lastTime2 >13f)
                {
                    phase5 = false;
                    phase6 = true;
                }

            }
            else if (phase6) // gameplay
            {
                //Debug.Log("Phase6");
                if (deltaTimeCumulated - lastTime2 > 15f)
                {
                    phase6 = false;
                    phase0 = false;
                    deltaTimeCumulated = 0f;
                    lastTime1 = deltaTimeCumulated;
                    lastTime2 = deltaTimeCumulated;
                }
            }
        }
    }
}
