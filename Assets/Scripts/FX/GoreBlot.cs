using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GoreBlot : MonoBehaviour
{
    public Sprite[] goreBlotSprite;
    public Sprite[] goreSmearSprite;
    public int smearLayer = 9;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = goreBlotSprite[Random.Range(0, goreBlotSprite.Length)];

        rb.freezeRotation = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Vector2.Dot(collision.contacts[0].normal, Vector2.up) > 0.5f)
        {
            TilemapCollider2D col = collision.gameObject.GetComponent<TilemapCollider2D>();

            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.velocity = new Vector2(0, 0);
            GetComponent<Collider2D>().enabled = false;

            transform.parent = collision.transform;

            sr.sprite = goreSmearSprite[Random.Range(0, goreSmearSprite.Length)];
            sr.sortingOrder = smearLayer;
            enabled = false;
        }
    }
}
