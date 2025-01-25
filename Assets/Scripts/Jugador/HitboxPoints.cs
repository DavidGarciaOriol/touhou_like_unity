using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxPoints : MonoBehaviour
{
    // Velocidad rotaci�n.
    float velocidadRotacion = 25;
    public bool rotacionInversa = false;
    Quaternion rotacion;

    void Update()
    {
        // Rotaci�n de la hitbox
        float direccion = rotacionInversa ? -1f : 1f;
        transform.Rotate(0f, 0f, velocidadRotacion * direccion * Time.deltaTime);
    }
}
