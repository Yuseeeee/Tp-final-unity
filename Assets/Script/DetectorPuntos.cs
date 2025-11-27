using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorPuntos : MonoBehaviour
{
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        ValorPelota bola = other.GetComponent<ValorPelota>();

        if (bola != null)
        {
            scoreManager.AddScore(bola.scorePoints);
        }
    }

}
