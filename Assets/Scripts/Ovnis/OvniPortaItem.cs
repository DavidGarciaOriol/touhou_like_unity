using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvniPortaItem : MonoBehaviour
{
    [SerializeField] // Referencia al item que suelta al ser destruido
    GameObject objetoMejora;

    [SerializeField] // Cantidad de ese item que suelta el ovni
    int cantidadObjetos = 1;

    // Velocidad del ovni
    float velocidad = 1.5f;

    // Posición desde la que sale el Ovni
    Transform posicionSalida;

    [SerializeField] // Salud del ovni
    int salud = 10;

    // Dirección del ovni
    Vector2 direccion;

    // Sprite del ovni
    SpriteRenderer spriteRenderer;

    // Color original del ovni
    private Color colorOriginal;

    // Collider Ovni
    Collider2D colliderOvni;

    // Sonido de aparición del Ovni
    [SerializeField]
    AudioClip sonidoAparicion;

    void Start()
    {
        ControladorSonidos.instance.ReproducirSonido(sonidoAparicion, 1f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorOriginal = spriteRenderer.color;
        posicionSalida = GetComponent<Transform>();
        colliderOvni = GetComponent<Collider2D>();

        // La velocidad se establece aleatoriamente al aparecer
        velocidad = Random.Range(1f, 2f);
        
        // Depende de la posición de salida, se desplaza a la derecha o a la izquierda
        if (posicionSalida.position.x < 0)
        {
            direccion = Vector2.right;
        }
        else
        {
            spriteRenderer.flipX = true;
            direccion = Vector2.left;
        }
        
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    // Cuando recibe daño del jugador
    public void RecibirDamage(int damageRecibido)
    {
        salud -= damageRecibido;
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

    void Morir()
    {
        SoltarObjeto();
        colliderOvni.enabled = false;
        StartCoroutine(AnimacionDeMuerte());
    }

    void SoltarObjeto()
    {
        for (int i = 0; i < cantidadObjetos; i++)
        {
           Instantiate(objetoMejora,
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
