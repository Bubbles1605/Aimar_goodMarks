using System.Collections;
using UnityEngine;

public class ScriptDias : MonoBehaviour
{
    [Header("Gestión del Día")]
    public ScriptAlumno alumnoInicial;

    [Header("Alumno en Pantalla")]
    public ScriptAlumno alumnoActual;

    private bool dialogoEnCurso = false;

    void Start()
    {
        if (alumnoInicial != null)
        {
            StartCoroutine(SecuenciaAlumno(alumnoInicial));
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Avanzar texto rápido con clic en pantalla
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
                    // Como el botón solo aparece tras la secuencia principal, 
                    // cualquier clic en él lanzará SIEMPRE las frases aleatorias.
                    if (alumnoActual != null && !dialogoEnCurso)
                    {
                        StartCoroutine(ManejarDialogo(alumnoActual.DecirAleatorio(alumnoActual.frasesGenericas)));
                    }
                    break;
                }
            }
        }
    }

    // EL NUEVO ORDEN CINEMÁTICO
    public IEnumerator SecuenciaAlumno(ScriptAlumno alumno)
    {
        alumnoActual = alumno;

        // 1. Sube a la mesa
        yield return StartCoroutine(alumno.Aparecer());

        // 2. Dice su diálogo (le pasamos 'false' para que NO instancie el botón al acabar)
        yield return StartCoroutine(ManejarDialogo(alumno.Decir(alumno.dialogoPrincipal, false)));

        // 3. Entrega el examen (al terminar esta función, sí que aparece el botón)
        yield return StartCoroutine(alumno.EntregarExamen());
    }

    private IEnumerator ManejarDialogo(IEnumerator corrutinaHablar)
    {
        dialogoEnCurso = true;
        yield return StartCoroutine(corrutinaHablar);
        dialogoEnCurso = false;
    }
}