using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspectRatio : MonoBehaviour
{
    public float targetAspect = 1200f / 1080f; // Relaci�n de aspecto deseada (4:3 en este caso)

    void Start()
    {
        // Calcular la relaci�n de aspecto actual
        float windowAspect = (float)Screen.width / Screen.height;

        // Comparar con la relaci�n objetivo
        float scaleHeight = windowAspect / targetAspect;

        // Ajustar la vista
        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            // A�adir barras negras horizontales
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            camera.rect = rect;
        }
        else
        {
            // A�adir barras negras verticales
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = camera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }
    }
}