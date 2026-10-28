using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PausarJuego : MonoBehaviour
{
    [Header("Configuración del Menú")]
    public GameObject menuPausa;
    public Animator animadorMenu;
    public float duracionAnimacionEntrada = 0.5f;
    public float duracionAnimacionSalida = 0.5f;
    public float duracionAnimacionSalidaMenu = 0.5f;
    public float duracionAnimacionOpcionC = 0.5f; // Duración para la nueva animación C

    [HideInInspector]
    public bool juegoPausado = false;
    [Header("Sonidos")]
  
    public AudioClip reanudar;
    public AudioClip pausa;
    public AudioClip reiniciar;
    public AudioClip menu;
    public AudioSource audioSource;


    private void Update()
    {
        // Tecla Escape para pausar o reanudar mediante código
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }
  
    // --- FUNCIÓN PÚBLICA PARA TU BOTÓN DE PAUSA ---
    public void PulsarBotonPausa()
    {
        if (juegoPausado)
        {
            Reanudar();
        }
        else
        {
            Pausar();
        }
    }

    public void Pausar()
    {
        if (juegoPausado) return;

        if (menuPausa != null)
        {
            menuPausa.SetActive(true);
        }

        juegoPausado = true;
        Time.timeScale = 0.0f;
        StartCoroutine(RutinaPausar());
    }

    IEnumerator RutinaPausar()
    {
        yield return null;
        yield return null;
        if (pausa != null) audioSource.PlayOneShot(pausa, 1.3f);

        if (animadorMenu != null)
        {
            animadorMenu.ResetTrigger("AnimacionOpcionA");
            animadorMenu.ResetTrigger("AnimacionOpcionB");
            animadorMenu.ResetTrigger("AnimacionOpcionC"); // Limpiamos la C por seguridad
            animadorMenu.SetTrigger("TriggerEntrada");
        }

        yield return new WaitForSecondsRealtime(duracionAnimacionEntrada);

    }

    public void Reanudar()
    {
        StartCoroutine(RutinaReanudar());
    }

    IEnumerator RutinaReanudar()
    {
        Time.timeScale = 1.0f;
        if (reanudar != null) audioSource.PlayOneShot(reanudar, 1.3f);

        if (animadorMenu != null)
        {
            animadorMenu.ResetTrigger("TriggerEntrada");
            animadorMenu.ResetTrigger("AnimacionOpcionA");
            animadorMenu.ResetTrigger("AnimacionOpcionC");
            animadorMenu.SetTrigger("AnimacionOpcionB");
        }

        yield return new WaitForSecondsRealtime(duracionAnimacionSalida);

        if (menuPausa != null)
        {
            menuPausa.SetActive(false);
        }

        juegoPausado = false;
    }

    public void jugar()
    {
        StartCoroutine(RutinaJugar());
    }

    IEnumerator RutinaJugar()
    {
        Time.timeScale = 1.0f;
        if (menu != null) audioSource.PlayOneShot(menu, 1.3f);
        if (animadorMenu != null)
        {
            animadorMenu.ResetTrigger("TriggerEntrada");
            animadorMenu.ResetTrigger("AnimacionOpcionB");
            animadorMenu.ResetTrigger("AnimacionOpcionC");
            animadorMenu.SetTrigger("AnimacionOpcionA");
        }

        yield return new WaitForSecondsRealtime(duracionAnimacionSalidaMenu);

        SceneManager.LoadScene("Menu");
    }

    public void OpcionC()
    {
        StartCoroutine(RutinaOpcionC());
    }

    IEnumerator RutinaOpcionC()
    {
        Time.timeScale = 1.0f; // Restauramos el tiempo por si estaba pausado
        if (reiniciar != null) audioSource.PlayOneShot(reiniciar, 1.3f);

        if (animadorMenu != null)
        {
            animadorMenu.ResetTrigger("TriggerEntrada");
            animadorMenu.ResetTrigger("AnimacionOpcionA");
            animadorMenu.ResetTrigger("AnimacionOpcionB");
            animadorMenu.SetTrigger("AnimacionOpcionC");
        }

        yield return new WaitForSecondsRealtime(duracionAnimacionOpcionC);

        SceneManager.LoadScene("Clase");
    }
}