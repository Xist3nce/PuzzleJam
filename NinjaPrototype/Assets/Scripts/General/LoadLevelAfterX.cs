using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelAfterX : MonoBehaviour
{
    public string level;
    public float delay;

    float startTime;

    void Start()
    {
        startTime = Time.time + delay;
    }

    void Update()
    {
        if (Time.time > startTime)
        {
            SceneManager.LoadScene(level);
        }
    }
}
