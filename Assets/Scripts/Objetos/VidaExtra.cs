using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaExtra : Objeto
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("ReimuPointsCollectionHitbox"))
        {
            GameManager.instance.SumarVidas();
            Destroy(gameObject);
        }
    }
}
