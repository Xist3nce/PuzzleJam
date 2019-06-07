using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationDot : MonoBehaviour
{
    public Sprite outOfFocusSprite;
    public Sprite inFocusSprite;

    public List<DestinationDot> destinations;

    void OnDrawGizmosSelected()
    {
        foreach(DestinationDot d in destinations)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, d.transform.position);
        }
    }

    public void SetFocus(bool isInFocus)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (isInFocus)
        {
            sr.sprite = inFocusSprite;
        }
        else
        {
            sr.sprite = outOfFocusSprite;
        }
    }
}

