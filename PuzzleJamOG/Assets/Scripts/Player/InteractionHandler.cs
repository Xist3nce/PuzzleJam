using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    //TODO: add a gamemanager singleton to hold keylock info.

    private void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log("colliding with " + col.name);
        if ((col.tag == "Activatable") && (Input.GetButton("Activate")))   //This just checks if the trigger attached to the player and the player is hitting activate
        {
            col.gameObject.SendMessage("Activate");
            Debug.Log("Sending Activate message to " + col.name);
        }
    }
}
