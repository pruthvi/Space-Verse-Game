using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalcDistance : MonoBehaviour
{
    public GameObject player;
    public float distance;
    public int score;

    GameManager GM;
    public GameObject GameManagerObj;
    // Start is called before the first frame update
    void Start()
    {

        GM = GameManagerObj.GetComponent<GameManager>();
        distance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < 120)
        {
            Debug.Log("distance is less than 120");

            float newScore = 120 / distance;
            score = (int)newScore;
            GM.Score += (int)newScore;
        }

    }
}
