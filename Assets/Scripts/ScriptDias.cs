using System.Collections;
using UnityEngine;

public class ScriptDias : MonoBehaviour
{
    [Header("Alumnos del Día 1")]
    public ScriptAlumno basicote;
    public ScriptAlumno brainrot; // Para cuando lo añadas
    // public ScriptAlumno gordete;

    [Header("Alumno en Pantalla")]
    public ScriptAlumno alumnoActual;
    private bool dialogoEnCurso = false;

    [Header("Textos Día 1 - Basicote")]
    public string[] dialogoPresentacionBasicote = { "Hola profe, soy Basicote.", "A ver qué me has puesto." };
    public string[] basicoteReaccionFeliz = { "¡Bua, un aprobadillo! ¡Qué grande eres!", "Me voy al parque." };
    public string[] basicoteReaccionEnfadado = { "¿Un suspenso? ¿En serio?", "Vaya asco de corrección." };
    public string[] basicoteReaccionTriste = { "Negrito feo" };

    [Header("Textos Día 1 - Brainrot")]
    public string[] dialogoPresentacionBrainrot = { "Skibidi toilet profe.", "¿Ese examen es W o L?" };
    public string[] brainrotReaccionFeliz = { "¡W rizz! Eres el GOAT." };
    public string[] brainrotReaccionEnfadado = { "L bozo. Cringe." };
    public string[] brainrotReaccionTriste = { "Bro really said suspenso." };

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

        // Cambiamos el regenerarBoton a false para que no salga antes de irse
        if (nota >= 5)
        {
            basicote.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(basicoteReaccionFeliz, false)));
        }
        else if (nota == 4)
        {
            basicote.animator.SetTrigger("Enfadado");
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(basicoteReaccionEnfadado, false)));
        }
        else
        {
            basicote.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(basicoteReaccionTriste, false)));
        }

        // Opcional: Aquí podrías poner un código para que Basicote se vaya (baje por la pantalla)
        yield return StartCoroutine(basicote.Desaparecer());

        // Esperamos 1 segundo con la clase vacía antes de que suba el siguiente
        yield return new WaitForSeconds(1f);


        // ==========================================
        // TURNO 2: EL SIGUIENTE ALUMNO
        // ==========================================
        if (brainrot != null)
        {
            alumnoActual = brainrot;

            // 1. Sube y habla
            yield return StartCoroutine(brainrot.Aparecer());
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(dialogoPresentacionBrainrot, false)));

            // 2. Tira el examen a la mesa
            yield return StartCoroutine(brainrot.EntregarExamen());

            // 3. Evaluamos la nota con los Ifs
            int nota2 = brainrot.notaRecibida;

            if (nota2 >= 5)
            {
                brainrot.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(brainrot.Decir(brainrotReaccionFeliz, false)));
            }
            else if (nota2 == 4)
            {
                brainrot.animator.SetTrigger("Enfadado");
                yield return StartCoroutine(ManejarDialogo(brainrot.Decir(brainrotReaccionEnfadado, false)));
            }
            else
            {
                brainrot.animator.SetTrigger("Triste");
                yield return StartCoroutine(ManejarDialogo(brainrot.Decir(brainrotReaccionTriste, false)));
            }

            // Brainrot se va
            yield return StartCoroutine(brainrot.Desaparecer());
        }
    }

    private IEnumerator ManejarDialogo(IEnumerator corrutinaHablar)
    {
        dialogoEnCurso = true;
        yield return StartCoroutine(corrutinaHablar);
        dialogoEnCurso = false;
    }
}