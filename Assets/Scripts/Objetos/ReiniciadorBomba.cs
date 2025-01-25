using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReiniciadorBomba : Objeto
{
    [SerializeField] // Reducción de cooldown de la bomba
    float reduccionCooldown = 20f;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("ReimuPointsCollectionHitbox"))
        {
            Bomba.instance.RestarTiempoCooldown(reduccionCooldown);
            Destroy(gameObject);
        }
    }
}
