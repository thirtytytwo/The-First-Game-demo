using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager manager;
    SceneFader fader;
    List<Orb> orbs;
    Door lockedDoor;

    float gameTime;
    bool gameIsOver;

    public int deathNum;
    private void Awake()
    {
        if (manager != null)
        {
            Destroy(gameObject);
            return;
        }
        manager = this;

        DontDestroyOnLoad(this);

        orbs = new List<Orb>();
    }
    private void Update()
    {
        if(gameIsOver)
        {
            return;
        }
        gameTime += Time.deltaTime;

        UIManager.UpdateGameTime(gameTime);
    }

    public static void RegisterDoor(Door door)
    {
        manager.lockedDoor = door;
    }
    public static void RegisterSceneFader(SceneFader obj)
    {
        manager.fader = obj;
    }
    public static void RegisterOrb(Orb orb)
    {
        if (!manager.orbs.Contains(orb))
        {
            manager.orbs.Add(orb);
        }
        UIManager.UpdateOrbUI(manager.orbs.Count);
    }
    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (!manager.orbs.Contains(orb))
        {
            return;
        }
        manager.orbs.Remove(orb);

        UIManager.UpdateOrbUI(manager.orbs.Count);

        if(manager.orbs.Count == 0)
        {
            manager.lockedDoor.Open();
        }
    }
    public static void PlayerDie()
    {
        manager.fader.FadeOut();

        manager.deathNum++;

        manager.Invoke("RestartScene", 1.5f);

        UIManager.UpdatePlayerDeath(manager.deathNum);
    }
    public static void PlayerWin()
    {
        manager.gameIsOver = true;

        UIManager.DisplayGameOver();
    }
    public static bool GameOver()
    {
        return manager.gameIsOver;
    }
    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        AudioManager.StartLevel();

        manager.orbs.Clear();
    }
 
}
