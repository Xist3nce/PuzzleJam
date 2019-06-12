using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Radio : Gadget
{
    public AudioSource clickSoundSource;
    public AudioSource AnnoySoundSource;

    public GameObject gadgetButtonPrefab;
    public GameObject rangeIndicatorPrefab;
    public GameObject keepHoverPrefab;
    public GameObject soundSprite;

    public TextMesh countdown;

    public int[] possibleTimerDurations = { 0 };
    public List<GadgetButton> activeButtonList = new List<GadgetButton>();

    public float alertRadius;
    public bool alerting = false;

    bool gadgetInCountdown = false;
    int remainingDuration;
    GameObject keepHoverObject;
    GameObject rangeIndicator;

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

        if (!gadgetInCountdown && isInFocus && !alerting && PlayerIsInRange())
        {
            ShowButtons();
        }
        else
        {
            HideButtons();
        }
    }

    public void ButtonClickReceive(int duration)
    {
        remainingDuration = duration;
        gadgetInCountdown = true;
        clickSoundSource.Play();
        HideButtons();
    }

    public override void TurnOn()
    {
        AnnoySoundSource.Play();
        soundSprite.SetActive(true);
        alerting = true;
    }

    public override void TurnOff()
    {
        AnnoySoundSource.Stop();
        soundSprite.SetActive(false);
        alerting = false;
    }

    public override bool IsReady()
    {
        return true;
    }

    public override void DoStep()
    {
        if (gadgetInCountdown)
        {
            countdown.text = remainingDuration.ToString();
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
        if (PlayerIsInRange() && !alerting)
        {
            HideButtons();
            TurnOn();
            return true;
        }
        return false;
    }
}
