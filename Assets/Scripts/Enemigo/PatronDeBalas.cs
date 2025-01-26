using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronDeBalas : MonoBehaviour
{
    // El prefab de la bala
    public GameObject balaPrefab;

    // Tiempo entre disparos
    public float intervaloDisparo = 0.5f;

    // N�mero de balas
    public int numeroBalas = 2;

    // �ngulo inicial del disparo
    public float anguloInicial = 0f;

    // Separaci�n entre balas
    public float espaciadoAngulo = 15f;

    // Velocidad de las balas
    public float velocidadBala = 3f;

    // Corrutina activa
    private Coroutine rutinaDisparo;

    // Numero de iteraciones
    public int numeroIteraciones = 1;

    // Si est� atacando
    public bool atacando = false;

    // L�gica para iniciar la corrutina de disparo
    public void IniciarPatron(Transform origen)
    {
        if (rutinaDisparo == null)
        {
            atacando = true;

            rutinaDisparo = StartCoroutine(DispararBalas(origen));
        }
    }

    // Detiene la corrutina
    public void DetenerPatron()
    {
        atacando = false;

        if (rutinaDisparo != null)
        {
            
            StopCoroutine(rutinaDisparo);
            rutinaDisparo = null;
        }
    }

    // Corrutina para disparar las balas
    private IEnumerator DispararBalas(Transform origen)
    {
        // Numero de veces que tiene que ejecutarse el mismo patr�n
        for (int i = 0; i < numeroIteraciones; i++)
        {
            // Si el enemigo ha sido derrotado en mitad del ataque
            if (origen == null)
            {
                yield break;
            }

            // N�mero de balas por ciclo del patr�n
            for (int j = 0; j < numeroBalas; j++)
            {
                float angulo = anguloInicial + j * espaciadoAngulo - (espaciadoAngulo * (numeroBalas - 1) / 2);
                CrearBala(origen.position, angulo);
            }

            yield return new WaitForSeconds(intervaloDisparo);
        }
    }

    // Genera la bala seg�n el patr�n
    private void CrearBala(Vector2 posicion, float angulo)
    {
        GameObject bala = Instantiate(balaPrefab, posicion, Quaternion.Euler(0, 0, angulo+90));
        Vector2 direccion = new Vector2(Mathf.Cos(angulo * Mathf.Deg2Rad), Mathf.Sin(angulo * Mathf.Deg2Rad));
        bala.GetComponent<Rigidbody2D>().velocity = direccion * velocidadBala;
    }
}
