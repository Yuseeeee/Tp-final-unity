using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public UIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        score = 0;
        uiManager.UpdateScore(score);
    }

    public void AddScore()
    {
        score++;
        uiManager.UpdateScore(score);
    }

    public void AddScore(int scorePoints)
    {
        score += scorePoints;
        uiManager.UpdateScore(score);
    }
}
