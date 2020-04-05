using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public int Score;
    public Text scoreText;

    void Start()
    {
        Score = 0;
    }

    void Update()
    {
        scoreText.text = "Score : " + Score;
    }
}
