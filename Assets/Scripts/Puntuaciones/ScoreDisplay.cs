using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public Transform listaPuntuacionParent;
    public GameObject textoBase;

    private void Start()
    {
        MostrarPuntuaciones();
    }

    public void MostrarPuntuaciones()
    {
        // Obtener las mejores puntuaciones desde el ScoreManager
        List<EntradaPuntuacion> mejoresPuntuaciones = ScoreManager.ObtenerLasMejoresPuntuaciones();

        // Limpiar lista anterior
        foreach (Transform child in listaPuntuacionParent)
        {
            Destroy(child.gameObject);
        }

        // A�adir cada puntuaci�n como un nuevo texto
        for (int i = 0; i < mejoresPuntuaciones.Count; i++)
        {
            var puntuacion = mejoresPuntuaciones[i];

            // Crear un nuevo texto para cada entrada
            GameObject textoNuevo = Instantiate(textoBase, listaPuntuacionParent);
            TextMeshProUGUI text = textoNuevo.GetComponent<TextMeshProUGUI>();

            if (text != null)
            {
                // A�adir la posici�n (i + 1) antes del nombre y la puntuaci�n
                text.text = $"{i + 1}. {puntuacion.nombreJugador} - {puntuacion.puntos}";
            }
        }
    }
}
