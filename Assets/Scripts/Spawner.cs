using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unity Create a Game Series (E05. spawn system) - Sebastian Lague


public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public GameObject enemy;
    public GameObject VRRig;
    public GameObject lightTowerLights;
    public bool gameOver;
    public int score;
    private Wave currentWave;
    private int currentWaveNumber;
    private int enemiesRamainingToSpawn;
    private float nextSpawnTime;
    private bool started;
    private bool sirenIsOn;
    private float time;
    private float deltaTimeCumulated;

    private void Start()
    {
        score = 0;
        nextWave();
        started = false;
        sirenIsOn = false;
        deltaTimeCumulated = Time.deltaTime;
        gameOver = false;
    }

    private void Update()
    {
        if (enemiesRamainingToSpawn > 0 && Time.time > nextSpawnTime && (VRRig.GetComponent<Restart>().startButton || VRRig.GetComponent<Restart>().restartButton || started))
        {
            started = true;
            enemiesRamainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            GameObject spawnedEnemy = Instantiate(enemy, new Vector3(Random.Range(-40f, 40f), Random.Range(0.5f, 2f), Random.Range(50f, 100f)), Quaternion.identity) as GameObject;
            spawnedEnemy.transform.parent = gameObject.transform;
        }
        else if (enemiesRamainingToSpawn == 0 )
        {
            nextWave();
        }

        // siren is on
        deltaTimeCumulated += Time.deltaTime;
        if ( ! sirenIsOn )
        {
            time = deltaTimeCumulated;
        }
        if (sirenIsOn)
        {
            lightTowerLights.SetActive(true);
            VRRig.GetComponent<Restart>().audioSource1.PlayOneShot(VRRig.GetComponent<Restart>().warSirenAudio, 0.1f);
            if (deltaTimeCumulated - time > 5f)
            {
                lightTowerLights.SetActive(false);
                sirenIsOn = false;
            }
        }
    }

    public void GameOver()
    {
        gameOver = true;
        for (int i = 0 ; i < gameObject.transform.childCount; i ++ )
        {
            gameObject.transform.GetChild(i).gameObject.GetComponent<Target>().TakeDamage(gameObject.transform.GetChild(i).gameObject.GetComponent<Target>().health + 1f);
        }
        started = false;
        gameOver = false;
        VRRig.GetComponent<Restart>().InitiateScene();
    }

    void nextWave()
    {
        if ( currentWaveNumber < waves.Length )
        {
            currentWaveNumber ++;
            currentWave = waves[currentWaveNumber - 1];
            enemiesRamainingToSpawn = currentWave.enemyCount;
        }
        else
        {
            currentWaveNumber = 0;
        }
        
    }

    public void Siren()
    {
        sirenIsOn = true;
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
