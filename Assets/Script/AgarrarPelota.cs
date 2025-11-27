using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgarrarPelota : MonoBehaviour
{
    public float pickupDistance = 3f;
    public LayerMask ballLayer;
    public TirarPelota TirarPelota;
    public Transform PuntoDeAgarre;

    void Update()
    {

        TryPickupBall();

        Debug.DrawRay(Camera.main.transform.position,
                      Camera.main.transform.forward * pickupDistance,
                      Color.red);
    }

    void TryPickupBall()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance, ballLayer))
        {
            Rigidbody ball = hit.collider.GetComponent<Rigidbody>();

            if (ball != null && TirarPelota.currentBall == null)
            {
                Pickup(ball);
            }
        }
    }

    void Pickup(Rigidbody ball)
    {
        TirarPelota.currentBall = ball;
        ball.isKinematic = true;

        ball.transform.position = PuntoDeAgarre.position;
        ball.transform.rotation = PuntoDeAgarre.rotation;

        ball.transform.parent = PuntoDeAgarre;
    }
}
