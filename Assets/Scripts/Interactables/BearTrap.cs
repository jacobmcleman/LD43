using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : Interactable
{
    Animator animator;

    public float jumpForceMin = 200;
    public float jumpForceMax = 300;
    public float snapTorque = 0.5f;

    AudioSource trapScreamer;

    public Color inactiveColor = Color.grey;

    private bool playedLandSound = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        animator.speed = 0;

        trapScreamer = GetComponent<AudioSource>();
    }

    public override void OnInteract(CrewMember crewMember)
    {
        GetComponent<SpriteRenderer>().color = inactiveColor;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.AddForce(Vector2.up * Random.Range(jumpForceMin, jumpForceMax));
        rb.AddTorque(Random.Range(-snapTorque, snapTorque));

        GetComponent<Collider2D>().isTrigger = false;

        gameObject.layer = 11;

        GetComponent<SpriteRenderer>().sortingOrder = 9;

        animator.speed = 2;

        crewMember.Die();

        trapScreamer.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!playedLandSound)
        {
            playedLandSound = true;

            // Trap land sound here
        }
    }
}