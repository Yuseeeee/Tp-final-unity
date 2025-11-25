using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirarPelota : MonoBehaviour
{
    public Rigidbody currentBall; 

    public Rigidbody ballPrefab;
    public Transform shootPoint;

    public float force = 500f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Throw();
        }
    }

    void Throw()
    {
        if (currentBall != null)
        {
            currentBall.transform.parent = null; 
            currentBall.isKinematic = false;     
            currentBall.AddForce(shootPoint.forward * force);
            currentBall = null;
        }
        else
        {
            Rigidbody newBall = Instantiate(ballPrefab, shootPoint.position, shootPoint.rotation);
            newBall.AddForce(shootPoint.forward * force);
        }
    }
}

