using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GlassInteractable : Interactable {

    public bool active = true;

    public override void OnInteract(CrewMember crewMember)
    {
        crewMember.glassed = true;
        crewMember.hoverExitFunction();
    }

    public override void OnInteractContinue(CrewMember crewMember)
    {
        
    }

    public override void OnInteractEnd(CrewMember crewMember)
    {
        crewMember.glassed = false;
    }
}
