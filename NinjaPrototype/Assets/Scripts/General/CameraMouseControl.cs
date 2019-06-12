using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseControl : MonoBehaviour
{
    public int scrollStartDistance = 100;
    public float maxMoveSpeed = 20.0f;
    public float standartCameraOrthoSize = 10.0f;
    public TextSetAndFade text;

    Camera cam;
    float mouseScroll = 0.0f;

    Rect cameraPosRect;

    void Start()
    {
        cam = Camera.main;
    }

    public void SetCameraBounds(Rect r)
    {
        transform.position = new Vector3(r.center.x, r.center.y, transform.position.z);
        cameraPosRect = r;
    }

    public void DisplayLevelText(string s)
    {
        text.DisplayText(s);
    }

    void Update()
    {
        if (MouseIsInWindow())
        {
            Vector2 mousePosFromCenter = (Vector2)Input.mousePosition - new Vector2(Screen.width, Screen.height) / 2;
            Vector2 noScrollRectSize = new Vector2(Screen.width - 2 * scrollStartDistance, Screen.height - 2 * scrollStartDistance) / 2;
            float xMove = Mathf.Lerp(0, maxMoveSpeed, (Mathf.Abs(mousePosFromCenter.x) - noScrollRectSize.x) / scrollStartDistance);
            xMove *= Mathf.Sign(mousePosFromCenter.x);
            float yMove = Mathf.Lerp(0, maxMoveSpeed, (Mathf.Abs(mousePosFromCenter.y) - noScrollRectSize.y) / scrollStartDistance);
            yMove *= Mathf.Sign(mousePosFromCenter.y);

            Vector2 move = new Vector2(xMove, yMove) * Time.deltaTime * cam.orthographicSize;
            if (!cameraPosRect.Contains(transform.position + new Vector3(0,move.y,0)))
            {
                move.y = 0;
            }
            if (!cameraPosRect.Contains(transform.position + new Vector3(move.x, 0, 0)))
            {
                move.x = 0;
            }

            transform.position += new Vector3(move.x, move.y, 0);

            mouseScroll -= Input.mouseScrollDelta.y;
            mouseScroll = Mathf.Clamp(mouseScroll, -8.0f, 10.0f);
            cam.orthographicSize = standartCameraOrthoSize + (int)mouseScroll;
        }
    }

    bool MouseIsInWindow()
    {
        if (!Application.isFocused)
        {
            return false;
        }
        Vector3 view = cam.ScreenToViewportPoint(Input.mousePosition);
        return !(view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1);
    }
}
