using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{

    [SerializeField] // Cooldown de la bomba
    float cooldownBomba = 45;

    [SerializeField] // Daño de la bomba
    int damageBomba = 100;

    [SerializeField] // Partículas de explosión de la bomba
    GameObject prefabParticulasOndaBomba;

    [SerializeField] // Tecla de uso de la bomba
    KeyCode teclaBomba = KeyCode.E;

    [SerializeField] // Botón del mando para usar la bomba
    private string botonMandoBomba = "ZR";

    // Si la bomba está disponible
    private bool bombaDisponible = true;

    // Clip audio bomba
    [SerializeField]
    AudioClip audioBomba;

    void Update()
    {
        // Detectar si se pulsa la tecla o botón del mando
        if (bombaDisponible && (Input.GetKeyDown(teclaBomba) || Input.GetButtonDown(botonMandoBomba)))
        {
            UsarBomba();
        }
    }

    private void UsarBomba()
    {
        // Desactiva la posibilidad de usar la bomba
        bombaDisponible = false;

        // Instanciar el efecto de la onda expansiva
        if (prefabParticulasOndaBomba != null)
        {
            Instantiate(prefabParticulasOndaBomba, transform.position, Quaternion.identity);
        }

        // Elimina a los enemigos y balas
        ControladorSonidos.instance.ReproducirSonido(audioBomba);
        LimpiarPantalla();

        // Reiniciar el cooldown
        StartCoroutine(ReiniciarCooldown());
    }

    private void LimpiarPantalla()
    {
        // Encuentra todos los objetos de tipo "Enemigo" y "Proyectil" en la escena y derrota / destruye
        Enemigo[] enemigos = FindObjectsOfType<Enemigo>();
        BalaEnemiga[] proyectiles = FindObjectsOfType<BalaEnemiga>();

        foreach (Enemigo enemigo in enemigos)
        {
            enemigo.RecibirDamage(damageBomba);
        }

        foreach (BalaEnemiga proyectil in proyectiles)
        {
            Destroy(proyectil.gameObject);
            GameManager.instance.AgregarPuntos(5);
        }
    }

    private IEnumerator ReiniciarCooldown()
    {
        yield return new WaitForSeconds(cooldownBomba);
        bombaDisponible = true;
    }
}
