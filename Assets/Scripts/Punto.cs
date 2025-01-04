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
        // Si está siendo atraido al juagdor, se desplaza hacia él hasta alcanzarlo, en caso contrario, solo cae hacia abajo
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

    // Inicia el estado de atracción al jugador si éste lo habilita entrando en la zona de atracción de puntos
    public void ActivarAtraccion(Transform jugadorTransform)
    {
        siendoAtraidoAlJugador = true;
        jugador = jugadorTransform;
    }

    // Si choca con la hitbox de puntos del jugador, recibes los puntos, almacenándolos en la partida
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColliderPoint"))
        {
            GameManager.instance.AgregarPuntos(100);
            Destroy(gameObject);
        }
    }
}
