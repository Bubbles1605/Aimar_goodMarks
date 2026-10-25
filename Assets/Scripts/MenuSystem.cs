using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    public void jugar()
    {
        StartCoroutine(RutinaJugar());
    }

    IEnumerator RutinaJugar()
    {
        yield return null;

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

        if (animadorMenuPrincipal != null)
        {
            animadorMenuPrincipal.ResetTrigger("AnimacionJugar");
            animadorMenuPrincipal.ResetTrigger("AnimacionSalir");
            animadorMenuPrincipal.ResetTrigger("TriggerVolver");
            animadorMenuPrincipal.SetTrigger("AnimacionCreditos");
        }

        yield return new WaitForSecondsRealtime(duracionAnimacionCreditos);

        // EN VEZ DE CAMBIAR DE ESCENA, ACTIVAMOS EL PANEL DE CRÉDITOS
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

        if (animadorMenuPrincipal != null)
        {
            animadorMenuPrincipal.ResetTrigger("AnimacionJugar");
            animadorMenuPrincipal.ResetTrigger("AnimacionCreditos");
            animadorMenuPrincipal.ResetTrigger("TriggerVolver");
            animadorMenuPrincipal.SetTrigger("AnimacionSalir");
        }

        yield return new WaitForSecondsRealtime(duracionAnimacionSalir);

        Application.Quit();
        Debug.Log("El juego se ha cerrado.");
    }

    // --- NUEVA FUNCIÓN PARA EL BOTÓN 'X' ---
    public void CerrarCreditos()
    {
        // 1. Apagamos el panel de créditos
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(false);
        }

        // 2. Mandamos la orden al Animator de volver al estado inicial
        if (animadorMenuPrincipal != null)
        {
            animadorMenuPrincipal.ResetTrigger("AnimacionCreditos");
            animadorMenuPrincipal.SetTrigger("TriggerVolver");
        }
    }
}