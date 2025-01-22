using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{

    [SerializeField] // Cooldown de la bomba
    float cooldownBomba = 45;

    [SerializeField] // Part�culas de explosi�n de la bomba
    GameObject prefabParticulasOndaBomba;

    [SerializeField] // Tecla de uso de la bomba
    KeyCode teclaBomba = KeyCode.E;

    [SerializeField] // Bot�n del mando para usar la bomba
    private string botonMandoBomba = "ZR";

    // Si la bomba est� disponible
    private bool bombaDisponible = true;

    void Update()
    {
        // Detectar si se pulsa la tecla o bot�n del mando
        if (bombaDisponible && (Input.GetKeyDown(teclaBomba) || Input.GetButtonDown(botonMandoBomba)))
        {
            UsarBomba();
        }
    }

    private void UsarBomba()
    {
        // Desactiva la posibilidad de usar la bomba
        bombaDisponible = false;

        // Instanciar el efecto de la onda expansiva
        if (prefabParticulasOndaBomba != null)
        {
            Instantiate(prefabParticulasOndaBomba, transform.position, Quaternion.AngleAxis(-90, Vector3.right));
        }

        // Reiniciar el cooldown
        StartCoroutine(ReiniciarCooldown());
    }

    private IEnumerator ReiniciarCooldown()
    {
        yield return new WaitForSeconds(cooldownBomba);
        bombaDisponible = true;
    }
}
