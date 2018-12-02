using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ActivateFunction();

public class CrewMember : MonoBehaviour {
    public bool moving = false;
    public bool moveRight = true;

    public ActivateFunction activateFunction;
    public ActivateFunction hoverEnterFunction;
    public ActivateFunction hoverExitFunction;

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
        hoverEnterFunction = GetComponent<SpriteOutline>().HighlightOn;
        hoverExitFunction = GetComponent<SpriteOutline>().HighlightOff;

        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update ()
    {
		if(moving)
        {
            Vector3 movement = new Vector2(moveRight ? 1 : -1, 0);

            // rotate movement vector to be parallel to the surface the character is walking on
            RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, transform.localScale.y, ~((1 << 10) | (1 << 9)));

            if(groundHit)
            {
                //Project the movement onto a vector parallel to the surface
                movement = Vector3.Project(movement, new Vector2(groundHit.normal.y, -groundHit.normal.x));
                movement = speed * movement.normalized;

                rb2D.AddForce(movement);
            }

            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else if(rb2D.velocity.x < 0.1f)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        anim.SetFloat("velocity", rb2D.velocity.x);
    }

    private void OnMouseOver()
    {
        hoverEnterFunction();
    }
    private void OnMouseExit()
    {
        hoverExitFunction();
    }

    private void OnMouseDown()
    {
        activateFunction();
    }

    private void ToggleMove()
    {
        moving = !moving;
    }

    public void Die()
    {
        GetComponent<Animator>().SetTrigger("death");
        gameObject.layer = 10;
        enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        // rotate movement vector to be parallel to the surface the character is walking on
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, transform.localScale.y, ~((1 << 10) | (1 << 9)));

        if (groundHit)
        {
            Vector3 movement = new Vector2(moveRight ? 1 : -1, 0);

            //Project the movement onto a vector parallel to the surface
            movement = Vector3.Project(movement, new Vector2(groundHit.normal.y, -groundHit.normal.x));
            movement = speed * movement.normalized;

            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, movement);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (moving)
        {
            for (int i = 0; i < collision.contactCount; ++i)
            {
                float slope = Mathf.Abs(Vector2.Dot(collision.contacts[i].normal, new Vector2(0, 1)));

                float direction = Vector2.Dot(collision.contacts[i].normal, new Vector2(moveRight ? 1 : -1, 0));

                //Debug.Log("bump " + collision.contacts[i].normal + ", slope is " + slope + ", direction is " + direction);

                if (slope < 0.2f && direction < 0)
                {
                    //Debug.Log("turning");
                    moveRight = !moveRight;
                    //Want to return here to avoid double-flipping which would be boring
                    return;
                }
            }
        }
    }
}
