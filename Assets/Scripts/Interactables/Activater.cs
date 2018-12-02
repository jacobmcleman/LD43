﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activater : Interactable
{
    public Activatable[] toActivate;

    private GameObject crewedBy;

    public override void OnInteract(CrewMember crewMember)
    {
        ToggleState();
    }

    private void HighlightOn()
    {
        SpriteOutline crewSo = crewedBy.GetComponent<SpriteOutline>();

        foreach (Activatable a in toActivate)
        {
            SpriteOutline outline = a.gameObject.GetComponent<SpriteOutline>();
            if (outline != null)
            {
                outline.color = crewSo.color;
                outline.HighlightOn();
            }
        }
    }

    private void HighlightOff()
    {
        foreach (Activatable a in toActivate)
        {
            if (a.gameObject.GetComponent<SpriteOutline>())
            {
                a.gameObject.GetComponent<SpriteOutline>().HighlightOff();
            }
        }
    }

    private void ToggleState()
    {
        foreach(Activatable a in toActivate)
        {
            a.OnActivate(gameObject);
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
