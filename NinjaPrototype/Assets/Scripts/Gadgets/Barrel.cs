using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Barrel : Gadget
{
    public GameObject rangeIndicatorPrefab;
    public GameObject rollingBarrelPrefab;

    public float alertRadius;

    GameObject rangeIndicator;

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
        GameObject rollingBarrelObject = Instantiate(rollingBarrelPrefab, transform.position, Quaternion.identity);
        rollingBarrelObject.GetComponent<RollingBarrel>().alertRadius = alertRadius;
        DestroyThisGadget();
    }

    public override void TurnOff()
    {

    }

    public override bool IsReady()
    {
        return true;
    }

    public override void SetFocus(bool isInFocus)
    {
        base.SetFocus(isInFocus);

        if (isInFocus)
        {
            rangeIndicator = Instantiate(rangeIndicatorPrefab, transform.position - Vector3.forward * 5.0f, Quaternion.identity);
            rangeIndicator.transform.localScale = Vector3.one * alertRadius * 2.0f;
        }
        else
        {
            if (rangeIndicator)
            {
                Destroy(rangeIndicator);
            }
        }
    }

    public override bool OnClick()
    {
        if (PlayerIsInRange())
        {
            TurnOn();
            return true;
        }
        return false;
    }

    public override void DoStep()
    {

    }
}
