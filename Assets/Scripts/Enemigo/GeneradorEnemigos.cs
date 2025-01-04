using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{

    public GameObject enemigoPrefab;

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

    // Genera un enemigo y lo posiciona en una posición aleatoria de la parte superior de la pantalla.
    void GenerarEnemigo()
    {
        float coordenadaGeneracionX = Random.Range(-anchoAreaDeGeneracion / 2f, anchoAreaDeGeneracion / 2f);
        Vector2 posicionGeneracion = new Vector2(coordenadaGeneracionX, 6f);

        GameObject nuevoEnemigo = Instantiate(enemigoPrefab, posicionGeneracion, Quaternion.identity);

        // Reposicionamiento a la posición de ataque.
        nuevoEnemigo.GetComponent<Enemigo>().posicionDesplazamientoObjetivo = new Vector2(coordenadaGeneracionX, Random.Range(3f, 3.5f));
    }
}
