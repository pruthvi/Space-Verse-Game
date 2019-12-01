using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetDistance : MonoBehaviour
{

    public GameObject player;
    public float distance;
    public int score;
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        distance = 0;
        scoreText.text = "Score:" + score;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, this.transform.position);

        if(distance<1)
        {
            float newScore = 1 / distance; 
            score += (int)newScore;
            scoreText.text = "Score : " + score;
        }

    }
}
