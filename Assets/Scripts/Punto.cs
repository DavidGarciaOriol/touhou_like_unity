using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punto : MonoBehaviour
{

    // Velocidades del punto
    public float velocidadCaida = 1f;
    public float velocidadAtraccion = 10f;

    // Valor de puntuacion
    public int valor = 100;

    // Referencia al jugador para adquirir su posición
    Transform jugador;

    // Si está siendo atraido al jugador
    bool siendoAtraidoAlJugador = false;

    void Update()
    {
        if (siendoAtraidoAlJugador && jugador != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidadAtraccion * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.down * velocidadCaida * Time.deltaTime);

            if (transform.position.y < -5.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ActivarAtraccion(Transform jugadorTransform)
    {
        siendoAtraidoAlJugador = true;
        jugador = jugadorTransform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColliderPoint"))
        {
            GameManager.instance.AgregarPuntos(100);
            Destroy(gameObject);
        }
    }
}
