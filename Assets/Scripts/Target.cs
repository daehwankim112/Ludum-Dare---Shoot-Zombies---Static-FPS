using UnityEngine;

// Shooting with Raycasts - Unity Tutorial - Brackeys


public class Target : MonoBehaviour
{
    public float health;
    public Animator zombieAnimator;
    public GameObject fire;
    public GameObject spawner;
    public float speed;
    private bool sirenIsOn;

    void Start()
    {
        zombieAnimator = gameObject.GetComponent<Animator>();
        fire = GameObject.Find("Barrel Light");
        spawner = GameObject.Find("Spawner");
        sirenIsOn = false;
    }

    void Update()
    {
        // Dead. Falling into the ground
        if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Z_FallingBack") || zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Z_FallingForward"))
        {
            gameObject.transform.position += new Vector3(0, -0.001f, 0);
        }
        // Walking toward the fire
        else if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Z_Walk1_InPlace"))
        {
            gameObject.transform.position += Vector3.Normalize(fire.transform.position - new Vector3(0f, 1.917f, 0f) - gameObject.transform.position) * speed;
            Vector3 direction = fire.transform.position - new Vector3 (0f, 1.917f, 0f) - gameObject.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            gameObject.transform.rotation = rotation;
            float distance = (fire.transform.position + new Vector3(0f, 1.917f, 0f) - gameObject.transform.position).sqrMagnitude;
            if (distance < 50f)
            {
                spawner.GetComponent<Spawner>().Siren();
            }
            if (distance < 30f)
            {
                spawner.GetComponent<Spawner>().GameOver();
            }
        }


    }

    // Take damage from rifle
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            if (zombieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Z_Walk1_InPlace"))
            {
                zombieAnimator.SetTrigger("noSirenDeath");
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }
            else
            {
                zombieAnimator.SetTrigger("sirenDeath");
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }
            Die();
        }
        if (gameObject.name.Equals("Restart"))
        {
            gameObject.GetComponent<Restart>().restartButton = true;
        }
        else if (gameObject.name.Equals("Start"))
        {
            gameObject.GetComponent<Restart>().startButton = true;
        }
    }

    // Handle Die
    public void Die()
    {
        if (! spawner.GetComponent<Spawner>().gameOver)
        {
            spawner.GetComponent<Spawner>().score++;
        }
        Destroy(gameObject, 10);
    }
}
