using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsible for managing Overall GameScoring and UI
/// </summary>
public class GameManager : MonoBehaviour
{
    public GameObject finalScore;       //  GameObject of Final Total Score in the Death Screen
    public Text scoreText;              //  Score to display
    public GameObject dieScreen;        //  Die Screen GameObject

    private int _totalScore = 0;        //  Final Total Score

    /// <summary>
    /// Initialize UI and Score
    /// </summary>
    private void Start()
    {
        InitializeGame();
    }

    /// <summary>
    /// On Starting game, reset the UI parameters
    /// </summary>
    public void InitializeGame()
    {
        finalScore.SetActive(false);
        dieScreen.SetActive(false);
        _totalScore = 0;
    }

    /// <summary>
    /// Sets the Score to the ScoreBoard
    /// </summary>
    /// <param name="score">Small chunk of score</param>
    public void SetScore(int score)
    {
        _totalScore += score;
    }

    /// <summary>
    /// Displays the final total score with count animation
    /// </summary>
    public void DisplayTotalScore()
    {
        finalScore.SetActive(true);

        var scoreCount = 1;
        while (_totalScore >= scoreCount)
        {
            scoreText.text = scoreCount.ToString();
            scoreCount *=2;
        }

        //  Wait until scoring count is finished
        //  Show Restart/Exit Button
        dieScreen.SetActive(true);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PostScoreOnLeaderboard();
            SceneManager.LoadScene("Menu");
        }
    }
    public void PostScoreOnLeaderboard()
    {
        Social.ReportScore(_totalScore, "CgkIpoDx6LIFEAIQAQ", (bool success) => {
            // handle success or failure
            Debug.Log("Score Posted on Leaderboard");
        });
    }

}
