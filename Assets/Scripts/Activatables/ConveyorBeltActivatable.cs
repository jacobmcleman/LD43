using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConveyorBeltActivatable : Activatable
{
    private ConveyorBeltInteractable interacter;

    private void Start()
    {
        interacter = GetComponent<ConveyorBeltInteractable>();
    }

    public override void OnActivate(GameObject activatedBy)
    {
        interacter.active = !interacter.active;
    }
}
