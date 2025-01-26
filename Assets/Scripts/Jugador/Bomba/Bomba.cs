using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bomba : MonoBehaviour
{

    public static Bomba instance;

    [SerializeField] // Cooldown de la bomba
    float cooldownBomba = 45;

    [SerializeField] // Daño de la bomba
    int damageBomba = 100;

    [SerializeField] // Partículas de explosión de la bomba
    GameObject prefabParticulasOndaBomba;

    [SerializeField] // Tecla de uso de la bomba
    KeyCode teclaBomba = KeyCode.E;

    // Clip audio bomba
    [SerializeField]
    AudioClip audioBombaExplosion;

    [SerializeField]
    AudioClip audioBombaLista;

    // Si la bomba está disponible
    private bool bombaDisponible = true;
    private float tiempoRestanteCooldown = 0f;

    // Si el jugador está muerto (para no tirarla en caso contrario)
    private bool jugadorMuerto;
    public bool JugadorMuerto { get => jugadorMuerto; set => jugadorMuerto = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Detectar si se pulsa la tecla o botón del mando
        if (!JugadorMuerto && bombaDisponible && (Input.GetKey(teclaBomba) || Input.GetAxis("ZR") > 0.1f))
        {
            UsarBomba();
        }

        // Actualizar el temporizador en la UI
        if (!bombaDisponible)
        {
            ActualizarTemporizadorUI();
        }
        else if (tiempoRestanteCooldown == 0)
        {
            ActualizarTemporizadorUI();
        }
    }

    void ActualizarTemporizadorUI()
    {
        ManagerUI.instance.ActualizarTextoBombaCooldown(tiempoRestanteCooldown);
    }

    private void UsarBomba()
    {
        // Desactiva la posibilidad de usar la bomba
        bombaDisponible = false;

        // Reiniciar el tiempo restante del cooldown
        tiempoRestanteCooldown = cooldownBomba;

        // Instanciar el efecto de la onda expansiva
        if (prefabParticulasOndaBomba != null)
        {
            Instantiate(prefabParticulasOndaBomba, transform.position, Quaternion.identity);
        }

        // Elimina a los enemigos y balas
        ControladorSonidos.instance.ReproducirSonido(audioBombaExplosion, 0.65f);
        LimpiarPantalla();

        // Reiniciar el cooldown
        StartCoroutine(IniciarCooldown());
    }

    private void LimpiarPantalla()
    {
        // Encuentra todos los objetos de tipo "Enemigo" y "Proyectil" en la escena y derrota / destruye
        Enemigo[] enemigos = FindObjectsOfType<Enemigo>();
        BalaEnemiga[] proyectiles = FindObjectsOfType<BalaEnemiga>();
        OvniPortaItem[] ovnis = FindObjectsOfType<OvniPortaItem>();

        foreach (Enemigo enemigo in enemigos)
        {
            enemigo.RecibirDamage(damageBomba);
        }

        foreach (BalaEnemiga proyectil in proyectiles)
        {
            Destroy(proyectil.gameObject);
            GameManager.instance.AgregarPuntos(5);
        }

        foreach (OvniPortaItem ovni in ovnis)
        {
            ovni.RecibirDamage(damageBomba);
        }
    }

    private IEnumerator IniciarCooldown()
    {
        while (tiempoRestanteCooldown > 0f)
        {
            yield return new WaitForSeconds(1f);
            tiempoRestanteCooldown -= 1f;
        }

        // Reiniciar el tiempo restante
        tiempoRestanteCooldown = 0f; 
        bombaDisponible = true;
        ControladorSonidos.instance.ReproducirSonido(audioBombaLista, 1f);
    }

    public void RestarTiempoCooldown(float cantidad)
    {
        // Reduce el tiempo restante del cooldown
        tiempoRestanteCooldown -= cantidad;

        // Evitar valores negativos
        if (tiempoRestanteCooldown < 0f)
        {
            // Si el cooldown llega a cero, activar la bomba disponible
            tiempoRestanteCooldown = 0f;
            bombaDisponible = true;
            // ControladorSonidos.instance.ReproducirSonido(audioBombaLista, 1f);
        }
    }
}
