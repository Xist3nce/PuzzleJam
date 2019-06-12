using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnAudioFinish : MonoBehaviour
{
    AudioSource ac;
    void Start()
    {
        ac = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!ac.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
