using UnityEngine;

public class Jugador : MonoBehaviour
{
    // Velocidad del personaje
    float velocidadMaxima = 5f;
    public float velocidadNormal = 5f;
    public float velocidadReducida = 2f;

    // Vidas del personaje
    public int vidasReimu = 3;

    Vector2 velocidadActual;

    // Componente Rigidbody
    private Rigidbody2D rigidbody2D;

    // Dirección del movimiento del personaje
    private Vector2 direccionMovimiento;

    // Hitbox
    public GameObject hitboxInterior;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        OcultarReimuHitbox();
    }

    void Update()
    {
        ManejoInputs();
        Movimiento();

        velocidadActual = rigidbody2D.velocity;

        if (velocidadActual.magnitude > 2.5f || velocidadActual.magnitude == 0f)
        {
            OcultarReimuHitbox();
        }
        else
        {
            MostrarReimuHitbox();
        }
    }

    private void OnDisable()
    {
        rigidbody2D.velocity = Vector2.zero;
    }

    // Recoge los inputs del mando / teclado
    void ManejoInputs()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Comprobar  tecla de reducción de velocidad
        bool estaReduciendoVelocidad = Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("ZL") > 0.1f;

        // Ajustar velocidad máxima según el estado de reducción
        velocidadMaxima = estaReduciendoVelocidad ? velocidadReducida : velocidadNormal;


        direccionMovimiento = new Vector2(horizontal, vertical);
    }
    void MostrarReimuHitbox()
    {
        hitboxInterior.SetActive(true);
    }

    void OcultarReimuHitbox()
    {
        hitboxInterior.SetActive(false);
    }

    // Procesa el movimiento del jugador
    void Movimiento()
    {
        if (direccionMovimiento.magnitude  > 0)
        {
            direccionMovimiento.Normalize();
        }

        rigidbody2D.velocity = new Vector2(direccionMovimiento.x * velocidadMaxima, direccionMovimiento.y * velocidadMaxima);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null) 
        {
            if (collision.CompareTag("Enemy"))
            {
                vidasReimu -= 1;
            }
        }

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
