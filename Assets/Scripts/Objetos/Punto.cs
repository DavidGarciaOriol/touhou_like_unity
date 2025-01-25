using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punto : Objeto
{
    // Valor de puntuacion
    public int valor = 100;

    // Si choca con la hitbox de puntos del jugador, recibes los puntos, almacenándolos en la partida
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("ReimuPointsCollectionHitbox"))
        {
            GameManager.instance.AgregarPuntos(100);
            Destroy(gameObject);
        }
    }
}
