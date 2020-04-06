using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{

    // Load Game Scene
    public void StartGame()
    {
        SceneManager.LoadScene("Playground");
    }

    public void ShowLeaderboard()
    {
        // Social.ShowLeaderboardUI();
        Debug.Log("Loads Leaderboard");
    }

    //Exit Game
    public void ExitGame()
    {
        Application.Quit();
    }

}
