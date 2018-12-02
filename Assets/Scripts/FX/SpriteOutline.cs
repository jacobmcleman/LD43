using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour {
    public Color color = Color.white;

    private SpriteRenderer spriteRenderer;

    [Range (0, 16)]
    public int outlineWidth = 1;

    public bool drawOutline = false;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

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

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        spriteRenderer.GetPropertyBlock(mpb);

        mpb.SetFloat("_Outline", outline ? 1.0f : 0.0f);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineWidth);

        spriteRenderer.SetPropertyBlock(mpb);
    }
}
