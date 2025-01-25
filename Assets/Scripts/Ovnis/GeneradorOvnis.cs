using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorOvnis : MonoBehaviour
{
    [SerializeField]
    GameObject[] ovniPrefabs;

    [SerializeField]
    float[] pesosDeOvnis;

    [SerializeField]
    Transform[] listaPuntosGeneracionOvnis;

    [SerializeField]
    float intervaloDeGeneracion = 20f;

    [SerializeField] // Referencia al script del disparo del jugador
    DisparoJugador disparoJugador; 

    private float tiempoSiguienteGeneracion;

    void Update()
    {
        if (Time.time >= tiempoSiguienteGeneracion)
        {
            GenerarOvni();
            tiempoSiguienteGeneracion = Time.time + intervaloDeGeneracion;
        }
    }

    // Funci�n para escoger un ovni con peso
    /**
    * Esta funci�n la verdad es que ten�a m�s o menos idea de lo que
    * ten�a que hacer pero le ped� a GPT que me hiciera el algoritmo
    * porque no lo ten�a del todo claro a la hora de implementarlo.
    * He de decir que, una vez sobre el papel y entendiendo el enfoque
    * de hacer una copia del array de pesos para iterar sobre ella
    * y redistribuir los pesos de los objetos cuyas stats est�n al
    * m�ximo hacia los puntos en la copia para poder usar la original
    * en caso de perder las mejoras, etc... Es un acercamiento brillante.
    */
    int EscogerOvniConPeso()
    {
        // Crear una copia temporal de los pesos
        float[] pesosAjustados = (float[])pesosDeOvnis.Clone();

        // Referencia al �ndice del ovni de puntos (�ltimo en la lista)
        int indiceOvniPuntos = ovniPrefabs.Length - 1;

        // Ajustar los pesos seg�n las estad�sticas del jugador
        if (disparoJugador.DamageModificador >= 5)
        {
            // Si el da�o est� al m�ximo, transferir peso al ovni de puntos
            pesosAjustados[indiceOvniPuntos] += pesosAjustados[0];
            pesosAjustados[0] = 0f; // Eliminar peso del ovni de poder
        }
        if (disparoJugador.CadenciaModificador <= 0.075f)
        {
            // Si la cadencia est� al m�ximo, transferir peso al ovni de puntos
            pesosAjustados[indiceOvniPuntos] += pesosAjustados[2];
            pesosAjustados[2] = 0f; // Eliminar peso del ovni de cadencia
        }
        if (disparoJugador.Penetracion)
        {
            // Si la perforaci�n est� activada, transferir peso al ovni de puntos
            pesosAjustados[indiceOvniPuntos] += pesosAjustados[3];
            pesosAjustados[3] = 0f; // Eliminar peso del ovni de perforaci�n
        }

        // Normalizar los pesos restantes para que sigan sumando correctamente
        float sumaPesos = 0f;
        foreach (float peso in pesosAjustados)
        {
            sumaPesos += peso;
        }

        if (sumaPesos == 0f)
        {
            // Si algo sali� mal (todos los pesos son 0), devolver el ovni de puntos
            return indiceOvniPuntos;
        }

        // Generar un n�mero aleatorio dentro del rango de la suma de pesos
        float valorAleatorio = Random.Range(0f, sumaPesos);
        float acumulado = 0f;

        for (int i = 0; i < pesosAjustados.Length; i++)
        {
            acumulado += pesosAjustados[i];
            if (valorAleatorio < acumulado)
            {
                return i;
            }
        }

        // Si algo sale mal, devolver el �ltimo ovni (ovni de puntos)
        return indiceOvniPuntos;
    }


    // Genera un ovni y decide su punto de generacion
    void GenerarOvni()
    {
        int indicePosicionSalidaAleatoria = Random.Range(0, listaPuntosGeneracionOvnis.Length);
        Vector2 posicionSeleccionada = listaPuntosGeneracionOvnis[indicePosicionSalidaAleatoria].position;

        // Escoge un ovni respecto a sus pesos
        int ovniElegidoIndex = EscogerOvniConPeso();

        GameObject nuevoEnemigo = Instantiate(ovniPrefabs[ovniElegidoIndex], posicionSeleccionada, Quaternion.identity);
    }
}
