using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Cable : Gadget
{
    public AudioSource stripSoundSource;
    public AudioSource sizzleSound;
    public Sprite sparkSprite;
    public float killRadius = 0.5f;

    bool isKilling = false;
    SpriteRenderer r;

    #if UNITY_EDITOR
    public new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.forward, killRadius);
    }
    #endif

    public override void Start()
    {
        base.Start();
        r = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if (isKilling)
        {
            if (!stripSoundSource.isPlaying)
            {
                if (!sizzleSound.isPlaying)
                {
                    sizzleSound.Play();
                }
            }
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, killRadius, LayerMask.GetMask("Enemies"));
            foreach (Collider2D c2D in colliders)
            {
                Enemy enemy = c2D.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.Die();
                }
            }
        }
    }

    public override void TurnOn()
    {
        r.sprite = sparkSprite;
        isKilling = true;
        stripSoundSource.Play();

    }

    public override void TurnOff()
    {

    }

    public override bool IsReady()
    {
        return true;
    }

    public override bool OnClick()
    {
        if (PlayerIsInRange() && !isKilling)
        {
            TurnOn();
            return true;
        }
        return false;
    }

    public override void DoStep()
    {

    }
}
