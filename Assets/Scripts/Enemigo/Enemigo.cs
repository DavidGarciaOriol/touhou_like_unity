using System.Collections;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    // Posición a la que el enemigo se moverá para disparar tras aparecer
    public Vector2 posicionDesplazamientoObjetivo;

    // Velocidad
    public float velocidad = 1.85f;

    // Salud
    public int salud = 3;

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


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorOriginal = spriteRenderer.color;
        animador = GetComponent<Animator>();
        
        colliderEnemigo = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!posicionObjetivoAlcanzada) {
            DesplazarseAlObjetivo();
        } 
        else 
        {
            AbandonarPantalla();
        }
    }

    // Movimiento hacia la posición de disparo
    void DesplazarseAlObjetivo()
    {
        transform.position = Vector2.MoveTowards(transform.position, posicionDesplazamientoObjetivo, velocidad * Time.deltaTime);

        if ((Vector2)transform.position == posicionDesplazamientoObjetivo)
        {
            posicionObjetivoAlcanzada = true;

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

    // Movimiento para abandonar la pantalla y desaparecer
    void AbandonarPantalla()
    {
        transform.position = Vector2.MoveTowards(transform.position, posicionSalida, (velocidad * 0.5f) * Time.deltaTime);

        if ((Vector2)transform.position == posicionSalida)
        {
            Destroy(gameObject);
        }
    }

    public void RecibirDamage(int damageRecibido)
    {
        salud -= damageRecibido;
        StartCoroutine(EfectoAlSerGolpeado());
        if (salud <= 0)
        {
            Morir();
        }
    }

    IEnumerator EfectoAlSerGolpeado()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = colorOriginal;
    }

    void Morir()
    {
        animador.SetTrigger("Morir");
        colliderEnemigo.enabled = false;
        Instantiate(puntoPrefab, transform.position, Quaternion.identity);
        StartCoroutine(AnimacionDeMuerte());
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

        // Ajustamos  nueva posición original y tiempo transcurrido
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
    }
}
