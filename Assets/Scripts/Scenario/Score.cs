using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int scorevalue = 0;
    private Text scoring;

    void Start()
    {
        scoring = GetComponent<Text>();
        scorevalue = 0;
    }

    void Update()
    {
        scoring.text = "Score: " + scorevalue;
        if (EnemyController.zombiescore == 1)
        {
            scorevalue += 10;
            EnemyController.zombiescore = 0;
        }
        if (BossReaper.bossscore == 1)
        {
            scorevalue += 200;
            BossReaper.bossscore = 0;
        }
        if (PowerUp.powerupScore == 1)
        {
            scorevalue += 50;
            PowerUp.powerupScore = 0;
        }
    }
}

