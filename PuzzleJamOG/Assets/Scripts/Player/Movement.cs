using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float WalkSpeed;



    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.LockKeys)
        {
            if (((Input.GetAxis("Horizontal")) > 0) || ((Input.GetAxis("Horizontal")) < 0))
            {
                gameObject.transform.Translate(transform.right * WalkSpeed * Input.GetAxis("Horizontal"));
            }
            if (((Input.GetAxis("Vertical")) > 0) || ((Input.GetAxis("Vertical")) < 0))
            {
                gameObject.transform.Translate(transform.up * WalkSpeed * Input.GetAxis("Vertical"));
            }
        }
    }
}
