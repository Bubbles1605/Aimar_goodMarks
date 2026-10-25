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

    public void jugar()
    {
        StartCoroutine(RutinaJugar());
    }

    IEnumerator RutinaJugar()
    {
        if (animadorMenuPrincipal != null)
        {
            animadorMenuPrincipal.ResetTrigger("AnimacionCreditos");
            animadorMenuPrincipal.ResetTrigger("AnimacionSalir");
            animadorMenuPrincipal.SetTrigger("AnimacionJugar");
        }

        // Espera a que termine la animación antes de cambiar de escena
        yield return new WaitForSecondsRealtime(duracionAnimacionJugar);

        SceneManager.LoadScene("Clase");
    }

    public void creditos()
    {
        StartCoroutine(RutinaCreditos());
    }

    IEnumerator RutinaCreditos()
    {
        if (animadorMenuPrincipal != null)
        {
            animadorMenuPrincipal.ResetTrigger("AnimacionJugar");
            animadorMenuPrincipal.ResetTrigger("AnimacionSalir");
            animadorMenuPrincipal.SetTrigger("AnimacionCreditos");
        }

        // Espera a que termine la animación antes de cambiar a créditos
        yield return new WaitForSecondsRealtime(duracionAnimacionCreditos);

        SceneManager.LoadScene("Creditos");
    }

    public void salir()
    {
        StartCoroutine(RutinaSalir());
    }

    IEnumerator RutinaSalir()
    {
        if (animadorMenuPrincipal != null)
        {
            animadorMenuPrincipal.ResetTrigger("AnimacionJugar");
            animadorMenuPrincipal.ResetTrigger("AnimacionCreditos");
            animadorMenuPrincipal.SetTrigger("AnimacionSalir");
        }

        // Espera a que termine la animación antes de cerrar el juego
        yield return new WaitForSecondsRealtime(duracionAnimacionSalir);

        Application.Quit();
        Debug.Log("El juego se ha cerrado (Esto solo se ve en el editor).");
    }
}