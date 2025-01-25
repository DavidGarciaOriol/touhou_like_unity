using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordInput : MonoBehaviour
{   
    // Campo para mostrar el nombre actual
    public TextMeshProUGUI nombreInput;

    // Botón confirmar
    public Button botonConfirmar;

    // Panel del teclado
    public GameObject panel;

    // Botón de acceso a la pantalla para guardar puntuación
    // Para prevenir guardar dos veces
    public Button botonAbrirPantallaGuardarPuntuacion;

    // Texto con puntuacion
    public TextMeshProUGUI textoPuntuacion;

    private string nombreActual = "";
    private int longitudMaxima = 5;
    private int puntuacionActual;

    public void PrepararRecordInput(int puntuacion)
    {
        puntuacionActual = puntuacion;
        textoPuntuacion.text = "Puntos: " + puntuacion;
        nombreActual = "";
        nombreInput.text = "";
        panel.SetActive(true);
    }

    public void AgregarLetra(string letra)
    {
        if (nombreActual.Length < longitudMaxima)
        {
            nombreActual += letra;
            nombreInput.text = nombreActual;
        }
    }

    public void BorrarLetra()
    {
        if (nombreActual.Length > 0)
        {
            nombreActual = nombreActual.Substring(0, nombreActual.Length - 1);
            nombreInput.text = nombreActual;
        }
    }

    public void ConfirmarNombre()
    {
        if (nombreActual.Length > 0)
        {
            Debug.Log($"Guardando puntuación: {puntuacionActual} para el jugador: {nombreActual}");

            // Guardar el récord y cerrar el panel
            ScoreManager.AgregarPuntuacion(nombreActual, puntuacionActual);
            botonAbrirPantallaGuardarPuntuacion.interactable = false;
            panel.SetActive(false);
        }
    }
}
