using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Radio : Gadget
{
    public GameObject soundSprite;
    public float alertRadius;
    public bool alerting = false;

    bool gadgetInCountdown = false;
    int remainingDuration;
    GameObject keepHoverObject;

    #if UNITY_EDITOR
    public new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.forward, alertRadius);
    }
    #endif

    void ShowButtons()
    {
        keepHoverObject = Instantiate(keepHoverPrefab, transform.position, Quaternion.identity, transform);
        int i = 0;
        float spaceBetweenButtons = 1.0f;
        float buttonOffset = ((float)possibleTimerDurations.Length - 1.0f) * 0.5f * spaceBetweenButtons;
        foreach (int duration in possibleTimerDurations)
        {
            GameObject buttonObject = Instantiate(gadgetButtonPrefab);
            buttonObject.transform.position = transform.position + Vector3.up + Vector3.right * i * spaceBetweenButtons + Vector3.left * buttonOffset;
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

    public override void SetFocus(bool isInFocus){
        base.SetFocus(isInFocus);
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

    public void ButtonClickReceive(int duration)
    {
        remainingDuration = duration;
        gadgetInCountdown = true;
        HideButtons();
    }

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
        if (gadgetInCountdown)
        {
            if (remainingDuration == 0)
            {
                TurnOn();
                gadgetInCountdown = false;
            }
            remainingDuration--;
        }

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
