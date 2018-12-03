using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ActivateFunction();

public class CrewMember : MonoBehaviour {
    public bool moving = false;
    public bool moveRight = true;
    public bool glassed = false;

    public bool canActivate = true;

    public ActivateFunction activateFunction;
    public ActivateFunction hoverEnterFunction;
    public ActivateFunction hoverExitFunction;

    public float speed = 1;
    public float acceleration = 5;

    private Vector2 desiredMove = Vector2.zero;
    public Vector2 Movement
    {
        get { return desiredMove; }
    }
    public float moveMod = 0;

    public GameObject goreBlotPrefab;

    private GameObject arrow;

    public int minGores = 2;
    public int maxGores = 10;

    public float goreForceMin = 200;
    public float goreForceMax = 1000;

    protected Rigidbody2D rb2D;
    protected Animator anim;

    protected bool dead;
    public bool Dead
    {
        get { return dead; }
    }

    AudioSource crewScreamer;
    public AudioClip mouseOver;
    public AudioClip mouseOut;
    public AudioClip engage;
    public AudioClip deathSound;
    public AudioClip glassSound;
    private bool playMouseInSound;
    private bool playMouseOutSound;

    public GameObject loseScreen;

    public enum Role
    {
        RedShirt,
        Tech,
        Captain
    }

    public Role role;

    private void Start ()
    {
        if (canActivate)
        {
            activateFunction = ToggleMove;
            hoverEnterFunction = HoverOn;
            hoverExitFunction = HoverOff;
        }

        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        arrow = transform.Find("Arrow").gameObject;

        arrow.GetComponent<SpriteRenderer>().enabled = false;

        dead = false;

        crewScreamer = GetComponent<AudioSource>();
        playMouseInSound = true;
        playMouseOutSound = false;
        
    }

    private void Update ()
    {
        if (moving || moveMod != 0)
        {
            //crewScreamer.Play();
            Vector2 movement = new Vector2(moveRight ? 1 : -1, 0);
            if (!moving) movement = Vector2.zero;
            desiredMove = movement;
            float topSpeedModifier = (moveMod * speed) + (desiredMove.x * speed);
            movement.x += moveMod;
            
            // rotate movement vector to be parallel to the surface the character is walking on
            RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, transform.localScale.y, ~((1 << 10) | (1 << 9)));

            if (groundHit)
            {
                //Project the movement onto a vector parallel to the surface
                movement = Vector3.Project(movement, new Vector2(groundHit.normal.y, -groundHit.normal.x));
                movement = Mathf.Abs((moveMod * acceleration) + (desiredMove.x * acceleration)) * movement.normalized;
                
            }

            

            rb2D.AddForce(movement);

            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (Mathf.Abs(rb2D.velocity.x) > (speed + topSpeedModifier) * Time.deltaTime)
            {
                if (rb2D.velocity.y > 0.1f)
                {
                    //Debug.Log("Clamping speed. Before: " + rb2D.velocity + ", After: " + speed * rb2D.velocity.normalized);
                    rb2D.velocity = speed * Time.deltaTime * rb2D.velocity.normalized;
                }
                else
                {
                    //Debug.Log("Clamping speed. Before: " + rb2D.velocity + ", After: " + new Vector3((rb2D.velocity.x > 0 ? 1 : -1) * speed * Time.deltaTime, rb2D.velocity.y));
                    rb2D.velocity = new Vector3((rb2D.velocity.x > 0 ? 1 : -1) * speed * Time.deltaTime, rb2D.velocity.y);
                }
            }

            anim.SetFloat("velocity", desiredMove.x);
        }
        else
        {
            desiredMove = Vector2.zero;
            if (Mathf.Abs(rb2D.velocity.x) < 0.1f)
            {
                rb2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }

        
        anim.SetFloat("velocity", desiredMove.x);
    }

    private void PlayGlassSound ()
    {
        crewScreamer.PlayOneShot(glassSound);
    }

    private void OnMouseOver()
    {
        if (!dead && !glassed && canActivate && hoverEnterFunction != null) hoverEnterFunction();
    }
    private void OnMouseExit()
    {
        if (!dead && !glassed && canActivate && hoverExitFunction != null) hoverExitFunction();
    }

    private void OnMouseDown()
    {
        if (!dead && !glassed && canActivate && activateFunction != null) activateFunction();
        if (!dead && glassed && canActivate) PlayGlassSound();
    }

    private void ToggleMove()
    {
        if (!dead && !glassed)
        {
            moving = !moving;
            rb2D.velocity = new Vector2(moveMod, rb2D.velocity.y);
            if(moving) crewScreamer.PlayOneShot(engage, 1.0F);
            if(!moving) crewScreamer.PlayOneShot(mouseOut, 1.0F);
            
        } 
    }

    private void HoverOn()
    {
        GetComponent<SpriteOutline>().HighlightOn();
        if(!dead && playMouseInSound)
        {
            crewScreamer.PlayOneShot(mouseOver, 1.0F);
            playMouseInSound = false;
        } 
        
        if (!moving)
        {
            arrow.GetComponent<SpriteRenderer>().enabled = true;
            arrow.GetComponent<SpriteRenderer>().color = GetComponent<SpriteOutline>().color;

            arrow.transform.localScale = new Vector3(moveRight ? 1 : -1, 1, 1);
        }
    }

    private void HoverOff()
    {
        GetComponent<SpriteOutline>().HighlightOff();
        playMouseInSound = true;
        arrow.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Die()
    {
        GetComponent<Animator>().SetTrigger("death");
        gameObject.layer = 10;

        HoverOff();

        foreach (Transform child in transform)
        {
            child.gameObject.layer = 10;
        }

        enabled = false;

        dead = true;

        crewScreamer.PlayOneShot(deathSound);

        int toSpawn = Random.Range(minGores, maxGores);

        for(int i = 0; i < toSpawn; ++i)
        {
            GameObject gore = Instantiate(goreBlotPrefab);

            gore.transform.position = transform.position;
            gore.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * Random.Range(goreForceMin, goreForceMax));
        }

        if(role == Role.Captain)
        {
            if(loseScreen)
            {
                loseScreen.SetActive(true);
            }
            else
            {
                Debug.LogError("AAAA NO LOSE SCREEN ON THIS LEVELLL FIXXX PLSSSS");
            }
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
