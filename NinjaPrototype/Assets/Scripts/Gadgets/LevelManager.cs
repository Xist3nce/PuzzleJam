using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string levelText;

    void Start()
    {
        CameraMouseControl cmc = FindObjectOfType<CameraMouseControl>();
        Rect r = new Rect((Vector2)transform.position - GetComponent<BoxCollider2D>().size * 0.5f, GetComponent<BoxCollider2D>().size);
        cmc.SetCameraBounds(r);
        cmc.DisplayLevelText(levelText);
    }

    void Update()
    {
        
    }
}
