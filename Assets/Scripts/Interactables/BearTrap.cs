using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : Interactable
{
    Animator animator;

    public float jumpForceMin = 200;
    public float jumpForceMax = 300;
    public float snapTorque = 0.5f;

    private void Start()
    {
        animator = GetComponent<Animator>();

        animator.speed = 0;
    }

    public override void OnInteract(CrewMember crewMember)
    {
        crewMember.gameObject.GetComponent<Animator>().SetTrigger("death");
        crewMember.gameObject.layer = 10;
        crewMember.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.AddForce(Vector2.up * Random.Range(jumpForceMin, jumpForceMax));
        rb.AddTorque(Random.Range(-snapTorque, snapTorque));

        GetComponent<Collider2D>().isTrigger = false;

        gameObject.layer = 10;

        animator.speed = 2;
    }
}
