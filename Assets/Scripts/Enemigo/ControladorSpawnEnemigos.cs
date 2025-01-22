using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControladorSpawnEnemigos : MonoBehaviour
{
    [System.Serializable]
    public class Ronda
    {
        public string nombreRonda;

        // Duración de la ronda en segundos
        public float duracion;

        // Intervalo entre spawns
        public float intervaloSpawneo;

        // Lista de tipos de enemigo y sus probabilidades
        public List<TipoEnemigo> enemigosDisponibles;
    }

    [System.Serializable]
    public class TipoEnemigo
    {
        // Prefab del enemigo
        public GameObject prefabEnemigo;

        // Probabilidad de aparición
        public float probabilidad; 
    }

    // Lista con todas las rondas
    public List<Ronda> rondas;

    // Puntos de spawneo de enemigos
    public float anchoAreaDeGeneracion = 6f;

    // Altura desde la que aparecen los enemigos
    public float alturaGeneracion = 6f;

    // Rango de las posiciones de ataque iniciales
    public Vector2 rangoPosicionAtaque = new Vector2(2.5f, 3.5f); 

    private int indiceRondaActual = 0;
    private bool jugando = true;

    private void Start()
    {
        StartCoroutine(GestionarRondas());
    }

    private IEnumerator GestionarRondas()
    {
        while (jugando)
        {
            if (indiceRondaActual >= rondas.Count)
            {
                EscalarDificultad();
            }

            Ronda rondaActual = rondas[indiceRondaActual];
            Debug.Log($"Iniciando {rondaActual.nombreRonda}");

            float tiempoRonda = 0f;

            // Spawnea enemigos por la duración de la ronda actual
            while (tiempoRonda < rondaActual.duracion)
            {
                yield return StartCoroutine(SpawnearEnemigo(rondaActual));
                tiempoRonda += rondaActual.intervaloSpawneo;
                yield return new WaitForSeconds(rondaActual.intervaloSpawneo);
            }

            // Avanza a la siguiente ronda
            indiceRondaActual++;
        }
    }

    private IEnumerator SpawnearEnemigo(Ronda rondaActual)
    {
        // Elige enemigo en función a su probabilidad
        GameObject enemigoElegido = ElegirEnemigo(rondaActual.enemigosDisponibles);

        if (enemigoElegido != null)
        {
            // Se genera la posición de spawn del enemigo desde arriba de la pantalla
            float coordX = Random.Range(-anchoAreaDeGeneracion / 2f, anchoAreaDeGeneracion / 2f);
            Vector2 posicionGeneracion = new Vector2(coordX, alturaGeneracion);

            GameObject nuevoEnemigo = Instantiate(enemigoElegido, posicionGeneracion, Quaternion.identity);

            // Configurar la posición de ataque inicial
            Vector2 posicionAtaque = new Vector2(coordX, Random.Range(rangoPosicionAtaque.x, rangoPosicionAtaque.y));

            if (nuevoEnemigo.TryGetComponent<Enemigo>(out Enemigo scriptEnemigo))
            {
                scriptEnemigo.posicionDesplazamientoObjetivo = posicionAtaque;
            }
        }

        yield return null;
    }

    private GameObject ElegirEnemigo(List<TipoEnemigo> enemigos)
    {
        float sumaTotal = 0;
        foreach (var enemigo in enemigos)
        {
            sumaTotal += enemigo.probabilidad;
        }

        float eleccion = Random.Range(0, sumaTotal);
        float acumulador = 0;

        foreach (var enemigo in enemigos)
        {
            acumulador += enemigo.probabilidad;
            if (eleccion <= acumulador)
            {
                return enemigo.prefabEnemigo;
            }
        }
        return null;
    }

    private void EscalarDificultad()
    {
        Debug.Log("Escalando dificultad más allá de las rondas definidas.");

        Ronda ultimaRonda = rondas[rondas.Count - 1];
        Ronda nuevaRonda = new Ronda
        {
            nombreRonda = "Ronda Infinita",
            duracion = ultimaRonda.duracion,
            intervaloSpawneo = Mathf.Max(0.5f, ultimaRonda.intervaloSpawneo * 0.9f),
            enemigosDisponibles = new List<TipoEnemigo>()
        };

        foreach (var enemigo in ultimaRonda.enemigosDisponibles)
        {
            nuevaRonda.enemigosDisponibles.Add(new TipoEnemigo
            {
                prefabEnemigo = enemigo.prefabEnemigo,
                probabilidad = enemigo.probabilidad * 1.1f
            });
        }

        rondas.Add(nuevaRonda);
    }
}
