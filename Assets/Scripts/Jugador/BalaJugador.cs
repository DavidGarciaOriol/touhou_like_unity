using UnityEngine;

public class BalaJugador : MonoBehaviour
{
    // Velocidad de desplazamiento de la bala
    public float velocidad = 10f;

    [SerializeField] // Daño base de la bala, sobre el que se aplican los modificadores del Disparo del Jugador
    int damageBase = 1;
    int damageModificador = 1;
    public int DamageModificador { get => damageModificador; set => damageModificador = value; }

    [SerializeField] // Estado básico de perforación de la bala. False por defecto
    bool penetracion = false;
    public bool Penetracion { get => penetracion; set => penetracion = value; }

    // Referencia al Sprite
    SpriteRenderer sprite;

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // Desplaza la bala hacia arriba
        transform.Translate(Vector2.up * velocidad * Time.deltaTime);
    }

    // Maneja las colisiones con un enemigo o un Ovni porta objetos. Si tiene perforación, no se destruye al contacto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null) 
        { 
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Enemigo>().RecibirDamage(damageBase * damageModificador);
                if (!Penetracion)
                {
                    Destroy(gameObject);
                }
            }

            if (collision.CompareTag("UFOItem"))
            {
                collision.GetComponent<OvniPortaItem>().RecibirDamage(damageBase * damageModificador);
                if (!Penetracion)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
