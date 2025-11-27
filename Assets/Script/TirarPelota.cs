using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirarPelota : MonoBehaviour
{
    public Transform shootPoint;            
    public Rigidbody currentBall;           
    public UnityEngine.UI.Slider sliderFuerza;
    public float minForce = 6f;            
    public float maxForce = 18f;            
    public float maxChargeTime = 1.2f;      

    public float upwardForce = 3f;          

    public bool changeScaleWhileCharging = true;
    public float maxScaleMultiplier = 1.12f; 

    private float chargeTimer = 0f;
    private bool isCharging = false;

    void Update()
    {
        if (currentBall == null)
            return;

        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCharge();
        }

        if (Input.GetKey(KeyCode.L) && isCharging)
        {
            ContinueCharge();
        }

        if (Input.GetKeyUp(KeyCode.L) && isCharging)
        {
            ReleaseAndThrow();
        }
    }

    void StartCharge()
    {
        isCharging = true;
        chargeTimer = 0f;

        if (sliderFuerza != null)
        sliderFuerza.value = 0f;

        if (changeScaleWhileCharging)
            currentBall.transform.localScale = Vector3.one; 
    }

    void ContinueCharge()
    {
        chargeTimer += Time.deltaTime;
        chargeTimer = Mathf.Min(chargeTimer, maxChargeTime);

        if (changeScaleWhileCharging)
        {
            float t = chargeTimer / maxChargeTime;
            float scale = 1f + (maxScaleMultiplier - 1f) * t;
            currentBall.transform.localScale = Vector3.one * scale;
        }
        if (sliderFuerza != null)
        {
            float t = chargeTimer / maxChargeTime;   // Va de 0 a 1
            sliderFuerza.value = t;
        }

    }   

    void ReleaseAndThrow()
    {
        float tNorm = chargeTimer / maxChargeTime;      
        float appliedForward = Mathf.Lerp(minForce, maxForce, tNorm);

        Rigidbody rb = currentBall.GetComponent<Rigidbody>();

        currentBall.transform.SetParent(null, true);

        currentBall.transform.position = shootPoint.position + shootPoint.forward * 0.15f;

        rb.isKinematic = false;

        Vector3 direction;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        
        if (sliderFuerza != null)
        sliderFuerza.value = 0f;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            direction = (hit.point - shootPoint.position).normalized;
        }
        else
        {
            direction = Camera.main.transform.forward;
        }

        Vector3 finalForce = direction * appliedForward + Vector3.up * upwardForce;

        rb.AddForce(finalForce, ForceMode.Impulse);

        rb.AddTorque(Vector3.right * 1.5f, ForceMode.Impulse);

        if (changeScaleWhileCharging)
            currentBall.transform.localScale = Vector3.one;

        currentBall = null;
        isCharging = false;
        chargeTimer = 0f;
    }
}
