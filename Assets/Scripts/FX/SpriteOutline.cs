using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour {
    public Color color = Color.white;

    private SpriteRenderer spriteRenderer;
    private TilemapRenderer tileRenderer;

    [Range (0, 16)]
    public int outlineWidth = 1;

    public bool drawOutline = false;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tileRenderer = GetComponent<TilemapRenderer>();

        UpdateOutline(drawOutline);
    }

    private void OnDisable()
    {
        UpdateOutline(false);
    }

    private void Update()
    {
        UpdateOutline(drawOutline);
    }

    public void HighlightOn()
    {
        drawOutline = true;
    }

    public void HighlightOff()
    {
        drawOutline = false;
    }

    private void ApplyTo(Renderer r, bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        r.GetPropertyBlock(mpb);

        mpb.SetFloat("_Outline", outline ? 1.0f : 0.0f);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineWidth);

        r.SetPropertyBlock(mpb);
    }

    void UpdateOutline(bool outline)
    {
        if (spriteRenderer) ApplyTo(spriteRenderer, outline);
        if (tileRenderer) ApplyTo(tileRenderer, outline);
    }
}
