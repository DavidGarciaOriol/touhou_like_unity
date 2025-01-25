
using UnityEngine;

public class HitboxDamage : MonoBehaviour
{
    // Velocidad rotación.
    float velocidadRotacion = 25;
    public bool rotacionInversa = false;

    // Update is called once per frame
    void Update()
    {
        float direccion = rotacionInversa? -1f : 1f;

        transform.Rotate(0f, 0f, velocidadRotacion * direccion * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("EnemyBullet") || collision.CompareTag("Enemy") || collision.CompareTag("UFOItem"))
            {
                GetComponentInParent<Jugador>().RecibirDamage();
            }
        }
    }
}
