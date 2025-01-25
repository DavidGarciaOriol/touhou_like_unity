using System;
using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    // Objeto prefab del la bala / disparo
    public GameObject balaPrefab;

    // Cadencia de disparo
    [SerializeField]
    float cadenciaBase = 0.2f;
    float cadenciaModificador = 0.2f;
    float cadenciaDisparoMinima = 0.075f;
    public float CadenciaModificador { get => cadenciaModificador; set => cadenciaModificador = value; }

    // Modificador de daño
    [SerializeField]
    int damageBase = 1;
    int damageModificador = 1;
    int damageMaximo = 5;
    public int DamageModificador { get => damageModificador; set => damageModificador = value; }

    // Atraviesa
    [SerializeField]
    bool penetracion = false;
    public bool Penetracion { get => penetracion; set => penetracion = value; }
    
    // Posición de salida de la bala / disparo
    [SerializeField]
    Transform posicionDisparo;

    // Posiciones disponibles de disparo
    /*[SerializeField]
    int numeroPosicionesDisparoBase = 1;
    int numeroPosicionesDisparoActual = 1;
    int numeroPosicionesDisparoMaxima = 5;
    public int NumeroPosicionesDisparoActual { get => numeroPosicionesDisparoActual; set => numeroPosicionesDisparoActual = value; }
    */

    // Temporizador
    float tiempoSiguienteDisparo;

    // Clip de sonido
    [SerializeField]
    AudioClip audioDisparo;

    void Start()
    {
        CadenciaModificador = cadenciaBase;
    }

    void Update()
    {
        // Se calcula el ratio de disparo para saber si puede disparar de nuevo o no
        if (Time.time >= tiempoSiguienteDisparo && GetComponentInParent<Jugador>().puedeMoverse)
        {
            Disparar();

            // Se refresca y recalcula el tiempo para el siguiente disparo
            tiempoSiguienteDisparo = Time.time + CadenciaModificador;
        }
    }

    // Genera el disparo en la posición de disparo.
    void Disparar()
    {
        GameObject nuevaBala = Instantiate(balaPrefab, posicionDisparo.position, Quaternion.identity);
        BalaJugador balaScript = nuevaBala.GetComponent<BalaJugador>();

        if (balaScript != null)
        {
            balaScript.DamageModificador = DamageModificador;
            balaScript.Penetracion = Penetracion;
        }
    }

    // Disminuye la cadencia de disparo
    public void MejorarCadencia(float mejora)
    {
        if (cadenciaModificador > cadenciaDisparoMinima)
        {
            CadenciaModificador -= mejora;
        }
        else
        {
            cadenciaModificador = cadenciaDisparoMinima;
        }
        ActualizarUICadencia();
    }

    // Restaura la cadencia al valor base
    public void ReiniciarCadencia()
    {
        CadenciaModificador = cadenciaBase;
        ActualizarUICadencia();
    }

    // Mejora el daño del disparo
    public void MejorarDamage(int mejora)
    {
        if (damageModificador < damageMaximo)
        {
            DamageModificador += mejora;
        }
        else
        {
            DamageModificador = damageMaximo;
        }
        ActualizarUIPoder();
    }

    // Restaura el daño al valor base
    public void ReiniciarDamage()
    {
        DamageModificador = damageBase;
        ActualizarUIPoder();
    }

    // Cambia el estado de las balas, si perforan o no, por parámetro
    public void CambiarPenetracion(bool condicion)
    {
        Penetracion = condicion;
        ActualizarUIPerforacion(Penetracion);
    }

    void ActualizarUIPoder()
    {
        ManagerUI.instance.ActualizarTextoPoder(damageModificador);
    }

    void ActualizarUICadencia()
    {
        ManagerUI.instance.ActualizarTextoCadencia(cadenciaModificador);
    }

    void ActualizarUIPerforacion(bool perforando)
    {
        ManagerUI.instance.ActualizarTextoPerforacion(perforando);
    }

}
