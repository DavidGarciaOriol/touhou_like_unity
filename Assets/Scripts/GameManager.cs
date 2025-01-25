using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Instancia del GameManager
    public static GameManager instance;

    // Parámetros por defecto que se muestran en la escena para vidas, puntos y graze.
    private int puntuacion = 0;
    private int vidasReimu = 3;
    private int contadorGraze = 0;

    public GameObject panelGameOver;
    public GameObject panelGuardarPuntuacion;

    public TextMeshProUGUI puntuacionFinal;

    
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
        panelGameOver.SetActive(false);
    }

    // Suma puntos al jugador en la partida
    public void AgregarPuntos(int puntos)
    {
        puntuacion += puntos;
        ActualizarPuntuacionUI();
    }

    // Resta puntos al jugador en la partida
    public void RestarPuntos(int puntos)
    {
        if (puntuacion - puntos < 0)
        {
            puntuacion = 0;
        }
        else
        {
            puntuacion -= puntos;
        }
        ActualizarPuntuacionUI();
    }

    // Calcula puntos perdidos al perder una vida el juagdor
    public int CalcularPuntosPerdidos()
    {
        int puntosPerdidos = 0;
        if (puntuacion > 0)
        {
            puntosPerdidos = Mathf.RoundToInt(puntuacion * 0.2f);
            RestarPuntos(puntosPerdidos);
        }
        return puntosPerdidos; 
    }

    public void RestarVidas()
    {
        vidasReimu --;
        if (vidasReimu < 0) vidasReimu = 0;

        ActualizarVidasUI();
        VerificarGameOver();
    }

    public void SumarVidas()
    {
        vidasReimu ++;
        ActualizarVidasUI();
    }

    public void SumarContadorGraze()
    {
        contadorGraze ++;
        ActualizarGrazingUI();
    }

    private void VerificarGameOver()
    {
        if (vidasReimu <= 0)
        {
            puntuacionFinal.text = "Score: " + puntuacion;
            panelGameOver.SetActive(true);
            Destroy(Bomba.instance.gameObject);
            Time.timeScale = 0; // Pausar el juego
        }
    }

    public void MostrarPanelGuardarPuntuacion()
    {
        panelGuardarPuntuacion.SetActive(true);
        panelGuardarPuntuacion.GetComponent<RecordInput>().PrepararRecordInput(puntuacion);
    }

    void ActualizarPuntuacionUI()
    {
        ManagerUI.instance.ActualizarTextoPuntuacion(puntuacion);
    }

    void ActualizarVidasUI()
    {
        ManagerUI.instance.ActualizarTextoVidas(vidasReimu);
    }

    void ActualizarGrazingUI()
    {
        ManagerUI.instance.ActualizarTextoGraze(contadorGraze);
    }

    /*public void ReiniciarJuego()
    {
        // Reinicio de variables y tiempo del juego
        ReiniciarEstado();

        // Actualizar UIs
        ActualizarVidasUI();
        ActualizarPuntuacionUI();
        ActualizarGrazingUI();

        // Ocultar pantalla de Game Over
        panelGameOver.SetActive(false);

        // Reiniciar escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReiniciarEstado()
    {
        vidasReimu = 3;
        puntuacion = 0;
        contadorGraze = 0;

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }

        Time.timeScale = 1; // Asegúrate de reanudar el tiempo
    }*/
}
