using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassClicky : MonoBehaviour {

    AudioSource glassScreamer;
    public AudioClip glassTap;

    private void start()
    {
        glassScreamer = GetComponent<AudioSource>();
    }
    // Use this for initialization
    private void OnMouseDown()
    {
        glassScreamer.PlayOneShot(glassTap);
        Debug.Log("Hello");

    }
}
