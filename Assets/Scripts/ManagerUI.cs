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
        textoPuntuacion.text = "Score: " + nuevaPuntuacion;
    }

    public void ActualizarTextoVidas(int nuevoNumeroVidas)
    {
        textoVidas.text = "Vidas: " + nuevoNumeroVidas;
    }

    public void ActualizarTextoGraze(int nuevoConteoGrazing)
    {
        textoGraze.text = "Graze: " + nuevoConteoGrazing;
    }
}
