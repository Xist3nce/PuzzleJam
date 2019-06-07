using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class DestinationDot : MonoBehaviour
{
    public Sprite outOfFocusSprite;
    public Sprite inFocusSprite;
    public bool guardAccessible = true;

    public float travelRadius = 6.0f;

    double lastUpdateTime = 0.0f;

    public List<DestinationDot> destinations;

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

    public void SetFocus(bool isInFocus)
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

    [ContextMenu("Update Connections")]
    public void RunUpdateConnectionWithEditorTime()
    {
        UpdateConnection(EditorApplication.timeSinceStartup);
    }

    public void UpdateConnection(double time)
    {
        Undo.RecordObject(this, "UpdateConnection");
        if(lastUpdateTime == time)
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
                    dd.UpdateConnection(time);
                }
            }
        }
        PrefabUtility.RecordPrefabInstancePropertyModifications(this);
    }
}

