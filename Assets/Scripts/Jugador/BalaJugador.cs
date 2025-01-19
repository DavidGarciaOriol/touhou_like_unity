using UnityEngine;

public class BalaJugador : MonoBehaviour
{
    public float velocidad = 10f;
    public int damage = 1;

    Animator animacion;
    SpriteRenderer sprite;

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * velocidad * Time.deltaTime);
    }
}
