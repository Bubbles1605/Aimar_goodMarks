using System.Collections;
using UnityEngine;

public class ScriptDias : MonoBehaviour
{
    [Header("Alumnos del Día 1")]
    public ScriptAlumno basicote;
    public ScriptAlumno brainrot; 
    public ScriptAlumno repelente;

    [Header("Alumno en Pantalla")]
    public ScriptAlumno alumnoActual;
    private bool dialogoEnCurso = false;
    private bool yaPenalizadoHoy = false;

    [Header("Textos Día 1 - Basicote")]
    public string[] dialogoPresentacionBasicote = { "Hola profe, soy Basicote.", "A ver qué me has puesto." };
    public string[] basicoteReaccionFeliz = { "¡Bua, un aprobadillo! ¡Qué grande eres!", "Me voy al parque." };
    public string[] basicoteReaccionEnfadado = { "¿Un suspenso? ¿En serio?", "Vaya asco de corrección." };
    public string[] basicoteReaccionTriste = { "Negrito feo" };

    [Header("Textos Día 1 - Brainrot")]
    public string[] dialogoPresentacionBrainrot = { "Skibidi toilet profe.", "¿Ese examen es W o L?" };
    public string[] brainrotReaccionFeliz = { "¡GYAAAAAT! Eres el GOAT." };
    public string[] brainrotReaccionEnfadado = { "Ya sabía yo que das mucho cringe." };
    public string[] brainrotReaccionTriste = { "Bro really said suspenso. -1000 de aura." };

    [Header("Textos Día 1 - Listilla")]
    public string[] dialogoPresentacionListilla = { "Buenos días, profesor Mark.", "Espero mi merecido diez." };
    public string[] repelenteReaccion10 = { "Como debe ser.", "Mi padre estará orgulloso." };
    public string[] repelenteReaccionAprobado = { "¿Un triste aprobado?", "Esto es inaceptable." };
    public string[] repelenteReaccionSuspenso = { "¿TÚ SABES QUIÉN ES MI PADRE?", "Estás muerto." };

    void Start()
    {
        PlayerPrefs.DeleteKey("FaltasTungTung");
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
        yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "¿Te enteras, o qué?" }, false)));

        string[] misOpciones = { "Sí", "No" };
        yield return StartCoroutine(basicote.Preguntar(misOpciones));

        if (basicote.respuestaElegida == 0)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "PUES EMPIEZA, JODER." }, true)));
        }
        else if (basicote.respuestaElegida == 1)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "ME LA SUDA." }, true)));
        }

        // 2. Tira el examen a la mesa y el juego se pausa hasta que el jugador ponga nota
        yield return StartCoroutine(basicote.EntregarExamen());

        // 3. ¡EL EXAMEN YA SE HA IDO! Evaluamos la nota con los Ifs
        int nota = basicote.notaRecibida;

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

        yield return StartCoroutine(basicote.Desaparecer());
        yield return new WaitForSeconds(1f);

        // ==========================================
        // TURNO 2: BRAINROT
        // ==========================================
        if (brainrot != null)
        {
            alumnoActual = brainrot;
            yaPenalizadoHoy = false;

            // 1. Sube y habla
            yield return StartCoroutine(brainrot.Aparecer());
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(dialogoPresentacionBrainrot, false)));
            // 2. Tira el examen a la mesa
            yield return StartCoroutine(brainrot.EntregarExamen());

            // 3. Evaluamos la nota con los Ifs
            nota = brainrot.notaRecibida;

            if (nota >= 5)
            {
                brainrot.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(brainrot.Decir(brainrotReaccionFeliz, false)));
            }
            else if (nota == 4)
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

        // ==========================================
        // TURNO 3: LISTILLA
        // ==========================================

        if (repelente != null)
        {
            alumnoActual = repelente;

            yield return StartCoroutine(repelente.Aparecer());
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(dialogoPresentacionListilla, false)));

            // Si quieres ponerle una pregunta de prueba, puedes hacerlo aquí
            // string[] listillaOpciones = { "Toma un 10 imaginario", "Eres pesada" };
            // yield return StartCoroutine(listilla.Preguntar(listillaOpciones));

            yield return StartCoroutine(repelente.EntregarExamen());

            nota = repelente.notaRecibida;

            // Su lógica de notas es distinta: solo el 10 la hace feliz
            if (nota == 10)
            {
                repelente.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(repelente.Decir(repelenteReaccion10, false)));
            }
            else if (nota >= 5)
            {
                repelente.animator.SetTrigger("Enfadado");
                yield return StartCoroutine(ManejarDialogo(repelente.Decir(repelenteReaccionAprobado, false)));
            }
            else
            {
                repelente.animator.SetTrigger("Enfadado"); // Puedes cambiarlo a "Furiosa" si tienes esa animación
                yield return StartCoroutine(ManejarDialogo(repelente.Decir(repelenteReaccionSuspenso, false)));
            }

            yield return StartCoroutine(repelente.Desaparecer());
        }


    }

    private IEnumerator ManejarDialogo(IEnumerator corrutinaHablar)
    {
        dialogoEnCurso = true;
        yield return StartCoroutine(corrutinaHablar);
        dialogoEnCurso = false;
    }






    // --- SISTEMA DE INFRACCIONES DE MARK---
    public void PenalizarDibujo() //NIÑO BRAINROT
    {
        // Si ya está hablando o no hay alumno, ignoramos el toque para no solapar textos
        if (dialogoEnCurso || alumnoActual == null) return;

        int faltas = PlayerPrefs.GetInt("FaltasTungTung", 0);

        // Solo sumamos a la memoria si es la primera vez que le pintas HOY
        if (!yaPenalizadoHoy)
        {
            faltas++;
            PlayerPrefs.SetInt("FaltasTungTung", faltas);
            yaPenalizadoHoy = true;
        }

        //QUÉ TE DICE DEPENDIENDO DEL DÍA
        if (faltas >= 3)
        {
            StartCoroutine(ManejarDialogo(alumnoActual.Decir(new string[] { "TÚ TE LO HAS BUSCADO.", "HAS INTENTADO DESTROZAR A TUNG-TUNG TRES VECES.", "AHORA SUFRIRÁS SU IRA." }, false)));
            //AHORA SALTARIA LA ANIMACIÓN Y PIERDES QUE NO SE COMO SE HACE
        }
        else if (faltas == 2)
        {
            StartCoroutine(ManejarDialogo(alumnoActual.Decir(new string[] { "Te he dicho que no toques mis dibujos.", "La próxima vez no será un aviso."}, true)));
        }
        else if (faltas == 1)
        {
            StartCoroutine(ManejarDialogo(alumnoActual.Decir(new string[] { "¡¡¡OYE!!!", "¡¿QUÉ LE HACES A MI TUNG-TUNG?!" }, true)));
        }
    }
}