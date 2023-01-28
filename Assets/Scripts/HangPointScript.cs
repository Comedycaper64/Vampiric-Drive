using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangPointScript : MonoBehaviour
{
    [SerializeField] private float openTime = 5f;
    public bool open = false;
    [SerializeField] private Sprite candelbara1;
    [SerializeField] private Sprite candelbara2;
    [SerializeField] private Sprite candelbara3;
    [SerializeField] private AudioClip extendSFX;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OpenHangPoint()
    {
        open = true;
        audioSource.PlayOneShot(extendSFX);
        StartCoroutine(OpenCandle());
        StartCoroutine(OpenTime());
    }

    private IEnumerator OpenTime()
    {
        yield return new WaitForSeconds(openTime);
        open = false;
        StartCoroutine(CloseCandle());
    }

    public IEnumerator OpenCandle()
    {
        GetComponent<SpriteRenderer>().sprite = candelbara2;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().sprite = candelbara3;
    }

    public IEnumerator CloseCandle()
    {
        GetComponent<SpriteRenderer>().sprite = candelbara2;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().sprite = candelbara1;
    }
}
