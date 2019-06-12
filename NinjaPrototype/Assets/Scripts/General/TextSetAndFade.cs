using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSetAndFade : MonoBehaviour
{
    public float fadeDelayTime;
    public float fadeTransitionTime;
    TextMesh tm;

    float timeTillFade;
    float timeTillInvisible;

    void Start()
    {
        tm = GetComponent<TextMesh>();
    }

    public void DisplayText(string s)
    {
        tm.text = s;
        timeTillFade = Time.time + fadeDelayTime;
    }

    void Update()
    {
        Color textColor = Color.white;
        float overtime = Mathf.Clamp(Time.time - timeTillFade, 0.0f, fadeTransitionTime);
        textColor.a = 1.0f - overtime / fadeTransitionTime;
        tm.color = textColor;
    }
}
