using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSave : MonoBehaviour
{
    Camera cam;
    public RenderTexture rt;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void OnPostRender()
    {
        rt = cam.activeTexture;
        if(rt)
        {
            Debug.Log("hasone");
        }
    }

    void Update()
    {
        
    }
}
