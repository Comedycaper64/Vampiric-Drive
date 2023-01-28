using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTarget : MonoBehaviour
{
    [SerializeField] private int health = 1;
    private GameObject character;
    [SerializeField] private Sprite skull1;
    [SerializeField] private Sprite skull2;
    [SerializeField] private Sprite skull3;
    [SerializeField] private AudioClip destroySFX;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        character = GameObject.FindGameObjectWithTag("Player");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (gameObject.tag != "Hang")
        {
            character.GetComponent<PlayerController>().IncreaseDashes();
        }

        if ((gameObject.tag == "Hang") && !GetComponent<HangPointScript>().open)
        {
            character.GetComponent<PlayerController>().IncreaseDashes();
            GetComponent<HangPointScript>().OpenHangPoint();
        }

        if (health <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            audioSource.PlayOneShot(destroySFX);
            StartCoroutine(DestroySelf());
        }
    }

    private IEnumerator DestroySelf()
    {
        if (skull2 != null)
        {
            GetComponent<SpriteRenderer>().sprite = skull2;
            yield return new WaitForSeconds(0.3f);
            GetComponent<SpriteRenderer>().sprite = skull3;
            yield return new WaitForSeconds(0.3f);
        }
        Destroy(gameObject);

    }
}
