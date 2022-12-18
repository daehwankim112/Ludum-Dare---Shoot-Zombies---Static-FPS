using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// How to Make a Realistic Shotgun in VR - Valem Tutorials

public class Rifle : MonoBehaviour
{
    public InputHelpers.Button shootingButton;
    public XRNode shootingNode;

    private bool wasPressed = false;
    public GameObject shootingParticles;
    public Transform shootingPoint;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip audioClipHit;
    public Animator shootingAnimator;
    public float shootingDistance = 100f;
    public float shootingRadius = 0.1f;
    public LayerMask killableLayers;
    public float damage = 10f;
    private GameObject closestZombie;
    public LayerMask terrainLayerMask;

    void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(shootingNode), shootingButton, out bool isPressed);
        if(isPressed)
        {
            if (!wasPressed)
                Shoot();
            wasPressed = true;
        }
        else
        {
            wasPressed = false;
        }
    }

    private void Shoot()
    {
        // Find the hit point
        RaycastHit hit;
        if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out hit, shootingDistance, terrainLayerMask))
        {
            GameObject spawnBulletImpacts = Instantiate(shootingParticles, hit.point, new Quaternion(0f, 0f, 0f, 0f));
            Destroy(spawnBulletImpacts, 2);
        }

        // Add particle effect
        GameObject spawnParticles = Instantiate(shootingParticles, shootingPoint.position, shootingPoint.rotation);
        Destroy(spawnParticles, 2);

        // Add shooting sound
        audioSource.PlayOneShot(audioClip);

        // Add trigger haptic feedback
        InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(shootingNode);
        if(rightHandDevice.TryGetHapticCapabilities(out HapticCapabilities capabilities) && capabilities.supportsImpulse)
        {
            rightHandDevice.SendHapticImpulse(0, 0.5f, 0.1f);
        }

        // Add a gun animation
        shootingAnimator.SetTrigger("Trigger");

        // How to kill someone
        Collider[] killList = Physics.OverlapCapsule(shootingPoint.position, shootingPoint.position + shootingPoint.forward * shootingDistance, shootingRadius, killableLayers);
        
        if ( killList.Length > 0 )
        {
            closestZombie = killList[0].gameObject;
        }
        foreach (var item in killList)
        {
            if (Vector3.Magnitude(shootingPoint.position - item.gameObject.transform.position) < Vector3.Magnitude(shootingPoint.position - closestZombie.transform.position))
                closestZombie = item.gameObject;
        }
        if (closestZombie != null && killList.Length > 0)
        {
            Debug.Log(closestZombie.name);
            Target target = closestZombie.transform.GetComponent<Target>();
            UIButtons UIButton = closestZombie.transform.GetComponent<UIButtons>();
            if (target != null)
            {
                target.TakeDamage(damage);
                audioSource.PlayOneShot(audioClipHit);
            }
            else if (UIButton != null)
            {
                UIButton.buttonShot();
                UIButton = null;
            }
        }
        Array.Clear(killList, 0, killList.Length);
    }
}
