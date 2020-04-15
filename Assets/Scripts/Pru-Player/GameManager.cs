using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject finalScore;
    public Text scoreText;
    public GameObject dieScreen;

    private int _totalScore = 0;

    private void Start()
    {
        GameStarted();
    }

    //  Sets the Score to the ScoreBoard
    public void SetScore(int score)
    {
        _totalScore += score;
    }

    //  Displays final total score with count animation
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

    //  On Starting game, reset the UI parameters
    public void GameStarted()
    {
        finalScore.SetActive(false);
        dieScreen.SetActive(false);
        _totalScore = 0;
    }
}
