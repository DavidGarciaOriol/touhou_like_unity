using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject panelPantallaPuntuaciones;

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

}
