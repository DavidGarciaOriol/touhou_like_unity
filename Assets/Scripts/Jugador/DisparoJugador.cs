using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    // Objeto prefab del la bala / disparo
    public GameObject balaPrefab;

    // Ritmo de disparo
    public float ratioDisparo = 0.1f;

    // Posición de salida de la bala / diapro
    public Transform posicionDisparo;

    // Temporizador
    private float tiempoSiguienteDisparo;

    void Start()
    {

    }

    void Update()
    {
        // Se calcula el ratio de disparo para saber si puede disparar de nuevo o no
        if (Time.time >= tiempoSiguienteDisparo)
        {
            Disparar();

            // Se refresca y recalcula el tiempo para el siguiente disparo
            tiempoSiguienteDisparo = Time.time + ratioDisparo;
        }
    }

    // Genera el disparo en la posición de disparo.
    void Disparar()
    {
        Instantiate(balaPrefab, posicionDisparo.position, Quaternion.identity);
    }
}
