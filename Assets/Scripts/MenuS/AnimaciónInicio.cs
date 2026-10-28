using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEditor.ShaderData;

public class AnimaciónInicio : MonoBehaviour
{
    [Header("Configuración")]
    public Animator animadorBotonOGeneral;
    public GameObject panelMenuPausa;
    public GameObject panelInicio;
    public float duracionAnimacionInicio = 2.0f;
    [Header("Sonidos")]
    public AudioClip inicio1;
    public AudioClip inicio2;
    public AudioSource audioSource;
    public AudioSource audioSource2;

    private void Start()
    {
        // Comprobamos si ya se vio la animación anteriormente (1 = sí, 0 = no)
        if (PlayerPrefs.GetInt("AnimacionInicioVista", 0) == 1)
        {
            // Si ya se vio, nos saltamos la animación y abrimos directamente el menú
            if (panelMenuPausa != null) panelMenuPausa.SetActive(true);
            if (panelInicio != null) panelInicio.SetActive(false);
            else gameObject.SetActive(false);
        }
    }

    public void ClickBotonInicio()
    {
        StartCoroutine(RutinaAnimacionInicio());
        if (inicio1 != null)
            audioSource.PlayOneShot(inicio1, 1.3f);
    }

    IEnumerator RutinaAnimacionInicio()
    {
        // 1. Marcamos en la memoria del juego que ya se ha visto la animación
        PlayerPrefs.SetInt("AnimacionInicioVista", 1);
        PlayerPrefs.Save(); // Guardamos los datos

        // 2. Activamos el trigger "inicio" en el Animator
        if (animadorBotonOGeneral != null)
        {
            animadorBotonOGeneral.SetTrigger("Inicio");
        }

        // 3. Esperamos a que la animación termine de reproducirse
        yield return new WaitForSecondsRealtime(duracionAnimacionInicio);

        // 4. Activamos el panel del menú de pausa
        if (panelMenuPausa != null)
        {

            panelMenuPausa.SetActive(true);
        }

        // 5. Desactivamos el panel de inicio actual
        if (panelInicio != null)
        {

            panelInicio.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}