using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acelerador : Objeto
{
    // Mejora del modificador de cadencia
    [SerializeField]
    float mejorarCadencia = 0.025f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("ReimuPointsCollectionHitbox"))
        {
            collision.GetComponentInParent<DisparoJugador>().MejorarCadencia(mejorarCadencia);
            Destroy(gameObject);
        }
    }
}
