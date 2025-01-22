using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punto : Objeto
{
    // Valor de puntuacion
    public int valor = 100;

    // Referencia fuente de audio
    [SerializeField]
    AudioClip audioPuntoObtenido;

    // Si choca con la hitbox de puntos del jugador, recibes los puntos, almacenándolos en la partida
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ReimuPointsCollectionHitbox"))
        {
            ControladorSonidos.instance.ReproducirSonido(audioPuntoObtenido);
            GameManager.instance.AgregarPuntos(100);
            Destroy(gameObject);
        }
    }
}
