using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseHandler : MonoBehaviour
{
    public bool DisplayingTip;
    public GameObject HoveredObject;
    public Text MouseText;
    [Space]
    public ToolTip Tip;
    public string TipText;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y,0) ; // This just makes the object follow the mouse but retains Z as 0.
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Activatable") //TODO: This is gonna be used to display hover tool tips
        {
            HoveredObject = col.gameObject;
            Debug.Log(col.name + " has been hovered");
        }
        if (col.GetComponent<ToolTip>())
        {
            Debug.Log("Hovered object has Tooltip");
            Tip = col.GetComponent<ToolTip>();
            TipText = Tip.Hovertip;
            DisplayTip();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Activatable")
        {
            HoveredObject = null;
            
        }
        if (col.GetComponent<ToolTip>())
        {
            Tip = null;
            TipText = null;
            UnDisplayTip();
        }


    }


    public void DisplayTip()
    {
        MouseText.enabled = true;
        MouseText.text = TipText;
    }
    public void UnDisplayTip()
    {
        MouseText.enabled = false;
        MouseText.text = null;
    }
}
