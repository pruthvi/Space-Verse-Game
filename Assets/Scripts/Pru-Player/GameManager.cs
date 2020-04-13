using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;


    //  Sets the Score to the ScoreBoard
    public void SetScore(int score)
    {
        scoreText.text = "Score : " + score;
    }
}
