using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class EntradaPuntuacion
{
    public string nombreJugador;
    public int puntos;

    public EntradaPuntuacion(string playerName, int score)
    {
        this.nombreJugador = playerName;
        this.puntos = score;
    }
}

[Serializable]
public class DatosPuntuacion
{
    public List<EntradaPuntuacion> puntuaciones = new List<EntradaPuntuacion>();
}

public class ScoreManager : MonoBehaviour
{
    private static string rutaArchivo;
    private static DatosPuntuacion datosPuntuacion;

    private void Awake()
    {
        // Ruta al archivo persistente con los datos de jugadores.
        rutaArchivo = Path.Combine(Application.persistentDataPath, "puntuaciones.json");

        // Depuración: Imprimir la ruta en consola.
        Debug.Log($"Ruta del archivo de puntuaciones: {rutaArchivo}");

        // Cargar datos al iniciar
        CargarPuntuaciones();
    }

    public static void AgregarPuntuacion(string nombreJugador, int puntuacion)
    {
        if (datosPuntuacion == null)
        {
            datosPuntuacion = new DatosPuntuacion();
        }

        // Agregar nueva puntuación
        datosPuntuacion.puntuaciones.Add(new EntradaPuntuacion(nombreJugador, puntuacion));

        // Ordena de mayor a menor puntuación. Guarda las 10 mejores puntuaciones.
        datosPuntuacion.puntuaciones.Sort((a, b) => b.puntos.CompareTo(a.puntos));
        if (datosPuntuacion.puntuaciones.Count > 10)
        {
            datosPuntuacion.puntuaciones = datosPuntuacion.puntuaciones.GetRange(0, 10);
        }

        GuardarPuntuaciones();
    }

    // Obtiene las 10 mejores puntuaciones
    public static List<EntradaPuntuacion> ObtenerLasMejoresPuntuaciones()
    {
        if (datosPuntuacion == null)
        {
            CargarPuntuaciones();
        }

        return datosPuntuacion.puntuaciones;
    }

    // Carga todas las puntuaciones del JSON
    private static void CargarPuntuaciones()
    {
        if (File.Exists(rutaArchivo))
        {
            string archivoJson = File.ReadAllText(rutaArchivo);
            datosPuntuacion = JsonUtility.FromJson<DatosPuntuacion>(archivoJson);
        }
        else
        {
            datosPuntuacion = new DatosPuntuacion();
        }
    }

    // Guarda las puntuaciones en el JSON
    private static void GuardarPuntuaciones()
    {
        if (string.IsNullOrEmpty(rutaArchivo))
        {
            Debug.LogError("La ruta del archivo de puntuaciones no está inicializada.");
            return;
        }

        try
        {
            string archivoJson = JsonUtility.ToJson(datosPuntuacion, true);
            File.WriteAllText(rutaArchivo, archivoJson);
            Debug.Log("Puntuaciones guardadas correctamente.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al guardar puntuaciones: {e.Message}");
        }
    }

}
