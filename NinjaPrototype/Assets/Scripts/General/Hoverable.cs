using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hoverable : MonoBehaviour
{
    public abstract void SetFocus(bool isInFocus);

    public abstract bool OnClick();
}
