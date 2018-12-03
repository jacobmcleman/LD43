using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConveyorBeltInteractable : Interactable {

    public bool active = true;
    public float speed = 10;
    public float animSpeedMultiplier = 1;

    private void Update()
    {
        float realSpeed = speed;
        if (!active) realSpeed = 0;

        float animSpeed = Mathf.Abs(realSpeed) * animSpeedMultiplier;

        GetComponentInChildren<Tilemap>().animationFrameRate = animSpeed;
    }

    public override void OnInteract(CrewMember crewMember)
    {
        //Empty? 
    }

    public override void OnInteractContinue(CrewMember crewMember)
    {
        if (active)
        {
            crewMember.moveMod = speed > 0 ? 1 : -1;
        }
        else
        {
            crewMember.moveMod = 0;
        }
    }

    public override void OnInteractEnd(CrewMember crewMember)
    {
        crewMember.moveMod = 0;
    }
}
