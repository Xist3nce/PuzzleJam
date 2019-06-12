using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    public GameObject blackscreenSprite;
    public AudioSource looseSound;

    Player player;
    AudioSource audioSource;
    List<Enemy> enemys = new List<Enemy>();
    List<Gadget> gadgets = new List<Gadget>();

    bool roundStart = true;
    bool playerRound = true;

    bool restarting;

    List<Hoverable> newHoverables = new List<Hoverable>();
    List<Hoverable> oldHoverables = new List<Hoverable>();

    public void RegisterEnemy(Enemy e)
    {
        enemys.Add(e);
    }

    public void RemoveEnemy(Enemy e)
    {
        enemys.Remove(e);
    }

    public void RegisterGadget(Gadget g)
    {
        gadgets.Add(g);
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        audioSource = GetComponent<AudioSource>();
    }

    public void RestartLevel()
    {
        if (!looseSound.isPlaying)
        {
            if(!restarting)
            {
                restarting = true;
                looseSound.Play();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        //blackscreenSprite.SetActive(false);
    }

    public void BlankScreen()
    {
        blackscreenSprite.SetActive(true);
    }

    void Update()
    {
        if (restarting)
        {
            return;
        }
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

        /*if (audioSource.isPlaying)
        {
            return;
        }*/

        if (playerRound)
        {
            if (roundStart)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (player.IsReady())
                    {
                        Collider2D[] allClickColliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), ~LayerMask.GetMask("HoverNoClick"));
                        foreach (Collider2D c2D in allClickColliders)
                        {
                            if (!c2D.isTrigger)
                            {
                                Hoverable h = c2D.gameObject.GetComponentInParent<Hoverable>();
                                if (h)
                                {
                                    if (h is DestinationDot)
                                    {
                                        if (player.GoToDot((DestinationDot)h))
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
                    bool allGadgetsDidSteps = true;
                    foreach (Gadget g in gadgets)
                    {
                        if (!g.IsReady())
                        {
                            allGadgetsDidSteps = false;
                        }
                    }
                    if (allGadgetsDidSteps)
                    {
                        roundStart = true;
                        playerRound = false;
                    }
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
                    //audioSource.Play();
                }
            }
        }
    }
}
