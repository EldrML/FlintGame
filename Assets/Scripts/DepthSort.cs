using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthSort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public bool dynamic = false;

    // Use this for initialization
    void Start()
    {
        this.spriteRenderer = this.GetComponentInParent<SpriteRenderer>();
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * 20f) * -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.dynamic)
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * 20f) * -1;
        }
    }
}
