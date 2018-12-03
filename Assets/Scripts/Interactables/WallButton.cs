using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallButton : Interactable
{
    public Activatable[] toActivate;
    public SpriteOutline[] toHighlight;

    private bool occupied = false;

    public Vector3 crewmemberPosOffset = new Vector3(0, 0.1f, -0.1f);
    public Color crewmemberTint = Color.grey;

    private GameObject crewedBy;

    public override void OnInteract(CrewMember crewMember)
    {
        if (!occupied)
        {
            crewMember.activateFunction = ToggleState;
            crewMember.hoverEnterFunction = HighlightOn;
            crewMember.hoverExitFunction = HighlightOff;
            crewMember.moving = false;
            crewMember.moveMod = 0;

            crewedBy = crewMember.gameObject;

            //Set the crew members collider to let other crew walk past
            crewedBy.layer = 10;

            foreach(Transform child in crewedBy.transform)
            {
                child.gameObject.layer = 10;
            }

            crewedBy.transform.position += crewmemberPosOffset;
            crewedBy.GetComponent<Collider2D>().offset = -crewmemberPosOffset;
            crewedBy.GetComponent<SpriteRenderer>().color = crewmemberTint;
            crewedBy.GetComponent<SpriteRenderer>().sortingOrder--;

            

            //Prevent anyone else from interacting with this
            occupied = true;
        }
    }

    private void HighlightOn()
    {
        SpriteOutline mySo = GetComponent<SpriteOutline>();
        SpriteOutline crewSo = crewedBy.GetComponent<SpriteOutline>();

        mySo.HighlightOn();
        mySo.color = crewSo.color;
        crewSo.HighlightOn();

        foreach (Activatable a in toActivate)
        {
            SpriteOutline outline = a.gameObject.GetComponent<SpriteOutline>();
            if (outline != null)
            {
                outline.color = crewSo.color;
                outline.HighlightOn();
            }

            foreach(Transform child in a.transform)
            {
                SpriteOutline outlineB = child.GetComponent<SpriteOutline>();
                if (outlineB != null)
                {
                    outlineB.color = crewSo.color;
                    outlineB.HighlightOn();
                }
            }
        }

        foreach (SpriteOutline outline in toHighlight)
        {
            outline.color = crewSo.color;
            outline.HighlightOn();
        }
    }

    private void HighlightOff()
    {
        GetComponent<SpriteOutline>().HighlightOff();
        crewedBy.GetComponent<SpriteOutline>().HighlightOff();

        foreach (Activatable a in toActivate)
        {
            if (a.gameObject.GetComponent<SpriteOutline>())
            {
                a.gameObject.GetComponent<SpriteOutline>().HighlightOff();
            }

            foreach (Transform child in a.transform)
            {
                SpriteOutline outlineB = child.GetComponent<SpriteOutline>();
                if (outlineB != null)
                {
                    outlineB.HighlightOff();
                }
            }
        }

        foreach (SpriteOutline outline in toHighlight)
        {
            outline.HighlightOff();
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

    private void OnMouseOver()
    {
        if (occupied)
        {
            HighlightOn();
        }
    }
    private void OnMouseExit()
    {
        if (occupied)
        {
            HighlightOff();
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
