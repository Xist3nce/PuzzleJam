using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingEntity
{
    public GameObject dotIndicatorPreset;

    public GameObject exclamationObject;
    public GameObject confusedObject;

    public List<DestinationDot> path;
    public int standTime = 2;
    DestinationDot currentDotTarget;
    Gadget currentGadgetTarget;
    int stepsLeftTillStateChange;
    int pathPos = 0;

    State state = State.Patrolling;

    public override void Start()
    {
        base.Start();
        currentDotTarget = path[0];
        FindObjectOfType<Controls>().RegisterEnemy(this);
    }


    List<DestinationDot> markedDots = new List<DestinationDot>();
    List<StartAndDestination> queue = new List<StartAndDestination>();

    public void DoStep()
    {
        if (stepsLeftTillStateChange <= 0)
        {
            if(state == State.Standing)
            {
                ChangeState(State.Patrolling);
            }
        }

        if (currentDot == currentDotTarget || currentDotTarget == null)
        {
            if(state == State.Investigating)
            {
                if (currentGadgetTarget)
                {
                    currentGadgetTarget.TurnOff();
                    currentGadgetTarget = null;
                }
                ChangeState(State.Standing);
            }
            if(state == State.Patrolling)
            {
                pathPos++;
                currentDotTarget = path[pathPos % path.Count];
            }
        }

        if(state != State.Standing)
        {
            DestinationDot nextPosDot = FindAccessiblePathDot(currentDotTarget);
            if (nextPosDot)
            {
                GoToDot(nextPosDot);
            }
        }

        if(state == State.Standing)
        {
            FlipLookDirection();
        }

        stepsLeftTillStateChange--;
    }

    DestinationDot FindAccessiblePathDot(DestinationDot targetDot)
    {
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
        while (queue.Count > 0)
        {
            StartAndDestination sad = queue[0];
            if (sad.destinationDot == targetDot)
            {
                return sad.startDot;
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
                    foreach (DestinationDot dd in sad.destinationDot.destinations)
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
        return null;
    }

    List<GameObject> currentIndicators = new List<GameObject>();
    public override void SetFocus(bool isInFocus)
    {
        if (isInFocus)
        {
            /*DestinationDot nextPosDot = FindAccessiblePathDot(currentDotTarget);
            if (nextPosDot)
            {
                currentIndicators.Add(Instantiate(dotIndicatorPreset, nextPosDot.transform.position, Quaternion.identity));
            }*/
            foreach(DestinationDot dd in path)
            {
                currentIndicators.Add(Instantiate(dotIndicatorPreset, dd.transform.position, Quaternion.identity));
            }
        }
        else
        {
            foreach(GameObject g in currentIndicators)
            {
                Destroy(g);
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

    public void OnAlert(Vector2 pos, DestinationDot dot, Gadget g)
    {
        currentDotTarget = dot;
        currentGadgetTarget = g;
        ChangeState(State.Investigating);
    }

    void ChangeState(State newState)
    {
        if(state != State.Investigating && newState == State.Investigating)
        {
            confusedObject.SetActive(true);
        }

        if (state == State.Investigating && newState != State.Investigating)
        {
            confusedObject.SetActive(false);
        }

        if(newState == State.Standing)
        {
            stepsLeftTillStateChange = standTime;
        }

        state = newState;
    }

    public override bool OnClick()
    {
        return false;
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

enum State
{
    Standing,
    Patrolling,
    Investigating,
    Hunting
}