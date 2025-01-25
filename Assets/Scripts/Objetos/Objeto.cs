using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objeto : MonoBehaviour
{
    // Velocidad de caída
    public float velocidadCaida = 1f;
    public float velocidadAtraccion = 10f;

    // Referencia al jugador para adquirir su posición
    Transform jugador;

    // Si está siendo atraido al jugador
    bool siendoAtraidoAlJugador = false;

    // Referencia fuente de audio
    [SerializeField]
    AudioClip audioObjetoObtenido;

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

    // Inicia el estado de atracción al jugador si éste lo habilita entrando en la zona de atracción de objetos
    public void ActivarAtraccion(Transform jugadorTransform)
    {
        siendoAtraidoAlJugador = true;
        jugador = jugadorTransform;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ReimuPointsCollectionHitbox"))
        {
            ControladorSonidos.instance.ReproducirSonido(audioObjetoObtenido, 0.6f);
        }
    }
}
