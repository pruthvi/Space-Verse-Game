using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    private int _totalScore = 0;

    //  Sets the Score to the ScoreBoard
    public void SetScore(int score)
    {
        _totalScore += score;
        scoreText.text = _totalScore.ToString();
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
