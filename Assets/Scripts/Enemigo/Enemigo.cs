using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    // Posición a la que el enemigo se moverá para disparar tras aparecer
    public Vector2 posicionDesplazamientoObjetivo;

    // Velocidad
    public float velocidad = 2f;

    // Salud
    public int salud = 5;

    // Indica si el enemigo ha alcanzado si posición de disparo
    private bool posicionObjetivoAlcanzada = false;

    // Posición de salida de la pantalla
    private Vector2 posicionSalida;

    // Referencia al renderer del sprite del enemigo
    private SpriteRenderer spriteRenderer;

    // Animación de muerte
    private Animator animador;

    // Color original del enemigo
    private Color colorOriginal;

    // Collider del enemigo
    private Collider2D colliderEnemigo;

    // Puntos que suelta el enemigo al morir
    public GameObject puntoPrefab;
    public int cantidadPuntos;

    // Patrón de Balas
    public PatronDeBalas[] patronesDeBalas;

    // Instancias de los Patrones de Balas
    private List<GameObject> instanciasPatrones = new List<GameObject>();

    // Tiempo entre ataques o duración del ataque
    public float duracionAtaque = 3f;

    // Numero de ciclos de disparo
    public int ciclosDisparo = 3;

    // Si está en animación de muerte
    public bool muriendo;

    // Clip audio recibir daño
    [SerializeField]
    AudioClip sonidoRecibirDamage;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorOriginal = spriteRenderer.color;
        animador = GetComponent<Animator>();

        colliderEnemigo = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Desplazarse hacia el objetivo
        if (!posicionObjetivoAlcanzada) 
        {
            DesplazarseAlObjetivo();
        }

        // Si está en el objetivo y ya no está atacando, abandonar la escena
        else
        {
            // Variables para controlar numero total de patrones y
            // si han acabado su ataque
            int instanciasTotales = 0;
            int instanciasFinalizadas = 0;
            
            // Suma la actual a las totales. Si ha terminado de atacar,
            // se suma a finalizadas
            foreach (GameObject instancia in instanciasPatrones)
            {
                instanciasTotales += 1;
                if (!instancia.GetComponent<PatronDeBalas>().atacando)
                {
                    instanciasFinalizadas += 1;
                }
            }

            // Si las instancias esperadas y finalizadas coinciden,
            // el enemigo tiene via libre para abandonar la pantalla
            if (instanciasFinalizadas == instanciasTotales)
            {
                AbandonarPantalla();
            }
        }

        // Si el enemigo está muriendo, detiene todas las instancias
        // de patrones instantáneamente
        if (instanciasPatrones != null && muriendo)
        {
            foreach (GameObject instancia in instanciasPatrones)
            {
                if (instancia != null)
                {
                    PatronDeBalas patron = instancia.GetComponent<PatronDeBalas>();
                    patron.DetenerPatron();
                }
            }
        }
    }

    // Movimiento hacia la posición de disparo
    void DesplazarseAlObjetivo()
    {
        transform.position = Vector2.MoveTowards(transform.position, posicionDesplazamientoObjetivo, velocidad * Time.deltaTime);

        if ((Vector2)transform.position == posicionDesplazamientoObjetivo && !muriendo)
        {
            posicionObjetivoAlcanzada = true;
            StartCoroutine(RealizarAtaque());

            // Calcular el destino de salida
            if (transform.position.x >= 0)
            {
                // Si aparece en la mitad derecha, se mueve hacia el borde izquierdo
                posicionSalida = new Vector2(-4f, transform.position.y);
            }
            else
            {
                // Si aparece en la mitad izquierda, se mueve hacia el borde derecho
                posicionSalida = new Vector2(4f, transform.position.y);
            }
        }
    }

    // Corrutina que se encarga de la lógica de ataque
    private IEnumerator RealizarAtaque()
    {
        // Limpiar la lista de instancias anteriores
        instanciasPatrones.Clear();

        // Iniciar todos los patrones de balas asignados
        foreach (var patronPrefab in patronesDeBalas)
        {
            if (patronPrefab != null)
            {
                GameObject instancia = Instantiate(patronPrefab.gameObject, transform.position, Quaternion.identity);
                instanciasPatrones.Add(instancia); // Guardar la instancia en la lista

                PatronDeBalas patron = instancia.GetComponent<PatronDeBalas>();
                patron.numeroIteraciones = ciclosDisparo;
                patron.intervaloDisparo = duracionAtaque / ciclosDisparo;
                patron.IniciarPatron(transform);
            }
        }

        // Esperar el tiempo del ataque
        yield return new WaitForSeconds(duracionAtaque);

        // Detener todos los patrones
        foreach (GameObject instancia in instanciasPatrones)
        {
            PatronDeBalas patron = instancia.GetComponent<PatronDeBalas>();
            patron.DetenerPatron();
        }

        // Comenzar el movimiento de salida
        AbandonarPantalla();
    }

    // Movimiento para abandonar la pantalla y desaparecer
    void AbandonarPantalla()
    {
        transform.position = Vector2.MoveTowards(transform.position, posicionSalida, (velocidad * 0.5f) * Time.deltaTime);

        if ((Vector2)transform.position == posicionSalida)
        {
            Destroy(gameObject);
        }
    }

    // Cuando recibe daño del proyectil del jugador
    public void RecibirDamage(int damageRecibido)
    {
        salud -= damageRecibido;
        ControladorSonidos.instance.ReproducirSonido(sonidoRecibirDamage, 0.25f);
        StartCoroutine(EfectoAlSerGolpeado());
        if (salud <= 0)
        {
            Morir();
        }
    }

    // Parpadeo rojo al recibir daño
    IEnumerator EfectoAlSerGolpeado()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = colorOriginal;
    }

    // Cuando es derrotado
    void Morir()
    {   
        muriendo = true;
        if (animador != null)
        {
            animador.SetTrigger("Morir");
        }
        colliderEnemigo.enabled = false;
        SoltarPuntos();
        StartCoroutine(AnimacionDeMuerte());
    }

    void SoltarPuntos()
    {
        for (int i = 0; i < cantidadPuntos; i++)
        {
            Instantiate(puntoPrefab,
                new Vector2(
                    transform.position.x + Random.Range(-0.15f, 0.15f),
                    transform.position.y + Random.Range(-0.15f, 0.15f)),
                Quaternion.identity);
        }
    }

    IEnumerator AnimacionDeMuerte()
    {
        float duracionTransparencia = 0.8f;
        float velocidadTransparencia = 1f / duracionTransparencia;

        float duracionMovimientoArriba = duracionTransparencia * 0.2f;
        float duracionMovimientoAbajo = duracionTransparencia - duracionMovimientoArriba;

        Vector2 posicionOriginal = transform.position;
        Color color = colorOriginal;

        float tiempoTranscurrido = 0f;

        // Movimiento leve hacia arriba
        while (tiempoTranscurrido < duracionMovimientoArriba)
        {
            // Subida lineal hacia arriba
            float movimientoArriba = Mathf.Lerp(0, 0.15f, tiempoTranscurrido / duracionMovimientoArriba);
            transform.position = posicionOriginal + new Vector2(0, movimientoArriba);
            
            // Reducción de opacidad del sprite
            color.a -= velocidadTransparencia * Time.deltaTime;
            spriteRenderer.color = color;

            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        // Ajustamos nueva posición original y tiempo transcurrido
        posicionOriginal = transform.position;
        tiempoTranscurrido = 0f;

        // Movimiento hacia abajo
        while (tiempoTranscurrido < duracionMovimientoAbajo)
        {
            // Caída rápida hacia abajo
            spriteRenderer.flipY = true;
            float movimientoAbajo = Mathf.Lerp(0, -0.75f, tiempoTranscurrido / duracionMovimientoAbajo);
            transform.position = posicionOriginal + new Vector2(0, movimientoAbajo);

            // Reducción de opacidad del sprite.
            color.a -= velocidadTransparencia * Time.deltaTime;
            spriteRenderer.color = color;

            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        color.a = 0;
        spriteRenderer.color = color;

        // Eliminar el objeto
        Destroy(gameObject);

        yield break;
    }
}
