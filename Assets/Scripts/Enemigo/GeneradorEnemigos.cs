using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{
    [SerializeField]
    GameObject[] enemigosPrefabs;

    [SerializeField]
    float[] pesosDeEnemigos;

    public float intervaloDeGeneracion = 1f;
    public float anchoAreaDeGeneracion = 6f;

    private float tiempoSiguienteGeneracion;

    void Start()
    {
        
    }

    void Update()
    {
        if (Time.time >= tiempoSiguienteGeneracion)
        {
            GenerarEnemigo();
            tiempoSiguienteGeneracion = Time.time + intervaloDeGeneracion;
        }
    }

    // Función para escoger un enemigo con peso
    int EscogerEnemigoConPeso()
    {
        float sumaPesos = 0f;
        foreach (float peso in pesosDeEnemigos)
        {
            sumaPesos += peso;
        }

        float valorAleatorio = Random.Range(0f, sumaPesos);
        float acumulado = 0f;

        for (int i = 0; i < enemigosPrefabs.Length; i++)
        {
            acumulado += pesosDeEnemigos[i];
            if (valorAleatorio < acumulado)
            {
                return i;
            }
        }
        return enemigosPrefabs.Length - 1;  // Si algo sale mal, devolver el último enemigo
    }

    // Genera un enemigo y lo posiciona en una posición aleatoria de la parte superior de la pantalla.
    void GenerarEnemigo()
    {
        float coordenadaGeneracionX = Random.Range(-anchoAreaDeGeneracion / 2f, anchoAreaDeGeneracion / 2f);
        Vector2 posicionGeneracion = new Vector2(coordenadaGeneracionX, 6f);

        // Escoge un enemigo respecto a sus pesos
        int enemigoElegidoIndex = EscogerEnemigoConPeso();

        GameObject nuevoEnemigo = Instantiate(enemigosPrefabs[enemigoElegidoIndex], posicionGeneracion, Quaternion.identity);

        // Reposicionamiento a la posición de ataque.
        nuevoEnemigo.GetComponent<Enemigo>().posicionDesplazamientoObjetivo = new Vector2(coordenadaGeneracionX, Random.Range(3f, 3.5f));
    }
}
