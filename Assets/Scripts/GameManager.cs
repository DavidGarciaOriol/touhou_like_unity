using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    private int puntuacion = 0;
    private int vidasReimu = 3;

    public GameObject panelGameOver;

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

    // Suma puntosal jugador en la partida
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

    public void RestarVidas()
    {
        vidasReimu --;
        ActualizarVidasUI();
        VerificarGameOver();
    }

    public void SumarVidas()
    {
        vidasReimu ++;
        ActualizarVidasUI();
    }

    private void VerificarGameOver()
    {
        if (vidasReimu <= 0)
        {
            puntuacionFinal.text = "SCORE:" + puntuacion;
            panelGameOver.SetActive(true);
            Time.timeScale = 0; // Pausar el juego
        }
    }

    void ActualizarPuntuacionUI()
    {
        ManagerUI.instance.ActualizarTextoPuntuacion(puntuacion);
    }

    void ActualizarVidasUI()
    {
        ManagerUI.instance.ActualizarTextoVidas(vidasReimu);
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1; // Reanudar el tiempo

        // Restablecer variables
        vidasReimu = 3;
        puntuacion = 0;

        // Actualizar UI
        ActualizarVidasUI();
        ActualizarPuntuacionUI();

        // Ocultar pantalla de Game Over
        panelGameOver.SetActive(false);

        // Reiniciar escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirDelJuego()
    {
        #if UNITY_EDITOR
            // En el editor, detiene el modo de juego.
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // En una compilación, cierra la aplicación.
            Application.Quit();
        #endif
    }
}
