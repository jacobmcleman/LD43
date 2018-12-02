using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : Interactable {

    public GameObject winCanvas;

    public override void OnInteract(CrewMember crewMember)
    {
        crewMember.enabled = false;
        crewMember.GetComponent<Animator>().enabled = false;
        winCanvas.SetActive(true);
    }
}
