using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingEntity
{
    public List<DestinationDot> path;
    int pathPos = -1;

    void Start()
    {
        FindObjectOfType<Controls>().RegisterEnemy(this);
    }

    public void DoStep()
    {
        pathPos++;
        Debug.Log(path[pathPos % path.Count]);
        GoToDot(path[pathPos%path.Count]);
    }
}
