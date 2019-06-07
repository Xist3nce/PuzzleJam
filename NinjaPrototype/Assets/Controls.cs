using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Controls : MonoBehaviour
{
    public MovingEntity player;
    public List<Enemy> enemys = new List<Enemy>();

    bool roundStart = true;
    bool playerRound = true;

    List<DestinationDot> newDotsList = new List<DestinationDot>();
    List<DestinationDot> oldDotsList = new List<DestinationDot>();

    public void RegisterEnemy(Enemy e)
    {
        enemys.Add(e);
    }

    void Update()
    {
        Collider2D[] allColliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        newDotsList.Clear();
        foreach (Collider2D c2D in allColliders)
        {
            if (c2D.gameObject.CompareTag("Dot"))
            {
                DestinationDot dd = c2D.gameObject.GetComponent<DestinationDot>();
                newDotsList.Add(dd);
                dd.SetFocus(true);
            }
        }
        foreach(DestinationDot dd in oldDotsList.Except(newDotsList))
        {
            dd.SetFocus(false);
        }
        oldDotsList = newDotsList.ToList();

        if (playerRound)
        {
            if (roundStart)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (newDotsList.Count > 0)
                    {
                        if (player.IsReady())
                        {
                            if (player.GoToDot(newDotsList.First()))
                            {
                                roundStart = false;
                            }
                        }
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    roundStart = false;
                }
            }
            else
            {
                if (player.IsReady())
                {
                    playerRound = false;
                    roundStart = true;
                }
            }
        }
        else
        {
            if (roundStart)
            {
                foreach(Enemy e in enemys)
                {
                    e.DoStep();
                }
                roundStart = false;
            }
            else
            {
                bool allEnemysDidSteps = true;
                foreach (Enemy e in enemys)
                {
                    if (!e.IsReady())
                    {
                        allEnemysDidSteps = false;
                    }
                }
                if (allEnemysDidSteps)
                {
                    roundStart = true;
                    playerRound = true;
                }
            }
        }
    }
}
