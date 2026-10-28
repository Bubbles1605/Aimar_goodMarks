using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderData;

public class MenuSystem : MonoBehaviour
{
    [Header("Configuración del Animator del Menú")]
    public Animator animadorMenuPrincipal;
    public float duracionAnimacionJugar = 0.5f;
    public float duracionAnimacionCreditos = 0.5f;
    public float duracionAnimacionSalir = 0.5f;

    [Header("Configuración de Créditos")]
    [Tooltip("Arrastra aquí el panel que tiene el texto de créditos y el botón X")]
    public GameObject panelCreditos;
    [Header("Sonidos")]
    public AudioClip Jugar;
    public AudioClip creditos_entrar;
    public AudioClip Salir;
    public AudioClip Cerrar_creditos;
    public AudioSource audioSource;
    public AudioSource audioSource2;

    private void Start()
    {
        audioSource2.Play();
    }
    public void jugar()
    {
        StartCoroutine(RutinaJugar());
    }

    IEnumerator RutinaJugar()
    {
        yield return null;
        if (Jugar != null) audioSource.PlayOneShot(Jugar, 1.3f);
        if (animadorMenuPrincipal != null)
        {
            animadorMenuPrincipal.ResetTrigger("AnimacionCreditos");
            animadorMenuPrincipal.ResetTrigger("AnimacionSalir");
            animadorMenuPrincipal.ResetTrigger("TriggerVolver");
            animadorMenuPrincipal.SetTrigger("AnimacionJugar");
        }

        yield return new WaitForSecondsRealtime(duracionAnimacionJugar);
        SceneManager.LoadScene("Clase");
    }

    public void creditos()
    {
        StartCoroutine(RutinaCreditos());
    }

    IEnumerator RutinaCreditos()
    {
        yield return null;
        if (creditos_entrar != null) audioSource.PlayOneShot(creditos_entrar, 1.3f);

        if (animadorMenuPrincipal != null)
        {
            animadorMenuPrincipal.ResetTrigger("AnimacionJugar");
            animadorMenuPrincipal.ResetTrigger("AnimacionSalir");
            animadorMenuPrincipal.ResetTrigger("TriggerVolver");
            animadorMenuPrincipal.SetTrigger("AnimacionCreditos");
        }

        yield return new WaitForSecondsRealtime(duracionAnimacionCreditos);

        if (panelCreditos != null)
        {
            panelCreditos.SetActive(true);
        }
    }

    public void salir()
    {
        StartCoroutine(RutinaSalir());
    }

    IEnumerator RutinaSalir()
    {
        yield return null;
        if (Salir != null) audioSource.PlayOneShot(Salir, 1.3f);
        // --- REINICIAMOS LA ANIMACIÓN DE INICIO ---
        // Ponemos el valor a 0 para que la próxima vez vuelva a mostrarse la intro
        PlayerPrefs.SetInt("AnimacionInicioVista", 0);
        PlayerPrefs.Save();

        if (animadorMenuPrincipal != null)
        {
            animadorMenuPrincipal.ResetTrigger("AnimacionJugar");
            animadorMenuPrincipal.ResetTrigger("AnimacionCreditos");
            animadorMenuPrincipal.ResetTrigger("TriggerVolver");
            animadorMenuPrincipal.SetTrigger("AnimacionSalir");
        }

        yield return new WaitForSecondsRealtime(duracionAnimacionSalir);

        Application.Quit();
        Debug.Log("El juego se ha cerrado y se ha reiniciado la intro para la próxima vez.");
    }

    public void CerrarCreditos()
    {
        if (Cerrar_creditos != null) audioSource.PlayOneShot(Cerrar_creditos, 1.3f);

        if (panelCreditos != null)
        {
            panelCreditos.SetActive(false);
        }

        if (animadorMenuPrincipal != null)
        {
            animadorMenuPrincipal.ResetTrigger("AnimacionCreditos");
            animadorMenuPrincipal.SetTrigger("TriggerVolver");
        }
    }
}