using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject panelPantallaPuntuaciones;
    public GameObject panelPantallaTutorial;

    public void Salir()
    {
        Application.Quit();
    }

    public void MostrarPantallaPuntuaciones()
    {
        panelPantallaPuntuaciones.SetActive(true);
    }

    public void OcultarPantallaPuntuaciones()
    {
        panelPantallaPuntuaciones.SetActive(false);
    }

    public void MostrarPantallaTutorial()
    {
        panelPantallaTutorial.SetActive(true);
    }

    public void OcultarPantallaTutorial() 
    {
        panelPantallaTutorial.SetActive(false);
    }
}
