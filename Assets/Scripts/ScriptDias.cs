using System.Collections;
using UnityEngine;

public class ScriptDias : MonoBehaviour
{
    [Header("Alumnos del Día 1")]
    public ScriptAlumno basicote;
    // public ScriptAlumno brainrot; // Para cuando lo añadas
    // public ScriptAlumno gordete;

    [Header("Alumno en Pantalla")]
    public ScriptAlumno alumnoActual;
    private bool dialogoEnCurso = false;

    [Header("Textos Día 1 - Basicote")]
    public string[] dialogoPresentacionBasicote = { "Hola profe, soy Basicote.", "A ver qué me has puesto." };
    public string[] basicoteReaccionFeliz = { "¡Bua, un aprobadillo! ¡Qué grande eres!", "Me voy al parque." };
    public string[] basicoteReaccionEnfadado = { "¿Un suspenso? ¿En serio?", "Vaya asco de corrección." };
    public string[] basicoteReaccionTriste = { "Negrito feo" };

    void Start()
    {
        // Empezamos el juego lanzando directamente el guion del Día 1
        StartCoroutine(SecuenciaDia1());
    }

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
                    if (alumnoActual != null && !dialogoEnCurso)
                    {
                        StartCoroutine(ManejarDialogo(alumnoActual.DecirAleatorio(alumnoActual.frasesGenericas)));
                    }
                    break;
                }
            }
        }
    }

    // --- EL GUION DEL DÍA 1 ---
    public IEnumerator SecuenciaDia1()
    {
        // ==========================================
        // TURNO 1: BASICOTE
        // ==========================================
        alumnoActual = basicote;

        // 1. Sube y habla
        yield return StartCoroutine(basicote.Aparecer());
        yield return StartCoroutine(ManejarDialogo(basicote.Decir(dialogoPresentacionBasicote, false)));

        // 2. Tira el examen a la mesa y el juego se pausa hasta que el jugador ponga nota
        yield return StartCoroutine(basicote.EntregarExamen());

        // 3. ¡EL EXAMEN YA SE HA IDO! Evaluamos la nota con los Ifs
        int nota = basicote.notaRecibida;

        if (nota >= 5)
        {
            basicote.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(basicoteReaccionFeliz, true)));
        }
        else if (nota == 4)
        {
            basicote.animator.SetTrigger("Enfadado");
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(basicoteReaccionEnfadado, true)));
        } else
        {
            basicote.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(basicoteReaccionTriste, true)));
        }

        // Opcional: Aquí podrías poner un código para que Basicote se vaya (baje por la pantalla)
        // yield return StartCoroutine(basicote.Desaparecer());


        // ==========================================
        // TURNO 2: EL SIGUIENTE ALUMNO
        // ==========================================
        /*
        alumnoActual = brainrot;
        yield return StartCoroutine(brainrot.Aparecer());
        ... y sus propios ifs aquí
        */
    }

    private IEnumerator ManejarDialogo(IEnumerator corrutinaHablar)
    {
        dialogoEnCurso = true;
        yield return StartCoroutine(corrutinaHablar);
        dialogoEnCurso = false;
    }
}