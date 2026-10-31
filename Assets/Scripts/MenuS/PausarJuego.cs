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

    private bool accionEnProgreso = false;

    private void Update()
    {
        // Tecla Escape para pausar o reanudar mediante código
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (accionEnProgreso) return;

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
        if (accionEnProgreso) return;

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
        if (juegoPausado || accionEnProgreso) return;

        if (menuPausa != null)
        {
            menuPausa.SetActive(true);
        }

        juegoPausado = true;
        Time.timeScale = 0.0f;

        // --- NUEVO: CORTAR VOCES DEL ALUMNO ---
        // Buscamos al Director para saber qué alumno está en pantalla y le cortamos todos los sonidos
        ScriptDias director = FindFirstObjectByType<ScriptDias>();
        if (director != null && director.alumnoActual != null)
        {
            AudioSource[] reproductoresAlumno = director.alumnoActual.GetComponents<AudioSource>();
            foreach (AudioSource reproductor in reproductoresAlumno)
            {
                reproductor.Stop();
            }
        }

        StartCoroutine(RutinaPausar());
    }

    IEnumerator RutinaPausar()
    {
        accionEnProgreso = true;
        yield return null;
        yield return null;

        if (pausa != null) audioSource.PlayOneShot(pausa, 1.3f);

        if (animadorMenu != null)
        {
            animadorMenu.ResetTrigger("AnimacionOpcionA");
            animadorMenu.ResetTrigger("AnimacionOpcionB");
            animadorMenu.ResetTrigger("AnimacionOpcionC");
            animadorMenu.SetTrigger("TriggerEntrada");
        }

        yield return new WaitForSecondsRealtime(duracionAnimacionEntrada);

        accionEnProgreso = false;
    }

    public void Reanudar()
    {
        if (accionEnProgreso) return;
        StartCoroutine(RutinaReanudar());
    }

    IEnumerator RutinaReanudar()
    {
        accionEnProgreso = true;
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
        Time.timeScale = 1.0f;
        juegoPausado = false;

        accionEnProgreso = false;
    }

    public void jugar()
    {
        if (accionEnProgreso) return;
        StartCoroutine(RutinaJugar());
    }

    IEnumerator RutinaJugar()
    {
        accionEnProgreso = true;
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
        if (accionEnProgreso) return;
        StartCoroutine(RutinaOpcionC());
    }

    IEnumerator RutinaOpcionC()
    {
        accionEnProgreso = true;
        Time.timeScale = 1.0f;

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