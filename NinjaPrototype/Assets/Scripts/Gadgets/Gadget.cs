using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Gadget : Hoverable
{
    public GameObject gadgetButtonPrefab;
    public GameObject keepHoverPrefab;
    public GameObject dotIndicatorPreset;
    public float playerActivateRadius = 2.0f;

    public int[] possibleTimerDurations = {0};
    public List<GadgetButton> activeButtonList = new List<GadgetButton>();

    public abstract void TurnOn();

    public abstract void TurnOff();

    private void Start()
    {
        FindObjectOfType<Controls>().RegisterGadget(this);
    }

    public void AlertEnemies(float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemies"));
        foreach(Collider2D c2D in colliders)
        {
            if (!c2D.isTrigger)
            {
                Enemy enemy = c2D.GetComponentInParent<Enemy>();
                enemy.OnAlert(transform.position, GetNearDot(), this);
            }
        }
    }

    public DestinationDot GetNearDot()
    {
        Collider2D c2D = Physics2D.OverlapCircle(transform.position, playerActivateRadius, LayerMask.GetMask("Dots"));
        if (c2D)
        {
            return c2D.GetComponent<DestinationDot>();
        }
        return null;
    }

    public bool PlayerIsInRange()
    {
        Collider2D c2D = Physics2D.OverlapCircle(transform.position, playerActivateRadius, LayerMask.GetMask("Ninja"));
        if (c2D)
        {
            return true;
        }
        return false;
    }

    List<GameObject> currentIndicators = new List<GameObject>();
    public override void SetFocus(bool isInFocus)
    {
        if (isInFocus)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, playerActivateRadius, LayerMask.GetMask("Dots"));
            foreach (Collider2D c2D in colliders)
            {
                currentIndicators.Add(Instantiate(dotIndicatorPreset, c2D.transform.position, Quaternion.identity));
            }
        }
        else
        {
            foreach (GameObject g in currentIndicators)
            {
                Destroy(g);
            }
        }
    }

    public abstract void DoStep();

    #if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.forward, playerActivateRadius);
    }
    #endif
}
