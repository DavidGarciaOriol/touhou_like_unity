using UnityEngine;

public class BalaJugador : MonoBehaviour
{
    public float velocidad = 10f;
    public int damage = 1;
    int damageModificador = 0;
    int finalDamage = 0;

    Animator animacion;
    SpriteRenderer sprite;

    void Start()
    {
        damageModificador = GetComponentInParent<DisparoJugador>().damageModificador;
        finalDamage = damage * damageModificador;
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * velocidad * Time.deltaTime);
    }
}
