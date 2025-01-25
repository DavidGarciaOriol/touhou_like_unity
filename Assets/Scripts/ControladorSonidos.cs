using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorSonidos : MonoBehaviour
{

    public static ControladorSonidos instance;

    private AudioSource recursoAudio;

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

        recursoAudio = GetComponent<AudioSource>();
    }

    public void ReproducirSonido(AudioClip sonido, float volumen, float pitch = 1f)
    {
        recursoAudio.pitch = pitch;
        recursoAudio.PlayOneShot(sonido, volumen);
    }
    
}
