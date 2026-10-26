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
        StartCoroutine(RutinaPausar());
    }

    IEnumerator RutinaPausar()
    {
        yield return null;
        yield return null;

        if (animadorMenu != null)
        {
            animadorMenu.ResetTrigger("AnimacionOpcionA");
            animadorMenu.ResetTrigger("AnimacionOpcionB");
            animadorMenu.ResetTrigger("AnimacionOpcionC"); // Limpiamos la C por seguridad
            animadorMenu.SetTrigger("TriggerEntrada");
        }

        yield return new WaitForSecondsRealtime(duracionAnimacionEntrada);
        Time.timeScale = 0.0f;
    }

    public void Reanudar()
    {
        StartCoroutine(RutinaReanudar());
    }

    IEnumerator RutinaReanudar()
    {
        Time.timeScale = 1.0f;

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

    // --- NUEVA FUNCIÓN PLACEHOLDER PARA LA OPCIÓN C ---
    public void OpcionC()
    {
        StartCoroutine(RutinaOpcionC());
    }

    IEnumerator RutinaOpcionC()
    {
        Time.timeScale = 1.0f; // Restauramos el tiempo por si estaba pausado

        if (animadorMenu != null)
        {
            // Limpiamos los demás triggers y activamos el de la opción C
            animadorMenu.ResetTrigger("TriggerEntrada");
            animadorMenu.ResetTrigger("AnimacionOpcionA");
            animadorMenu.ResetTrigger("AnimacionOpcionB");
            animadorMenu.SetTrigger("AnimacionOpcionC");
        }

        // Esperamos a que termine la animación de esta opción
        yield return new WaitForSecondsRealtime(duracionAnimacionOpcionC);

        // TODO: Aquí podrás escribir lo que quieras que haga después (cambiar de escena, abrir ajustes, etc.)
        Debug.Log("¡Animación de la Opción C terminada! Aquí puedes añadir tu código futuro.");
    }
}