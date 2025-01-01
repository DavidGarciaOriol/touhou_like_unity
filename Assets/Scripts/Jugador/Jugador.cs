using UnityEngine;

public class Jugador : MonoBehaviour
{
    // Velocidad del personaje
    public float velocidadMaxima = 5f;

    // Componente Rigidbody
    private Rigidbody2D rigidbody2D;

    // Dirección del movimiento del personaje
    private Vector2 direccionMovimiento;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ManejoInputs();
        Movimiento();
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

        direccionMovimiento = new Vector2(horizontal, vertical);
    }

    // Procesa el movimiento del jugador
    void Movimiento()
    {
        rigidbody2D.velocity = new Vector2(direccionMovimiento.x * velocidadMaxima, direccionMovimiento.y * velocidadMaxima);
    }

}
