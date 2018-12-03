using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    public bool roleIsRequired = true;
    public CrewMember.Role role = CrewMember.Role.RedShirt;

    public abstract void OnInteract(CrewMember crewMember);
    public virtual void OnInteractContinue(CrewMember crewMember) { }
    public virtual void OnInteractEnd(CrewMember crewMember) { }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        CrewMember crewMember = collider.gameObject.GetComponent<CrewMember>();
        if(crewMember != null && (!roleIsRequired || crewMember.role == role))
        {
            OnInteract(crewMember);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        CrewMember crewMember = collider.gameObject.GetComponent<CrewMember>();
        if (crewMember != null && (!roleIsRequired || crewMember.role == role))
        {
            OnInteractContinue(crewMember);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        CrewMember crewMember = collider.gameObject.GetComponent<CrewMember>();
        if (crewMember != null && (!roleIsRequired || crewMember.role == role))
        {
            OnInteractEnd(crewMember);
        }
    }
}
