using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // Velocidad rotación.
    float velocidadRotacion = 25;
    public bool rotacionInversa = false;
    Quaternion rotacion;

    // Update is called once per frame
    void Update()
    {
        float direccion = rotacionInversa? -1f : 1f;

        transform.Rotate(0f, 0f, velocidadRotacion * direccion * Time.deltaTime);
    }
}
