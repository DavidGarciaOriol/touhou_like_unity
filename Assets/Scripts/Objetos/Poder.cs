using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poder : Objeto
{
    // Mejora del modificador de daño
    [SerializeField]
    int mejoraDamage = 1;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("ReimuPointsCollectionHitbox"))
        {
            collision.GetComponentInParent<DisparoJugador>().MejorarDamage(mejoraDamage);
            Destroy(gameObject);
        }
    }
}
