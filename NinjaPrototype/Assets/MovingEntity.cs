using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity : MonoBehaviour
{
    public float moveTime = 0.4f;

    DestinationDot targetDot;

    float moveStartTime;
    Vector3 moveStartPos;
    bool isMoving = false;

    public void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(moveStartPos, targetDot.transform.position, (Time.time - moveStartTime) / moveTime);
            if(Time.time - moveStartTime > moveTime)
            {
                isMoving = false;
            }
        }
    }

    public bool GoToDot(DestinationDot d)
    {
        if(targetDot != null)
        {
            if (!targetDot.destinations.Contains(d))
            {
                return false;
            }
        }
        targetDot = d;
        moveStartTime = Time.time;
        moveStartPos = transform.position;
        isMoving = true;
        return true;
    }

    public bool IsReady()
    {
        return !isMoving;
    }
}
