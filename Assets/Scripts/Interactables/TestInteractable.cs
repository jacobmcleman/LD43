using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : Interactable {

	public override void OnInteract(CrewMember crewMember)
    {
        Debug.Log("Interacted with by: " + crewMember.gameObject.name);
    }
}
