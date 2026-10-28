using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ScriptDias : MonoBehaviour
{
    [Header("Alumnos del Día 1")]
    public ScriptAlumno basicote;
    public ScriptAlumno brainrot;
    public ScriptAlumno repelente;
    public ScriptAlumno stalker;
    public ScriptAlumno negrete;
    public ScriptAlumno gordete;
    public ScriptAlumno mewing;

    [Header("Sonidos")]
    public AudioClip entrar_niño;
    public AudioClip entrar_niña;

    [Header("Sonidos_Basicote")]
    public AudioClip Triste_basicote;
    public AudioClip Risa_basicote;
    public AudioClip Enfado_basicote;
    public AudioSource audioSource;

    [Header("Alumno en Pantalla")]
    public ScriptAlumno alumnoActual;
    public bool dialogoEnCurso = false;
    public bool esperandoRespuesta = false;
    private bool yaPenalizadoHoy = false;

    [Header("Configuración de Cursores")]
    public Texture2D cursorManita;

    [Header("Textos Día 1 - Basicote")]
    public string[] dialogoPresentacionBasicote = { "..." };
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

    [Header("Textos Día 1 - Stalker")]
    public string[] dialogoPreManzanaStalker = { "Hola.", "Me llamo Luna.", "Tú eres Mark.", "Toma, para ti. Es una manzana, es para ti y SOLO para ti." };
    public string[] dialogoPostManzanaStalker = { "Ah, ya, mis ejercicios. Toma." };
    public string[] stalker_5 = { "Mejoraré por ti.", "Ya lo verás." };
    public string[] stalker_sonrojada = { "Vaya, muchas gracias." };
    public string[] stalkerSuspenso = { "Mejoraré por ti.", "Ya lo verás." };

    [Header("Textos Día 1 - Negrete")]
    public string[] dialogoPresentacionNegrete = { "Epa profe, ya estoy aquí.", "A ver qué nota me cae." };
    public string[] negreteReaccionFeliz = { "¡Toma ya! ¡Ese diez sabe a gloria!" };
    public string[] negreteReaccionEnfadado = { "¿Un aprobado raspado? Me esperaba más de ti." };
    public string[] negreteReaccionTriste = { "Menuda estafa de corrección, tío." };

    [Header("Textos Día 1 - Gordete")]
    public string[] dialogoPresentacionGordete = { "Hola." };
    public string[] gordeteReaccionFeliz = { "Me encanta esta nota.", "Me encanta el queso también." };
    public string[] gordeteReaccionEnfadado = { "Esta nota me da ganas de morderte." };
    public string[] gordeteReaccionTriste = { "Esta nota me da arcadas." };

    [Header("Textos Día 1 - Mewing")]
    public string[] dialogoPresentacionMewing = { "...", "(Hace el gesto de callar con el dedo en los labios y marca mandíbula)" };
    public string[] mewingReaccionFeliz = { "(Asiente en completo silencio con una pose de gigachad)" };
    public string[] mewingReaccionEnfadado = { "(Mira fijamente con desprecio supremo sin abrir la boca)" };
    public string[] mewingReaccionTriste = { "(Sigue haciendo mewing intensamente a pesar del suspenso)" };

    [Header("Evento Manzana (Stalker)")]
    public GameObject prefabManzana;
    public Vector2 posicionManzanaMesa;

    [Header("Pantalla de Muerte / Game Over")]
    public GameObject panelPantallaMuerte; // El panel general con Reintentar / Menú

    void Start()
    {
        PlayerPrefs.DeleteKey("FaltasTungTung");

        if (panelPantallaMuerte != null)
        {
            panelPantallaMuerte.SetActive(false);
        }

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
                    if (alumnoActual != null && !dialogoEnCurso && !esperandoRespuesta)
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
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);

        yield return StartCoroutine(basicote.Aparecer());

        yield return StartCoroutine(ManejarDialogo(basicote.Decir(dialogoPresentacionBasicote, false)));
        yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "¿Te enteras, o qué?" }, false)));

        string[] opcionesSiNo = { "Sí", "No" };

        yield return StartCoroutine(ManejarPregunta(basicote.Preguntar(opcionesSiNo)));

        if (basicote.respuestaElegida == 0)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "PUES EMPIEZA, JODER." }, true)));
        }
        else if (basicote.respuestaElegida == 1)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "ME LA SUDA." }, true)));
        }

        yield return StartCoroutine(basicote.EntregarExamen());

        int nota = basicote.notaRecibida;

        if (nota >= 5)
        {
            basicote.animator.SetTrigger("Feliz");
            if (Risa_basicote != null) audioSource.PlayOneShot(Risa_basicote, 1.3f);

            yield return StartCoroutine(ManejarDialogo(basicote.Decir(basicoteReaccionFeliz, false)));
        }
        else if (nota == 4)
        {
            basicote.animator.SetTrigger("Enfadado");
            if (Enfado_basicote != null) audioSource.PlayOneShot(Enfado_basicote, 1.3f);

            yield return StartCoroutine(ManejarDialogo(basicote.Decir(basicoteReaccionEnfadado, false)));
        }
        else
        {
            basicote.animator.SetTrigger("Triste");
            if (Triste_basicote != null) audioSource.PlayOneShot(Triste_basicote, 1.3f);

            yield return StartCoroutine(ManejarDialogo(basicote.Decir(basicoteReaccionTriste, false)));
        }

        yield return StartCoroutine(basicote.Desaparecer());
        Destroy(basicote.gameObject);
        yield return new WaitForSeconds(1f);

        // ==========================================
        // TURNO 2: BRAINROT
        // ==========================================
        if (brainrot != null)
        {
            alumnoActual = brainrot;
            yaPenalizadoHoy = false;
            if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);

            yield return StartCoroutine(brainrot.Aparecer());
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(dialogoPresentacionBrainrot, false)));
            yield return StartCoroutine(brainrot.EntregarExamen());

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

            yield return StartCoroutine(brainrot.Desaparecer());
            Destroy(brainrot.gameObject);
            yield return new WaitForSeconds(1f);
        }

        // ==========================================
        // TURNO 3: LISTILLA (REPELENTE)
        // ==========================================
        if (repelente != null)
        {
            alumnoActual = repelente;
            if (entrar_niña != null) audioSource.PlayOneShot(entrar_niña, 5f);

            yield return StartCoroutine(repelente.Aparecer());
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(dialogoPresentacionListilla, false)));
            yield return StartCoroutine(repelente.EntregarExamen());

            nota = repelente.notaRecibida;

            if (nota == 10)
            {
                repelente.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(repelente.Decir(repelenteReaccion10, false)));
            }
            else if (nota >= 5)
            {
                repelente.animator.SetTrigger("Enfadado");
                yield return StartCoroutine(ManejarDialogo(repelente.Decir(repelenteReaccionAprobado, false)));
                if (repelente.animator != null) repelente.animator.SetTrigger("enfado2");
            }
            else
            {
                repelente.animator.SetTrigger("Enfadado");
                yield return StartCoroutine(ManejarDialogo(repelente.Decir(repelenteReaccionSuspenso, false)));
                if (repelente.animator != null) repelente.animator.SetTrigger("enfado2");
            }

            yield return StartCoroutine(repelente.Desaparecer());
            Destroy(repelente.gameObject);
            yield return new WaitForSeconds(1f);
        }

        // ==========================================
        // TURNO 4: STALKER (LUNA)
        // ==========================================
        if (stalker != null)
        {
            alumnoActual = stalker;
            if (entrar_niña != null) audioSource.PlayOneShot(entrar_niña, 5f);

            yield return StartCoroutine(stalker.Aparecer());
            yield return StartCoroutine(ManejarDialogo(stalker.Decir(dialogoPreManzanaStalker, false)));

            if (stalker.animator != null) stalker.animator.SetTrigger("Manzana");

            yield return new WaitForSeconds(1f);

            if (prefabManzana != null)
            {
                Vector3 posicionFinalManzana = new Vector3(posicionManzanaMesa.x, posicionManzanaMesa.y, 0f);
                GameObject manzanaCreada = Instantiate(prefabManzana, posicionFinalManzana, Quaternion.identity);
                manzanaCreada.tag = "ManzanaMesa";
            }

            yield return StartCoroutine(ManejarDialogo(stalker.Decir(dialogoPostManzanaStalker, false)));
            yield return StartCoroutine(stalker.EntregarExamen());

            nota = stalker.notaRecibida;

            if (nota < 5)
            {
                stalker.animator.SetTrigger("Triste");
                yield return StartCoroutine(ManejarDialogo(stalker.Decir(stalkerSuspenso, false)));
            }
            else if (nota == 5)
            {
                stalker.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(stalker.Decir(stalker_5, false)));
            }
            else
            {
                stalker.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(stalker.Decir(stalker_sonrojada, false)));
            }

            yield return StartCoroutine(stalker.Desaparecer());
            Destroy(stalker.gameObject);
            yield return new WaitForSeconds(1f);
        }

        // ==========================================
        // TURNO 5: NEGRETE
        // ==========================================
        if (negrete != null)
        {
            alumnoActual = negrete;
            if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);

            yield return StartCoroutine(negrete.Aparecer());
            yield return StartCoroutine(ManejarDialogo(negrete.Decir(dialogoPresentacionNegrete, false)));
            yield return StartCoroutine(negrete.EntregarExamen());

            nota = negrete.notaRecibida;

            if (nota >= 5)
            {
                negrete.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(negrete.Decir(negreteReaccionFeliz, false)));
            }
            else if (nota == 4)
            {
                negrete.animator.SetTrigger("Enfadado");
                yield return StartCoroutine(ManejarDialogo(negrete.Decir(negreteReaccionEnfadado, false)));
            }
            else
            {
                negrete.animator.SetTrigger("Triste");
                yield return StartCoroutine(ManejarDialogo(negrete.Decir(negreteReaccionTriste, false)));
            }

            yield return StartCoroutine(negrete.Desaparecer());
            Destroy(negrete.gameObject);
            yield return new WaitForSeconds(1f);
        }

        // ==========================================
        // TURNO 6: GORDETE (CON EJEMPLO DE MUERTE)
        // ==========================================
        if (gordete != null)
        {
            alumnoActual = gordete;
            if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);

            yield return StartCoroutine(gordete.Aparecer());
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(dialogoPresentacionGordete, false)));

            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¿Me das la manzana que tienes en tu mesa? Tengo hambre." }, false)));

            yield return StartCoroutine(ManejarPregunta(gordete.Preguntar(opcionesSiNo)));

            if (gordete.respuestaElegida == 0) // Si (Sí)
            {
                GameObject manzanaEnMesa = GameObject.FindWithTag("ManzanaMesa");
                if (manzanaEnMesa != null)
                {
                    Destroy(manzanaEnMesa);
                }
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "QUE DELICIA." , "Aunque el centro sabía un poco a lente con un ligero toque a batería de litio." , "En fin, lo que no mata engorda.", "Y si me mata, te vas a cagar, jeje." }, true)));
            }
            else if (gordete.respuestaElegida == 1) // Si (No)
            {
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Pues tu puta madre." }, true)));
            }

            yield return StartCoroutine(gordete.EntregarExamen());

            nota = gordete.notaRecibida;

            // ===========================================
            // AQUÍ LLAMAMOS AL SISTEMA DE MUERTE
            // ===========================================
            if (nota == 0)
            {
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¿¡Me has dejado sin comer y encima me pones un cero!?", "Te vas a arrepentir, cabrón." }, false)));
                yield return StartCoroutine(EjecutarMuerte(gordete)); // ¡Llamada mágica!
                yield break; // Paramos todo para que el día no siga
            }

            if (nota >= 5)
            {
                gordete.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(gordeteReaccionFeliz, false)));
            }
            else if (nota == 4)
            {
                gordete.animator.SetTrigger("Enfadado");
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(gordeteReaccionEnfadado, false)));
            }
            else
            {
                gordete.animator.SetTrigger("Triste");
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(gordeteReaccionTriste, false)));
            }

            yield return StartCoroutine(gordete.Desaparecer());
            Destroy(gordete.gameObject);
            yield return new WaitForSeconds(1f);
        }

        // ==========================================
        // TURNO 7: MEWING
        // ==========================================
        if (mewing != null)
        {
            alumnoActual = mewing;
            mewing.animator.SetInteger("Dia", 1);
            if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);

            yield return StartCoroutine(mewing.Aparecer());
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(dialogoPresentacionMewing, false)));

            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "¿Estás totalmente embelesado por mis rasgos perfectos?" }, false)));

            yield return StartCoroutine(ManejarPregunta(mewing.Preguntar(opcionesSiNo)));

            if (mewing.respuestaElegida == 0) // Si (Sí)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Todo mi esfuerzo ha dado sus frutos." }, true)));
            }
            else if (mewing.respuestaElegida == 1) // Si (No)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Endureceré mis rutinas de looksmaxing al doble de tiempo." }, true)));
            }

            yield return StartCoroutine(mewing.EntregarExamen());

            nota = mewing.notaRecibida;

            if (nota >= 5)
            {
                mewing.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(mewingReaccionFeliz, false)));
            }
            else if (nota == 4)
            {
                mewing.animator.SetTrigger("Enfadado");
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(mewingReaccionEnfadado, false)));
            }
            else
            {
                mewing.animator.SetTrigger("Triste");
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(mewingReaccionTriste, false)));
            }

            yield return StartCoroutine(mewing.Desaparecer());
            Destroy(mewing.gameObject);
        }
    }

    // DÍA 2 ---------------------------

    private void ForzarSoltarHerramientas()
    {
        Cursor.SetCursor(cursorManita, Vector2.zero, CursorMode.Auto);

        ControladorRotulador rotulador = FindFirstObjectByType<ControladorRotulador>();
        if (rotulador != null)
        {
            rotulador.SoltarTodo();
        }
    }

    private IEnumerator ManejarDialogo(IEnumerator corrutinaHablar)
    {
        ForzarSoltarHerramientas();
        dialogoEnCurso = true;
        yield return StartCoroutine(corrutinaHablar);
        dialogoEnCurso = false;
    }

    private IEnumerator ManejarPregunta(IEnumerator corrutinaPreguntar)
    {
        ForzarSoltarHerramientas();
        esperandoRespuesta = true;
        yield return StartCoroutine(corrutinaPreguntar);
        esperandoRespuesta = false;
    }

    // =========================================================
    // NUEVO: SISTEMA MAESTRO DE MUERTES
    // =========================================================
    public IEnumerator EjecutarMuerte(ScriptAlumno asesino)
    {
        GameObject panelAsesino = null; // Variable para guardar el panel

        if (asesino != null)
        {
            panelAsesino = asesino.panelMuerte; // Lo guardamos antes de destruir al niño

            asesino.DesplegarPanelMuerte();
            float tiempoEspera = asesino.duracionAnimacionMuerte;

            Destroy(asesino.gameObject);
            alumnoActual = null;

            yield return new WaitForSeconds(tiempoEspera);
        }

        // 5. Apagamos el panel del niño para que no bloquee los clics
        if (panelAsesino != null)
        {
            panelAsesino.SetActive(false);
        }

        // 6. Sacamos el panel de Game Over y congelamos el tiempo
        ActivarPantallaMuerte();
    }



    // --- SISTEMA DE INFRACCIONES DE MARK ---
    public void PenalizarDibujo()
    {
        if (dialogoEnCurso || alumnoActual == null) return;

        ForzarSoltarHerramientas();

        int faltas = PlayerPrefs.GetInt("FaltasTungTung", 0);

        if (!yaPenalizadoHoy)
        {
            faltas++;
            PlayerPrefs.SetInt("FaltasTungTung", faltas);
            yaPenalizadoHoy = true;
        }

        if (faltas >= 3)
        {
            StartCoroutine(ManejarDialogo(alumnoActual.Decir(new string[] { "TÚ TE LO HAS BUSCADO.", "HAS INTENTADO DESTROZAR A TUNG-TUNG TRES VECES.", "AHORA SUFRIRÁS SU IRA." }, false)));

            // Si el Tung-Tung te mata, puedes llamar a la muerte de Stalker, por ejemplo:
            // StartCoroutine(EjecutarMuerte(stalker));
            ActivarPantallaMuerte();
        }
        else if (faltas == 2)
        {
            StartCoroutine(ManejarDialogo(alumnoActual.Decir(new string[] { "Te he dicho que no toques mis dibujos.", "La próxima vez no será un aviso." }, true)));
        }
        else if (faltas == 1)
        {
            StartCoroutine(ManejarDialogo(alumnoActual.Decir(new string[] { "¡¡¡OYE!!!", "¡¿QUÉ LE HACES A MI TUNG-TUNG?!" }, true)));
        }
    }

    // --- FUNCIONES DE MUERTE Y ESCENAS ---
    // --- FUNCIONES DE MUERTE Y ESCENAS ---
    public void ActivarPantallaMuerte()
    {
        if (panelPantallaMuerte != null)
        {
            // 1. Encendemos el panel
            panelPantallaMuerte.SetActive(true);
            
        }
    }

    public void ReiniciarJuego()
    {
     
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
   
        SceneManager.LoadScene("Menu");
    }
}