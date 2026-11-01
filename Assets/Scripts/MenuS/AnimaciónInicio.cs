using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

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
        if (PlayerPrefs.GetInt("AnimacionInicioVista", 0) == 1)
        {
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
        PlayerPrefs.SetInt("AnimacionInicioVista", 1);
        PlayerPrefs.Save(); 

        if (animadorBotonOGeneral != null)
        {
            animadorBotonOGeneral.SetTrigger("Inicio");
        }

        yield return new WaitForSecondsRealtime(duracionAnimacionInicio);

        if (panelMenuPausa != null)
        {

            panelMenuPausa.SetActive(true);
        }

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