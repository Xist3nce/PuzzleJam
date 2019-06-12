using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DestinationDot : Hoverable
{
    public Sprite outOfFocusSprite;
    public Sprite inFocusSprite;
    public bool guardAccessible = true;

    public float travelRadius = 6.0f;

    double lastUpdateTime = 0.0f;

    public List<DestinationDot> destinations;

    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        foreach(DestinationDot d in destinations)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, d.transform.position);
        }
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.forward, travelRadius);
    }
    #endif

    public override void SetFocus(bool isInFocus)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (isInFocus)
        {
            sr.sprite = inFocusSprite;
        }
        else
        {
            sr.sprite = outOfFocusSprite;
        }
    }

    #if UNITY_EDITOR
    [ContextMenu("Update Connections")]
    public void RunUpdateConnectionWithEditorTime()
    {
        UpdateConnection(EditorApplication.timeSinceStartup);
    }
    #endif

    public void UpdateConnection(double time)
    {
        #if UNITY_EDITOR
        Undo.RecordObject(this, "UpdateConnection");
        #endif
        if (lastUpdateTime == time)
        {
            return;
        }
        lastUpdateTime = time;
        destinations.Clear();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, travelRadius, LayerMask.GetMask("Dots"));
        foreach(Collider2D c2D in colliders)
        {
            DestinationDot dd = c2D.GetComponent<DestinationDot>();
            if(dd != this)
            {
                if (!Physics2D.Linecast(transform.position, c2D.transform.position, LayerMask.GetMask("Walls")))
                {
                    destinations.Add(dd);
                }
            }
        }
        DestinationDot[] allDots = FindObjectsOfType<DestinationDot>();
        foreach(DestinationDot dd in allDots)
        {
            dd.UpdateConnection(time);
        }
        #if UNITY_EDITOR
        PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        #endif
    }

    public override bool OnClick()
    {
        return false;
    }
}

