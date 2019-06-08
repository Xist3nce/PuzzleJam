using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Radio : Gadget
{
    public GameObject soundSprite;
    public float alertRadius;
    public bool alerting = false;

    #if UNITY_EDITOR
    public new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.forward, alertRadius);
    }
    #endif

    public override void TurnOn()
    {
        soundSprite.SetActive(true);
        alerting = true;
    }

    public override void TurnOff()
    {
        soundSprite.SetActive(false);
        alerting = false;
    }

    public override void DoStep()
    {
        base.DoStep();

        if (alerting)
        {
            AlertEnemies(alertRadius);
        }
    }

    public override bool OnClick()
    {
        return false;
    }
}
