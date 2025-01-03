using UnityEngine;

public class BalaJugador : MonoBehaviour
{

    public float velocidad = 10f;
    public int damage = 1;

    Animator animacion;
    SpriteRenderer sprite;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemigo>().RecibirDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        // Destruir la bala al salir de la pantalla
        Destroy(gameObject);
    }
}
