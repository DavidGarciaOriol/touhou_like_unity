using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Jugador : MonoBehaviour
{
    // Velocidad del personaje
    float velocidadMaxima = 4.5f;
    public float velocidadNormal = 4.5f;
    public float velocidadReducida = 2.25f;
    private Vector2 velocidadActual;

    // Si el jugador puede moverse. False al recibir daño por ejemplo.
    private bool puedeMoverse = true;

    // Si está pulsando la tecla de focus
    bool estaReduciendoVelocidad = false;
    
    // Dirección del movimiento del personaje
    private Vector2 direccionMovimiento;

    // Componente Rigidbody
    private Rigidbody2D rigidbody2D;

    // Restricciones de movimiento
    private Vector2 limitesMinimos;
    private Vector2 limitesMaximos;

    // Componente SpriteRenderer Reimu
    private SpriteRenderer spriteRendererReimu;
    private Color spriteReimuColorBase;
    private Color newReimuSpriteColor;

    // Hitbox
    public GameObject hitboxInterior;
    public GameObject hitboxPuntos;
    public GameObject hitboxGraze;

    // Explosión al perder vidas
    public GameObject prefabExplosionParticulas;
    public GameObject prefabPunto;
    public Transform posicionInicial;
    public float tiempoInvulnerabilidad = 2f;
    public bool esInvulnerable = false;

    // Clip de audio de muerte
    [SerializeField]
    AudioClip audioMuerte;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRendererReimu = GetComponent<SpriteRenderer>();
        spriteReimuColorBase = spriteRendererReimu.color;

        // Posición inicial
        if (posicionInicial == null)
        {
            GameObject puntoInicial = new GameObject("PuntoInicial");
            puntoInicial.transform.position = transform.position;
            posicionInicial = puntoInicial.transform;
            Debug.Log("Punto inicial creado automáticamente en: " + posicionInicial.position);
        }

        OcultarReimuHitbox();

        // Calcula los límites del escenario usando la cámara principal
        Camera camara = Camera.main;
        if (camara != null)
        {
            Vector3 esquinaInferiorIzquierda = camara.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 esquinaSuperiorDerecha = camara.ViewportToWorldPoint(new Vector3(1, 1, 0));

            limitesMinimos = new Vector2(esquinaInferiorIzquierda.x, esquinaInferiorIzquierda.y);
            limitesMaximos = new Vector2(esquinaSuperiorDerecha.x, esquinaSuperiorDerecha.y);
        }

    }

    void Update()
    {
        if (!puedeMoverse)
        {
            return;
        }

        ManejoInputs();
        Movimiento();

        // Restringe la posición del jugador dentro de los límites
        Vector3 posicionRestringida = transform.position;
        posicionRestringida.x = Mathf.Clamp(posicionRestringida.x, limitesMinimos.x, limitesMaximos.x);
        posicionRestringida.y = Mathf.Clamp(posicionRestringida.y, limitesMinimos.y, limitesMaximos.y);
        transform.position = posicionRestringida;

        velocidadActual = rigidbody2D.velocity;

        if (!estaReduciendoVelocidad)
        {
            OcultarReimuHitbox();
            CambiarColorPersonaje(spriteReimuColorBase);
        }
        else
        {
            MostrarReimuHitbox();
            newReimuSpriteColor = spriteReimuColorBase;
            newReimuSpriteColor.a = 0.6f;
            CambiarColorPersonaje(newReimuSpriteColor);
        }
    }

    private void CambiarColorPersonaje(Color color)
    {
        spriteRendererReimu.color = color;
    }

    private void OnDisable()
    {
        rigidbody2D.velocity = Vector2.zero;
    }

    // Recoge los inputs del mando / teclado
    void ManejoInputs()
    {
        // Axis de movimiento en teclado y mando
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Comprobar tecla de reducción de velocidad
        estaReduciendoVelocidad = Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("ZL") > 0.1f;

        // Ajustar velocidad máxima segú si la reduce o no
        velocidadMaxima = estaReduciendoVelocidad ? velocidadReducida : velocidadNormal;

        // Asignar mahnitudes y velocidad a la dirección de movimiento
        direccionMovimiento = new Vector2(horizontal, vertical);
    }
    void MostrarReimuHitbox()
    {
        if (hitboxInterior != null)
        {
            SpriteRenderer sr = hitboxInterior.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = true; // Hacer la hitbox visible
            }
        }
    }

    void OcultarReimuHitbox()
    {
        if (hitboxInterior != null)
        {
            SpriteRenderer sr = hitboxInterior.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = false; // Ocultar la hitbox visualmente
            }
        }
    }

    // Procesa el movimiento del jugador
    void Movimiento()
    {
        if (direccionMovimiento.magnitude > 0 && estaReduciendoVelocidad)
        {
            direccionMovimiento.Normalize();
        }

        rigidbody2D.velocity = new Vector2(direccionMovimiento.x * velocidadMaxima, direccionMovimiento.y * velocidadMaxima);
    }

    public void RecibirDamage()
    {
        if (esInvulnerable)
        {
            Debug.Log("El jugador es invulnerable, saliendo de RecibirDamage().");
            return;
        }

        ControladorSonidos.instance.ReproducirSonido(audioMuerte);

        esInvulnerable = true;

        GameManager.instance.RestarVidas();

        Debug.Log("Vidas restadas.");

        // Detiene el movimiento del jugador
        DetenerMovimiento();

        StartCoroutine(ProcesoPerderVida());

        Debug.Log("Entrado en la corutina, terminando RecibirDamage().");
    }

    IEnumerator ProcesoPerderVida()
    {
        Debug.Log("Entrando en ProcesoPerderVida");

        // Bloquear movimiento
        puedeMoverse = false;

        // Se instancia la explosión
        Instantiate(prefabExplosionParticulas, transform.position, Quaternion.identity);

        // Se generan los puntos que pierde el jugador y salen despedidos
        // GenerarPuntosDespedidos();

        // Desactivamos al jugador temporalmente de forma visual
        DesactivarJugadorVisualmente();
        Debug.Log("Jugador desactivado temporalmente.");

        // Reaparece el jugador en la posición inicial tras esperar un segundo
        yield return new WaitForSeconds(1f);

        transform.position = posicionInicial.position;
        Debug.Log("Posición inicial asignada: " + transform.position);

        // Reactivamos el visual del jugador
        ActivarJugadorVisualmente();
        Debug.Log("Jugador reactivado.");

        // Activamos invulnerabilidad por dos segundos
        StartCoroutine(ActivarInvulnerabilidad());
        puedeMoverse = true;
    }

    void DesactivarJugadorVisualmente()
    {
        Debug.Log("Desactivando componentes del jugador.");

        // Detiene el movimiento del jugador. Repetido por seguridad
        DetenerMovimiento();

        // Desactiva el renderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        // Desactiva el collider
        Collider2D jugadorCollider = GetComponent<Collider2D>();
        if (jugadorCollider != null)
            jugadorCollider.enabled = false;

        // Descativa las hitboxes
        if (hitboxInterior != null)
            hitboxInterior.GetComponent<SpriteRenderer>().enabled = false;

        if (hitboxGraze != null)
            hitboxGraze.GetComponent<SpriteRenderer>().enabled = false;

        if (hitboxPuntos != null)
            hitboxPuntos.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void ActivarJugadorVisualmente()
    {
        Debug.Log("Reactivando componentes del jugador.");

        // Reactiva el renderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        // Reactiva el collider
        Collider2D jugadorCollider = GetComponent<Collider2D>();
        if (jugadorCollider != null)
            jugadorCollider.enabled = true;

        // Reactiva las hitboxes
        if (hitboxInterior != null)
            hitboxInterior.GetComponent<SpriteRenderer>().enabled = true;

        if (hitboxGraze != null)
            hitboxGraze.GetComponent<SpriteRenderer>().enabled = true;

        if (hitboxPuntos != null)
            hitboxPuntos.GetComponent<SpriteRenderer>().enabled = true;
    }

    void DetenerMovimiento()
    {
        // Detén el movimiento del Rigidbody
        if (rigidbody2D != null)
        {
            direccionMovimiento = Vector2.zero;
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    // Genera los puntos despedidos al perder vida
    /*void GenerarPuntosDespedidos()
    {
        Debug.Log("Generando puntos perdidos despedidos");

        int puntosPerdidos = GameManager.instance.CalcularPuntosPerdidos();

        if (puntosPerdidos >= 100)
        {
            for (int i = 0; i < puntosPerdidos; i += 100)
            {
                Vector3 posicionPunto = transform.position;
                prefabPunto = Instantiate(prefabPunto, posicionPunto, Quaternion.identity);

                // Aplicar fuerza hacia arriba y aleatoria
                Rigidbody2D rb = prefabPunto.GetComponent<Rigidbody2D>();
                Vector2 fuerza = new Vector2(Random.Range(-1f, 1f), Random.Range(1f, 2f));
                rb.AddForce(fuerza * 5f, ForceMode2D.Impulse);
            }
        }
    }*/

    // Activa invulnerabilidad por 2 segundos al reaparecer tras perder vida
    IEnumerator ActivarInvulnerabilidad()
    {
        Debug.Log("Entrando en ActivarInvulnerabilidad");

        // Desactiva el collider del jugador
        Collider2D jugadorCollider = GetComponent<Collider2D>();
        if (jugadorCollider != null)
            jugadorCollider.enabled = false;

        // Animación de parpadeo
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        for (float t = 0; t < tiempoInvulnerabilidad; t += 0.2f)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.2f);
        }

        // Finalizar invulnerabilidad
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        if (jugadorCollider != null)
            jugadorCollider.enabled = true;

        esInvulnerable = false; // Asegúrate de que esta línea siempre se ejecute
        Debug.Log("Invulnerabilidad desactivada.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("AttractionArea"))
            {
                Punto[] puntosEnPantalla = FindObjectsOfType<Punto>();
                foreach (Punto punto in puntosEnPantalla)
                {
                    punto.ActivarAtraccion(transform);
                }
            }
        }
    }
}
