using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RollingBarrel : Gadget
{
    public Transform barrelVisuals;
    public GameObject soundSprite;

    public float rollSpeed = 2.0f;
    public float fallAcceleration = 9.0f;
    public float alertRadius = 10.0f;

    bool rolling = true;
    bool falling = true;
    float downMomentum = 0.0f;
    float radius;
    LayerMask wallsLayer;

    #if UNITY_EDITOR
    public new void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.forward, alertRadius);
    }
    #endif

    public override void Start()
    {
        base.Start();
        wallsLayer = LayerMask.GetMask("Walls");
        radius = GetComponent<CircleCollider2D>().radius;
    }

    void Update()
    {
        if (falling)
        {
            if (GetGroundDistance() > radius + 0.05f)
            {
                downMomentum += Time.deltaTime * fallAcceleration;
            }
            else
            {
                downMomentum = 0.0f;
                if (!rolling)
                {
                    soundSprite.SetActive(true);
                    AlertEnemies(alertRadius);
                    falling = false;
                }
            }

            if (GetGroundDistance() < downMomentum * Time.deltaTime || GetGroundDistance() < radius)
            {
                RaycastHit2D rh2d = Physics2D.Raycast(transform.position, Vector2.down, 20.0f, wallsLayer);
                transform.position = rh2d.point + Vector2.up * radius;
            }
            else
            {
                transform.position += Vector3.down * downMomentum * Time.deltaTime;
            }
        }

        if (WallInFront())
        {
            rolling = false;
        }

        if (rolling)
        {
            barrelVisuals.Rotate(Vector3.forward * Time.deltaTime * -110.0f * rollSpeed);
            transform.position += Vector3.right * Time.deltaTime * rollSpeed;
        }
    }

    float GetGroundDistance()
    {
        RaycastHit2D rh2d = Physics2D.Raycast(transform.position, Vector2.down, 20.0f, wallsLayer);
        if (rh2d.collider)
        {
            return rh2d.distance;
        }
        return 20.0f;
    }

    bool WallInFront()
    {
        RaycastHit2D rh2d = Physics2D.Raycast(transform.position, Vector2.right, 1.0f, wallsLayer);
        if (rh2d.collider)
        {
            return (rh2d.distance < radius + 0.1f);
        }
        return false;
    }

    public override void TurnOn()
    {

    }

    public override void TurnOff()
    {
        soundSprite.SetActive(false);
    }

    public override bool IsReady()
    {
        if(!falling && !rolling)
        {
            return true;
        }
        return false;
    }

    public override bool OnClick()
    {
        return false;
    }

    public override void DoStep()
    {

    }
}
