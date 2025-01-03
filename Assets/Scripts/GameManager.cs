using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    private int puntuacion = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AgregarPuntos(int puntos)
    {
        puntuacion += puntos;
        AcrualizarPuntuacionUI();
    }

    public void RestarPuntos(int puntos)
    {
        puntuacion -= puntos;
        AcrualizarPuntuacionUI();
    }

    void AcrualizarPuntuacionUI()
    {
        ManagerUI.instance.ActualizarTextoPuntuacion(puntuacion);
    }
}
