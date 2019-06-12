using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingEntity
{
    public override void SetFocus(bool isInFocus)
    {
        
    }

    public override bool OnClick()
    {
        return false;
    }

    public override bool GoToDot(DestinationDot d)
    {
        if (d)
        {
            if (!string.IsNullOrEmpty(d.levelToLoad))
            {
                FindObjectOfType<Controls>().WinLevel(d.levelToLoad, true);
            }
        }
        return base.GoToDot(d);
    }
}
