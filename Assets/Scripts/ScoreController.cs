using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int currentStore = 0;
    private float currentScore;

    private void Start()
    {
        currentStore = 0;
    }

    public float GetCurrentscore()
    {
        return currentScore;
    }

    public void IncreaseCurrentScore(int Increment)
    {
        currentScore += Increment;
    }

    public void FinishScoring()
    {
        if (currentScore > ScoreData.highScore)
            {
            ScoreData.highScore = currentStore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
