using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y,0) ; // This just makes the object follow the mouse but retains Z as 0.
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Activatable") //TODO: This is gonna be used to display hover tool tips
        {
            Debug.Log(col.name + " has been hovered");
        }
    }
}
