using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : Interactable {

    public GameObject winCanvas;

    AudioSource winScreamer;
    public AudioClip winSound;

    public override void OnInteract(CrewMember crewMember)
    {
        if (!crewMember.Dead)
        {
            crewMember.enabled = false;
            crewMember.GetComponent<Animator>().enabled = false;
            crewMember.GetComponent<AudioSource>().enabled = false;
            winScreamer = GetComponent<AudioSource>();
            winScreamer.PlayOneShot(winSound, .7F);
            winCanvas.SetActive(true);
            LevelSelectButton.SetThisLevelCompleted(true);
        }
    }
}
