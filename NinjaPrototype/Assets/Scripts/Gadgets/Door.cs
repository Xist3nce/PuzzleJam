using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Gadget
{
    public int doorOpenWaitTime;

    public TextMesh counterText;

    public AudioSource doorButtonSound;
    public AudioSource doorOpenSound;
    public AudioSource doorCloseSound;

    public GameObject upperDoorObject;
    public GameObject lowerDoorObject;

    DoorState state = DoorState.closed;

    bool gadgetInCountdown = false;
    int remainingDuration;

    public override void Start()
    {
        base.Start();
        counterText.text = doorOpenWaitTime.ToString();
    }

    public override void TurnOn()
    {
        doorOpenSound.Play();
        state = DoorState.opening;
    }

    public override void TurnOff()
    {
        doorCloseSound.Play();
        state = DoorState.closing;
    }

    public override bool IsReady()
    {
        if(state == DoorState.open || state == DoorState.closed)
        {
            return true;
        }
        return false;
    }

    public override bool OnClick()
    {
        if (PlayerIsInRange() && state == DoorState.closed)
        {
            remainingDuration = doorOpenWaitTime;
            gadgetInCountdown = true;
            doorButtonSound.Play();
            return true;
        }
        return false;
    }

    float oldLerpVal = 0.0f;
    void Update()
    {
        if(state == DoorState.opening)
        {
            if (doorOpenSound.isPlaying)
            {
                float lerpVal = doorOpenSound.time / doorOpenSound.clip.length;
                if(lerpVal > oldLerpVal)
                {
                    Vector3 off = Vector3.Lerp(new Vector3(0, 0.5f, 0), new Vector3(0, 1.5f, 0), lerpVal);
                    upperDoorObject.transform.localPosition = off;
                    lowerDoorObject.transform.localPosition = -off;
                    oldLerpVal = lerpVal;
                }
            }
            else
            {
                Vector3 off = new Vector3(0, 1.5f, 0);
                upperDoorObject.transform.localPosition = off;
                lowerDoorObject.transform.localPosition = -off;
                state = DoorState.open;
                DestinationDot[] allDots = FindObjectsOfType<DestinationDot>();
                foreach (DestinationDot dd in allDots)
                {
                    dd.UpdateConnection(Time.time);
                }
            }
        }
    }

    public override void DoStep()
    {
        if (gadgetInCountdown)
        {
            counterText.text = remainingDuration.ToString();
            if (remainingDuration == 0)
            {
                TurnOn();
                gadgetInCountdown = false;
            }
            remainingDuration--;
        }
    }
}

enum DoorState
{
    opening,
    closing,
    open,
    closed
}