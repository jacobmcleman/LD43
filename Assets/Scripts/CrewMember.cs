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

    public float speed = 1;
    public float acceleration = 5;

    public GameObject goreBlotPrefab;

    public int minGores = 2;
    public int maxGores = 10;

    public float goreForceMin = 200;
    public float goreForceMax = 1000;

    protected Rigidbody2D rb2D;
    protected Animator anim;

    protected bool dead;

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

        dead = false;
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
                movement = acceleration * movement.normalized;

                rb2D.AddForce(movement);
            }

            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

            if(Mathf.Abs(rb2D.velocity.x) > speed * Time.deltaTime)
            {
                Debug.Log("Clamping speed. Before: " + rb2D.velocity + ", After: " + new Vector3((rb2D.velocity.x > 0 ? 1 : -1) * speed * Time.deltaTime, rb2D.velocity.y));
                rb2D.velocity = new Vector3((rb2D.velocity.x > 0 ? 1 : -1) * speed * Time.deltaTime, rb2D.velocity.y);
                
            }
        }
        else if(Mathf.Abs(rb2D.velocity.x) < 0.1f)
        {
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        anim.SetFloat("velocity", rb2D.velocity.x);
    }

    private void OnMouseOver()
    {
        if(!dead) hoverEnterFunction();
    }
    private void OnMouseExit()
    {
        if (!dead) hoverExitFunction();
    }

    private void OnMouseDown()
    {
        if (!dead) activateFunction();
    }

    private void ToggleMove()
    {
        if (!dead) moving = !moving;
    }

    public void Die()
    {
        GetComponent<Animator>().SetTrigger("death");
        gameObject.layer = 10;
        
        foreach(Transform child in transform)
        {
            child.gameObject.layer = 10;
        }

        enabled = false;

        dead = true;

        int toSpawn = Random.Range(minGores, maxGores);

        for(int i = 0; i < toSpawn; ++i)
        {
            GameObject gore = Instantiate(goreBlotPrefab);

            gore.transform.position = transform.position;
            gore.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * Random.Range(goreForceMin, goreForceMax));
        }
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
