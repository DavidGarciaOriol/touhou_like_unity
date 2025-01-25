using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxGraze : MonoBehaviour
{
    /* Grazing es la mecánica con la cual, al pasar cerca de una bala
     * pero sin tocarla le suma puntos al jugador para premiar el "Risky Gameplay".
     */

    // Velocidad rotación.
    float velocidadRotacion = 25;
    public bool rotacionInversa = false;
    Quaternion rotacion;

    // Puntos de Grazing
    int puntos = 5;

    // Clip audio grazing
    [SerializeField]
    AudioClip audioGraze;

    void Update()
    {
        // Rotación de la hitbox
        float direccion = rotacionInversa ? -1f : 1f;
        transform.Rotate(0f, 0f, velocidadRotacion * direccion * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("EnemyBullet") || collision.CompareTag("Enemy"))
            {
                ControladorSonidos.instance.ReproducirSonido(audioGraze, 0.45f);
                GameManager.instance.AgregarPuntos(puntos);
                GameManager.instance.SumarContadorGraze();
            }
        }
    }
}
