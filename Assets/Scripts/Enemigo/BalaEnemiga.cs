using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaEnemiga : MonoBehaviour
{
    // Tiempo de vida de la bala
    public float tiempoVida = 5f;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        // Destruir la bala si sale de la pantalla
        if (Mathf.Abs(transform.position.x) > 10 || Mathf.Abs(transform.position.y) > 10)
        {
            Destroy(gameObject);
        }
    }

    // Al colisionar con el jugador, éste pierde una vida y la bala desaaprece
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ReimuDamageHitbox"))
        {
            Destroy(gameObject);
        }
    }
}
