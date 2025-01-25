using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perforador : Objeto
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("ReimuPointsCollectionHitbox"))
        {
            collision.GetComponentInParent<DisparoJugador>().CambiarPenetracion(true);
            Destroy(gameObject);
        }
    }
}
