using System.Collections;
using UnityEngine;

public class ScriptDias : MonoBehaviour
{
    [Header("Alumno en Pantalla (Se asigna solo)")]
    public ScriptAlumno alumnoActual;

    private bool dialogoEnCurso = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogoEnCurso && alumnoActual != null)
            {
                alumnoActual.avanzarTexto = true;
                return;
            }

            Vector2 posRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] hits = Physics2D.OverlapPointAll(posRaton);

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("BotonHablar"))
                {
                    // Como el alumno se ha auto-asignado al aparecer, el Director ya sabe quién es
                    if (alumnoActual != null && !dialogoEnCurso)
                    {
                        if (!alumnoActual.primerDialogoDicho)
                        {
                            StartCoroutine(ManejarDialogo(alumnoActual.Decir(alumnoActual.dialogoPrincipal)));
                            alumnoActual.primerDialogoDicho = true;
                        }
                        else
                        {
                            StartCoroutine(ManejarDialogo(alumnoActual.DecirAleatorio(alumnoActual.frasesGenericas)));
                        }
                    }
                    break;
                }
            }
        }
    }

    private IEnumerator ManejarDialogo(IEnumerator corrutinaHablar)
    {
        dialogoEnCurso = true;
        yield return StartCoroutine(corrutinaHablar);
        dialogoEnCurso = false;
    }
}