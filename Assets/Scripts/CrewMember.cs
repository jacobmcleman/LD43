using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ActivateFunction();

public class CrewMember : MonoBehaviour {
    public bool moving = false;
    public bool moveRight = true;

    protected ActivateFunction activateFunction;

    public float speed = 5;

    protected Rigidbody2D rb2D;
    protected Animator anim;

    public enum Role
    {
        RedShirt,
        Tech,
        Captain
    }

    public Role role;

    private void Start ()
    {
        activateFunction = ToggleMove;

        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update ()
    {
		if(moving)
        {
            Vector3 movement = new Vector2(moveRight ? speed : -speed, 0);

            rb2D.AddForce(movement);

            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else if(rb2D.velocity.x < 0.1f)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        anim.SetFloat("velocity", rb2D.velocity.x);
    }

    private void OnMouseDown()
    {
        activateFunction();
    }

    private void ToggleMove()
    {
        moving = !moving;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("bump " + rb2D.velocity + ", " + collision.contacts[0].normal);
        if (Mathf.Abs(Vector2.Dot(collision.contacts[0].normal, new Vector2(0, 1))) < 0.2f)
        {
            moveRight = !moveRight;
        }
    }
}
