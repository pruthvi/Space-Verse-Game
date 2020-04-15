using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{

    public AudioClip nextLevelSound; // Gets Main Game Sound

    // Function to show Leaderboard
    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
        Debug.Log("Loads Leaderboard");
    }

    // Function to load game through Start Button
    public void LoadGame()
    {
        ChangeMusic(nextLevelSound);    // Changes sound track for game before level loads
        StartCoroutine(AsyncSceneLoading()); // Loads Level Async
    }

    IEnumerator AsyncSceneLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Playground");
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            yield return null;
        }
    }

    // Function gets audio manager and changes sound clip
    void ChangeMusic(AudioClip levelSound)
    {
        GameObject audioMG = GameObject.Find("AudioManager");
        audioMG.GetComponent<AudioManager>().SoundTransition(levelSound);
    }

    // Exit Game
    public void ExitGame()
    {
        Application.Quit();
    }

}
