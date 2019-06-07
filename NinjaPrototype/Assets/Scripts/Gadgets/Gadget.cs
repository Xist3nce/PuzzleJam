using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public abstract class Gadget : Hoverable
{
    public GameObject gadgetButtonPrefab;
    public GameObject keepHoverPrefab;
    public float playerActivateRadius = 2.0f;

    public int[] possibleTimerDurations = {0};
    public List<GadgetButton> activeButtonList = new List<GadgetButton>();
    bool gadgetInCountdown = false;
    int remainingDuration;
    GameObject keepHoverObject;

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
                enemy.OnAlert(transform.position, GetNearDot());
            }
        }
    }

    public override void SetFocus(bool isInFocus)
    {
        if (!gadgetInCountdown)
        {
            if (isInFocus)
            {
                Collider2D c2D = Physics2D.OverlapCircle(transform.position, playerActivateRadius, LayerMask.GetMask("Ninja"));
                if (c2D)
                {
                    ShowButtons();
                }
            }
            else
            {
                HideButtons();
            }
        }
    }

    void ShowButtons()
    {
        keepHoverObject = Instantiate(keepHoverPrefab, transform.position, Quaternion.identity, transform);
        int i = 0;
        foreach (int duration in possibleTimerDurations)
        {
            GameObject buttonObject = Instantiate(gadgetButtonPrefab);
            buttonObject.transform.position = transform.position + Vector3.up + Vector3.right * i;
            GadgetButton gadgetButton = buttonObject.GetComponent<GadgetButton>();
            gadgetButton.SetFunctionToRun(ButtonClickReceive, possibleTimerDurations[i]);
            activeButtonList.Add(gadgetButton);
            i++;
        }
    }

    void HideButtons()
    {
        if (keepHoverObject)
        {
            Destroy(keepHoverObject);
        }
        while (activeButtonList.Count > 0)
        {
            GadgetButton gadgetButton = activeButtonList.First();
            activeButtonList.Remove(gadgetButton);
            gadgetButton.DestroyButton();
        }
    }

    public void ButtonClickReceive(int duration)
    {
        remainingDuration = duration;
        gadgetInCountdown = true;
        HideButtons();
    }

    public void DoStep()
    {
        if (gadgetInCountdown)
        {
            if (remainingDuration == 0)
            {
                TurnOn();
                gadgetInCountdown = false;
            }
            remainingDuration--;
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

    #if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.forward, playerActivateRadius);
    }
    #endif
}
