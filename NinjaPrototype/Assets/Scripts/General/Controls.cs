using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Controls : MonoBehaviour
{
    Player player;
    List<Enemy> enemys = new List<Enemy>();
    List<Gadget> gadgets = new List<Gadget>();

    bool roundStart = true;
    bool playerRound = true;

    List<Hoverable> newHoverables = new List<Hoverable>();
    List<Hoverable> oldHoverables = new List<Hoverable>();

    public void RegisterEnemy(Enemy e)
    {
        enemys.Add(e);
    }

    public void RegisterGadget(Gadget g)
    {
        gadgets.Add(g);
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        Collider2D[] allColliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        newHoverables.Clear();
        foreach (Collider2D c2D in allColliders)
        {
            if (!c2D.isTrigger)
            {
                Hoverable h = c2D.gameObject.GetComponentInParent<Hoverable>();
                if (h)
                {
                    newHoverables.Add(h);
                }
            }
        }
        foreach(Hoverable h in oldHoverables.Except(newHoverables))
        {
            if (h)
            {
                h.SetFocus(false);
            }
        }
        foreach (Hoverable h in newHoverables.Except(oldHoverables))
        {
            if (h)
            {
                h.SetFocus(true);
            }
        }
        oldHoverables = newHoverables.ToList();

        if (playerRound)
        {
            if (roundStart)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (newHoverables.Count > 0)
                    {
                        if (player.IsReady())
                        {
                            foreach(Hoverable h in newHoverables)
                            {
                                if (h is DestinationDot)
                                {
                                    if (player.GoToDot((DestinationDot)newHoverables.First()))
                                    {
                                        roundStart = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (h.OnClick())
                                    {
                                        roundStart = false;
                                        break;
                                    }
                                }
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
                foreach (Gadget g in gadgets)
                {
                    g.DoStep();
                }
                foreach (Enemy e in enemys)
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
