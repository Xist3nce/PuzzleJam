using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingEntity : Hoverable
{
    public float moveTime = 0.4f;

    public AudioSource audioSource;
    public DestinationDot currentDot;

    float moveStartTime;
    Vector3 moveStartPos;
    bool isMoving = false;

    public virtual void Start()
    {
        DestinationDot[] allDots = FindObjectsOfType<DestinationDot>();
        float currentDist = float.MaxValue;

        foreach(DestinationDot dd in allDots)
        {
            float distance = Vector3.Distance(dd.transform.position, transform.position);
            if(distance < currentDist)
            {
                currentDist = distance;
                currentDot = dd;
            }
        }

        transform.position = currentDot.transform.position;
    }

    public virtual void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(moveStartPos, currentDot.transform.position, (Time.time - moveStartTime) / moveTime);
            if(Time.time - moveStartTime > moveTime)
            {
                if (audioSource.isPlaying)
                {
                    //audioSource.Stop();
                }
                isMoving = false;
            }
        }
    }

    public bool GoToDot(DestinationDot d)
    {
        if(currentDot != null)
        {
            if (!currentDot.destinations.Contains(d))
            {
                return false;
            }
        }
        audioSource.Play();
        currentDot = d;
        moveStartTime = Time.time;
        moveStartPos = transform.position;
        isMoving = true;

        //flipping
        if((moveStartPos - currentDot.transform.position).x > 0)
        {
            LookLeft();
        }
        else
        {
            LookRight();
        }

        return true;
    }

    public void LookLeft()
    {
        transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    }

    public void LookRight()
    {
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    public void FlipLookDirection()
    {
        transform.rotation *= Quaternion.Euler(0.0f, 180.0f, 0.0f);
    }

    public virtual bool IsReady()
    {
        return !isMoving;
    }
}
