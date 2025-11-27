using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI txtScore;

    void Start()
    {
        txtScore.text = "0" + " puntos";
    }

    public void UpdateScore(int score)
    {
    if (score == 1)
        {
        txtScore.text = score.ToString() + " punto";
        }
    else
        {
        txtScore.text = score.ToString() + " puntos";
        }
    }


}
