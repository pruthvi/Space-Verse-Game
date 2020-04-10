using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;

    private int _totalScore;

    //  Sets the Score to the ScoreBoard
    public void SetScore(int score)
    {
        _totalScore += score;
        scoreText.text = "Score : " + _totalScore;
    }
}
