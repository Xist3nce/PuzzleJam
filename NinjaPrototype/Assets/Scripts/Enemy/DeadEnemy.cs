using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEnemy : MonoBehaviour
{
    public AudioSource barrelDieSound;
    public AudioSource electricDieSound;

    public void PlayDieSound(KillReason kr)
    {
        if(kr == KillReason.Barrel)
        {
            barrelDieSound.Play();
        }
        if(kr == KillReason.Wires)
        {
            electricDieSound.Play();
        }
    }
}
