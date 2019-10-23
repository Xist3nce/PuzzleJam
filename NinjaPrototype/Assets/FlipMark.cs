using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipMark : MonoBehaviour
{
    bool pFlip = false;

    bool flipSprite = true;
    bool flipSpriteTrue = true;
    bool flipSpriteFalse = false;

    bool flipObject = true;
    Quaternion nonFlip = Quaternion.identity;
    Quaternion doFlip = Quaternion.Euler(0, 180, 0);

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (pFlip)
        {
            if (flipSprite)
            {
                //spriteRenderer.flipX = flipSpriteTrue;
            }
            else
            {
                //spriteRenderer.flipX = flipSpriteFalse;
            }
            //Debug.Log("sprite");
            flipSprite = !flipSprite;
        }
        else
        {
            if (flipObject)
            {
                transform.localRotation = nonFlip;
            }
            else
            {
                transform.localRotation = doFlip;
            }
            //Debug.Log("rot");
            flipObject = !flipObject;
        }
        pFlip = !pFlip;
    }
}
