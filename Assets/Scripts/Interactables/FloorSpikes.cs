using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpikes : Interactable {

    private float fallTime = 0;
    private bool fall = false;

	public override void OnInteract(CrewMember crewMember)
    {
        Debug.Log("Interacted!");

        crewMember.gameObject.GetComponent<Animator>().enabled = false;

        StartCoroutine("Fall", crewMember);
        crewMember.enabled = false;
    }

    IEnumerator Fall(CrewMember crewMember)
    {
        Debug.Log("Fall start");
        for (float t = 0f; t <= 1; t += 0.006f)
        {
            Debug.Log("Fall Time:" + t);
            crewMember.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -1*((1 - t) * 0 + t * 90));
            yield return null;
        }
    }
}

