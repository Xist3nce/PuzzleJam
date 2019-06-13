using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public MenuAction ma;

    Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            foreach(Collider2D c2D in colliders)
            {
                if(c2D == myCollider)
                {
                    switch (ma)
                    {
                        case MenuAction.play:
                            SceneManager.LoadScene("managers");
                            break;
                        case MenuAction.exit:
                            Application.Quit();
                            break;
                        case MenuAction.levelSelect:
                            break;
                        case MenuAction.end:
                            SceneManager.LoadScene("end");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}

public enum MenuAction
{
    play,
    exit,
    levelSelect,
    end
}
