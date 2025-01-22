using UnityEngine;

public class BalaJugador : MonoBehaviour
{
    public float velocidad = 10f;

    [SerializeField]
    int damage = 1;
    int damageModificador = 0;
    public int finalDamage = 0;

    [SerializeField]
    Jugador jugador;

    Animator animacion;
    SpriteRenderer sprite;

    void Start()
    {
        damageModificador = jugador.GetComponent<DisparoJugador>().damageModificador;
        finalDamage = damage * damageModificador;
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null) 
        { 
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Enemigo>().RecibirDamage(finalDamage);
                if (!jugador.GetComponent<DisparoJugador>().atraviesa)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
