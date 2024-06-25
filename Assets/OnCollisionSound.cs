using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionSound : MonoBehaviour
{
    public AudioClip clip;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        source.PlayOneShot(clip);
    }
}
