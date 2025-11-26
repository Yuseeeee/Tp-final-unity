using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TirarPelota : MonoBehaviour
{
    public Transform shootPoint;            // punto desde donde sale la pelota (empty delante de la cam)
    public Rigidbody currentBall;           // lo llena AgarrarPelota

    public float minForce = 6f;             // fuerza mínima al soltar rápido
    public float maxForce = 18f;            // fuerza máxima al cargar full
    public float maxChargeTime = 1.2f;      // tiempo en segundos para llegar a maxForce

    public float upwardForce = 3f;          // componente vertical para dar parábola

    // feedback
    public bool changeScaleWhileCharging = true;
    public float maxScaleMultiplier = 1.12f; // escala máxima durante carga

    // estados internos
    private float chargeTimer = 0f;
    private bool isCharging = false;

    void Update()
    {
        // Solo permitir cargar/soltar si hay pelota agarrada
        if (currentBall == null)
            return;

        // Empezar carga
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCharge();
        }

        // Mantener carga
        if (Input.GetKey(KeyCode.L) && isCharging)
        {
            ContinueCharge();
        }

        // Soltar y tirar
        if (Input.GetKeyUp(KeyCode.L) && isCharging)
        {
            ReleaseAndThrow();
        }
    }

    void StartCharge()
    {
        isCharging = true;
        chargeTimer = 0f;

        // Opcional: feedback visual inicial
        if (changeScaleWhileCharging)
            currentBall.transform.localScale = Vector3.one; // reset
    }

    void ContinueCharge()
    {
        chargeTimer += Time.deltaTime;
        chargeTimer = Mathf.Min(chargeTimer, maxChargeTime);

        // Feedback visual simple: agranda la pelota ligeramente mientras cargas
        if (changeScaleWhileCharging)
        {
            float t = chargeTimer / maxChargeTime;
            float scale = 1f + (maxScaleMultiplier - 1f) * t;
            currentBall.transform.localScale = Vector3.one * scale;
        }
    }

    void ReleaseAndThrow()
    {
        // calcular la fuerza según tiempo de carga (interpolación)
        float tNorm = chargeTimer / maxChargeTime;      // 0..1
        float appliedForward = Mathf.Lerp(minForce, maxForce, tNorm);

        // preparar la pelota
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();

        // asegurar que se suelte bien del handPoint / parent
        currentBall.transform.SetParent(null, true);

        // desplazar un poco hacia adelante para evitar colisión con la cámara/jugador
        // (usá shootPoint.forward si shootPoint está correctamente orientado)
        currentBall.transform.position = shootPoint.position + shootPoint.forward * 0.15f;

        // activar física
        rb.isKinematic = false;

        // dirección objetivo: usar raycast desde la cámara para apuntar al punto exacto
        Vector3 direction;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            direction = (hit.point - shootPoint.position).normalized;
        }
        else
        {
            direction = Camera.main.transform.forward;
        }

        // añadir un poco de upwardForce para parábola
        Vector3 finalForce = direction * appliedForward + Vector3.up * upwardForce;

        // aplicar fuerza como Impulse (mejor para lanzamientos)
        rb.AddForce(finalForce, ForceMode.Impulse);

        // opcional: aplicar torque para spin
        rb.AddTorque(Vector3.right * 1.5f, ForceMode.Impulse);

        // reset visual/estado
        if (changeScaleWhileCharging)
            currentBall.transform.localScale = Vector3.one;

        currentBall = null;
        isCharging = false;
        chargeTimer = 0f;
    }
}
