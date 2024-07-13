using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource;

    private void DestroySelf()
    {
        Destroy(audioSource.gameObject);
        Destroy(this.gameObject);
    }

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.pitch = Random.Range(.9f, 1.1f);
        audioSource.Play();
        Invoke("DestroySelf", audioClip.length + .5f);
    }
}
