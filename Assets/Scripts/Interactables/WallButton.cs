﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallButton : Interactable
{
    public Activatable[] toActivate;

    private bool occupied = false;

    public Vector3 crewmemberPosOffset = new Vector3(0, 0.1f, -0.1f);
    public Color crewmemberTint = Color.grey;

    public override void OnInteract(CrewMember crewMember)
    {
        if (!occupied)
        {
            crewMember.activateFunction = ToggleState;
            crewMember.moving = false;

            //Set the crew members collider to let other crew walk past
            crewMember.gameObject.layer = 10;

            crewMember.transform.position += crewmemberPosOffset;
            crewMember.GetComponent<Collider2D>().offset = -crewmemberPosOffset;
            crewMember.GetComponent<SpriteRenderer>().color = crewmemberTint;
            crewMember.GetComponent<SpriteRenderer>().sortingOrder--;

            //Prevent anyone else from interacting with this
            occupied = true;
        }
    }

    private void ToggleState()
    {
        foreach(Activatable a in toActivate)
        {
            a.OnActivate(gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (occupied)
        {
            ToggleState();
        }
    }

    private void OnDrawGizmos()
    {
        if (toActivate != null)
        {
            Gizmos.color = Color.cyan;
            foreach (Activatable a in toActivate)
            {
                if (a != null)
                {
                    Gizmos.DrawLine(transform.position, a.transform.position);
                }
            }
        }
    }
}
