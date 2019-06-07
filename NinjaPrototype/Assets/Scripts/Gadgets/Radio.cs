using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Radio : Gadget
{
    public GameObject soundSprite;
    public float alertRadius;
    public int playTimeLeft;

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
        playTimeLeft = 0;
        soundSprite.SetActive(true);
        AlertEnemies(alertRadius);
    }

    public override void TurnOff()
    {

    }

    public new void DoStep()
    {
        base.DoStep();
        if(playTimeLeft == 0)
        {
            TurnOff();
        }
        playTimeLeft--;
    }

    public override bool OnClick()
    {
        return false;
    }
}
