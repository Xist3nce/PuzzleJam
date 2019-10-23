using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintFixedTime : MonoBehaviour
{
    int i = 1;
    bool pFlip = false;

    System.Diagnostics.Stopwatch stopwatch;

    void Start()
    {
        stopwatch = new System.Diagnostics.Stopwatch();
    }

    void FixedUpdate()
    {
        stopwatch.Stop();
        //Debug.Log("b");
        if (pFlip)
        {
            //Debug.Log("spr: " + stopwatch.Elapsed);
        }
        else
        {
            //Debug.Log("rot: " + stopwatch.Elapsed);
        }
        if(i % 300 == 0)
        {
            Debug.Break();
        }
        i++;
        pFlip = !pFlip;
        stopwatch.Reset();
        stopwatch.Start();
    }
}
