using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingEntity
{
    public GameObject exclamationObject;

    public List<DestinationDot> path;
    public DestinationDot currentDotTarget;
    int pathPos = 0;

    void Start()
    {
        currentDotTarget = path[0];
        FindObjectOfType<Controls>().RegisterEnemy(this);
    }


    List<DestinationDot> markedDots = new List<DestinationDot>();
    List<StartAndDestination> queue = new List<StartAndDestination>();

    public void DoStep()
    {
        if (currentDot == currentDotTarget)
        {
            pathPos++;
            currentDotTarget = path[pathPos % path.Count];
        }

        markedDots.Clear();
        queue.Clear();
        foreach (DestinationDot dd in currentDot.destinations)
        {
            if (dd.guardAccessible)
            {
                queue.Add(new StartAndDestination(dd, dd));
            }
        }

        markedDots.Add(currentDot);
        while(queue.Count > 0)
        {
            StartAndDestination sad = queue[0];
            if(sad.destinationDot == currentDotTarget)
            {
                GoToDot(sad.startDot);
                break;
            }
            else
            {
                if (markedDots.Contains(sad.destinationDot))
                {
                    queue.Remove(sad);
                }
                else
                {
                    markedDots.Add(sad.destinationDot);
                    foreach(DestinationDot dd in sad.destinationDot.destinations)
                    {
                        if (dd.guardAccessible)
                        {
                            queue.Add(new StartAndDestination(sad.startDot, dd));
                        }
                    }
                    queue.Remove(sad);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ninja"))
        {
            exclamationObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ninja"))
        {
            exclamationObject.SetActive(false);
        }
    }
}

struct StartAndDestination
{
    public StartAndDestination(DestinationDot _startDot, DestinationDot _destinationDot)
    {
        startDot = _startDot;
        destinationDot = _destinationDot;
    }
    public DestinationDot startDot;
    public DestinationDot destinationDot;
}