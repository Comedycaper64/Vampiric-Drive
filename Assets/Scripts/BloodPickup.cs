using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPickup : MonoBehaviour
{
    [SerializeField] int bloodToRestore = 10;
    [SerializeField] bool singleUse = false;
    [SerializeField] private AudioClip drinkBloodSFX;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            otherCollider.GetComponent<PlayerController>().RecoverBlood(bloodToRestore);
            audioSource.PlayOneShot(drinkBloodSFX);
            if (singleUse)
            {
                Destroy(gameObject);
            }
        }
    }

}
