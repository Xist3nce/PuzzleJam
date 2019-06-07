using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingEntity : Hoverable
{
    public float moveTime = 0.4f;

    public DestinationDot currentDot;

    float moveStartTime;
    Vector3 moveStartPos;
    bool isMoving = false;

    public void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(moveStartPos, currentDot.transform.position, (Time.time - moveStartTime) / moveTime);
            if(Time.time - moveStartTime > moveTime)
            {
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
        currentDot = d;
        moveStartTime = Time.time;
        moveStartPos = transform.position;
        isMoving = true;

        //flipping
        if((moveStartPos - currentDot.transform.position).x > 0)
        {
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }

        return true;
    }

    public bool IsReady()
    {
        return !isMoving;
    }
}
