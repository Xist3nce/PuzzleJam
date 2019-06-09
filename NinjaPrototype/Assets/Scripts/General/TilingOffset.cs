using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilingOffset : MonoBehaviour
{
    public float speed = 1.0f;

    Renderer r;
    float offset = 0.0f;

    void Start()
    {
        r = GetComponent<Renderer>();
    }

    void Update()
    {
        offset += Time.deltaTime * speed;
        r.material.mainTextureOffset = Vector2.down * offset;
    }
}
