using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetButton : Hoverable
{
    public Sprite outOfFocusSprite;
    public Sprite inFocusSprite;
    public TextMesh textMesh;

    GadgetButtonCallback gadgetButtonCallback;
    int duration;

    public override void SetFocus(bool isInFocus)
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

    public override bool OnClick()
    {
        gadgetButtonCallback(duration);
        return true;
    }

    public void DestroyButton()
    {
        Destroy(gameObject);
    }

    public void SetFunctionToRun(GadgetButtonCallback _gadgetButtonCallback, int _duration)
    {
        gadgetButtonCallback = _gadgetButtonCallback;
        textMesh.text = _duration.ToString();
        duration = _duration;
    }

    public delegate void GadgetButtonCallback(int duration);
}
