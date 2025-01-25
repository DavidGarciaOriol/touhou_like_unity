using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManagerUI : MonoBehaviour
{

    public static ManagerUI instance;
    public TextMeshProUGUI textoPuntuacion;
    public TextMeshProUGUI textoVidas;
    public TextMeshProUGUI textoGraze;
    public TextMeshProUGUI textoPoder;
    public TextMeshProUGUI textoCadencia;
    public TextMeshProUGUI textoBombaCooldown;
    public TextMeshProUGUI textoPerforacion;

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

    public void ActualizarTextoPuntuacion(int nuevaPuntuacion)
    {
        textoPuntuacion.text = "Puntos: " + nuevaPuntuacion;
    }
    public void ActualizarTextoVidas(int nuevoNumeroVidas)
    {
        textoVidas.text = "Vidas: " + nuevoNumeroVidas;
    }
    public void ActualizarTextoGraze(int nuevoConteoGrazing)
    {
        textoGraze.text = "Graze: " + nuevoConteoGrazing;
    }
    public void ActualizarTextoBombaCooldown(float nuevoCooldown)
    {
        bool cooldownReiniciado = nuevoCooldown <= 0;
        if (cooldownReiniciado) textoBombaCooldown.text = "Bomba: ¡Listo!";
        else textoBombaCooldown.text = "Bomba: " + nuevoCooldown;
    }
    public void ActualizarTextoPoder(int nuevoPoder)
    {
        textoPoder.text = "Poder: " + nuevoPoder;
    }
    public void ActualizarTextoCadencia(float nuevaCadencia)
    {
        textoCadencia.text = "Cadencia: " + Math.Round(nuevaCadencia, 3);
    }

    public void ActualizarTextoPerforacion(bool perforando)
    {
        if (perforando) textoPerforacion.text = "Perforando: Sí";
        else textoPerforacion.text = "Perforando: No";
    }
}
