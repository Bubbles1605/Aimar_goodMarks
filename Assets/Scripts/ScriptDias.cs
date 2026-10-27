using System.Collections;
using UnityEngine;
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

    [Header("Textos Día 1 - Stalker")]
    public string[] dialogoPreManzanaStalker = { "Hola.", "Me llamo Luna.", "Tú eres Mark.", "Toma para ti, es una manzana, es para ti y SOLO para ti." };
    public string[] dialogoPostManzanaStalker = { "Ah, ya, mis ejercicios. Toma." };
    public string[] stalker_5 = { "Mejoraré por ti.", "Ya lo verás." };
    public string[] stalker_sonrojada = { "Hala, muchas gracias profe." };
    public string[] stalkerSuspenso = { "Vaya" };

    [Header("Textos Día 1 - Negrete")]
    public string[] dialogoPresentacionNegrete = { "Epa profe, ya estoy aquí.", "A ver qué nota me cae." };
    public string[] negreteReaccionFeliz = { "¡Toma ya! ¡Ese diez sabe a gloria!" };
    public string[] negreteReaccionEnfadado = { "¿Un aprobado raspado? Me esperaba más de ti." };
    public string[] negreteReaccionTriste = { "Menuda estafa de corrección, tío." };

    [Header("Textos Día 1 - Gordete")]
    public string[] dialogoPresentacionGordete = { "Hola profe, ¿hay algún bocata por ahí?", "Tengo un hambre que me muero." };
    public string[] gordeteReaccionFeliz = { "¡Qué pro! Me voy a celebrar esto comiendo pizza.", "¡GG!" };
    public string[] gordeteReaccionEnfadado = { "¿Un suspenso? Me estás trolleando con la nota." };
    public string[] gordeteReaccionTriste = { "Menudo hambre y menuda nota más mala me llevo..." };

    [Header("Textos Día 1 - Mewing")]
    public string[] dialogoPresentacionMewing = { "...", "(Hace el gesto de callar con el dedo en los labios y marca mandíbula)" };
    public string[] mewingReaccionFeliz = { "(Asiente en completo silencio con una pose de gigachad)" };
    public string[] mewingReaccionEnfadado = { "(Mira fijamente con desprecio supremo sin abrir la boca)" };
    public string[] mewingReaccionTriste = { "(Sigue haciendo mewing intensamente a pesar del suspenso)" };

    [Header("Evento Manzana (Stalker)")]
    public GameObject prefabManzana;
    public Vector2 posicionManzanaMesa;

    [Header("Paneles de Muerte por Alumno")]
    public GameObject panelMuerteBasicote;
    public GameObject panelMuerteBrainrot;
    public GameObject panelMuerteRepelente;
    public GameObject panelMuerteStalker;
    public GameObject panelMuerteNegrete;
    public GameObject panelMuerteGordete;
    public GameObject panelMuerteMewing;

    [Header("Animators de los Paneles de Muerte")]
    public Animator animadorMuerteBasicote;
    public Animator animadorMuerteBrainrot;
    public Animator animadorMuerteRepelente;
    public Animator animadorMuerteStalker;
    public Animator animadorMuerteNegrete;
    public Animator animadorMuerteGordete;
    public Animator animadorMuerteMewing;

    [Header("Duración de Animaciones de Muerte (Segundos)")]
    public float duracionMuerteBasicote = 2.0f;
    public float duracionMuerteBrainrot = 2.0f;
    public float duracionMuerteRepelente = 2.0f;
    public float duracionMuerteStalker = 2.0f;
    public float duracionMuerteNegrete = 2.0f;
    public float duracionMuerteGordete = 2.0f;
    public float duracionMuerteMewing = 2.0f;

    [Header("Pantalla de Derrota Final")]
    public GameObject panelPantallaMuerte;

    void Start()
    {
        PlayerPrefs.DeleteKey("FaltasTungTung");

        if (panelPantallaMuerte != null) panelPantallaMuerte.SetActive(false);
        if (panelMuerteBasicote != null) panelMuerteBasicote.SetActive(false);
        if (panelMuerteBrainrot != null) panelMuerteBrainrot.SetActive(false);
        if (panelMuerteRepelente != null) panelMuerteRepelente.SetActive(false);
        if (panelMuerteStalker != null) panelMuerteStalker.SetActive(false);
        if (panelMuerteNegrete != null) panelMuerteNegrete.SetActive(false);
        if (panelMuerteGordete != null) panelMuerteGordete.SetActive(false);
        if (panelMuerteMewing != null) panelMuerteMewing.SetActive(false);

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

        yield return StartCoroutine(basicote.Aparecer());
        yield return StartCoroutine(ManejarDialogo(basicote.Decir(dialogoPresentacionBasicote, false)));
        yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "¿Te enteras, o qué?" }, false)));

        string[] opcionesSiNo = { "Sí", "No" };
        yield return StartCoroutine(basicote.Preguntar(opcionesSiNo));

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
        Destroy(basicote.gameObject);
        yield return new WaitForSeconds(1f);

        // ==========================================
        // TURNO 2: BRAINROT
        // ==========================================
        if (brainrot != null)
        {
            alumnoActual = brainrot;
            yaPenalizadoHoy = false;

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
        // TURNO 6: GORDETE (Con ejemplo de muerte por nota 0)
        // ==========================================
        if (gordete != null)
        {
            alumnoActual = gordete;

            yield return StartCoroutine(gordete.Aparecer());
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(dialogoPresentacionGordete, false)));

            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¿Me das la manzana que tienes en tu mesa?" }, false)));
            yield return StartCoroutine(gordete.Preguntar(opcionesSiNo));

            if (gordete.respuestaElegida == 0)
            {
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¡Uuff, qué crack! ¡Me salvas la vida con este tentempié!" }, true)));

                GameObject manzanaEnMesa = GameObject.FindWithTag("ManzanaMesa");
                if (manzanaEnMesa != null) Destroy(manzanaEnMesa);
            }
            else if (gordete.respuestaElegida == 1)
            {
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Vaya tacaño... Me quedo con las tripas sonando." }, true)));
            }

            yield return StartCoroutine(gordete.EntregarExamen());

            nota = gordete.notaRecibida;

            // --- EJEMPLO DE CONDICIÓN DE MUERTE ESPECÍFICA ---
            if (nota == 0)
            {
                gordete.animator.SetTrigger("Enfadado");
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¿Un CERO? ¡¡Te voy a triturar con mis propias manos!!" }, false)));

                StartCoroutine(MatarJugadorPorAlumno(panelMuerteGordete, animadorMuerteGordete, duracionMuerteGordete));
                yield break;
            }
            // ------------------------------------------------

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

            yield return StartCoroutine(mewing.Aparecer());
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(dialogoPresentacionMewing, false)));

            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "(Señala su mandíbula firmemente)", "¿Estás totalmente embelesado por mis rasgos perfectos?" }, false)));
            yield return StartCoroutine(mewing.Preguntar(opcionesSiNo));

            if (mewing.respuestaElegida == 0)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "(Asiente con una sonrisa de superioridad y máxima elegancia)" }, true)));
            }
            else if (mewing.respuestaElegida == 1)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "(MIRA FUSILANDO CON LA MIRADA. -1000 de aura para ti)" }, true)));
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

    private IEnumerator ManejarDialogo(IEnumerator corrutinaHablar)
    {
        dialogoEnCurso = true;
        yield return StartCoroutine(corrutinaHablar);
        dialogoEnCurso = false;
    }

    // --- SISTEMA DE MUERTES CORREGIDO ---
    public IEnumerator MatarJugadorPorAlumno(GameObject panelMuerteAlumno, Animator animadorPanel, float duracionAnimacion)
    {
        // 1. Destruimos al alumno actual al instante para que no se solape
        if (alumnoActual != null)
        {
            Destroy(alumnoActual.gameObject);
        }

        // 2. Activamos el panel de muerte correspondiente
        if (panelMuerteAlumno != null)
        {
            panelMuerteAlumno.SetActive(true);
        }

        // 3. Activamos el trigger PRIMERO (antes de congelar el tiempo)
        if (animadorPanel != null)
        {
            animadorPanel.SetTrigger("AnimarMuerte");
        }

        // 4. Esperamos 1 frame para que el Animator reciba la orden correctamente
        yield return null;

        // 5. AHORA SÍ, pausamos el tiempo del juego por completo
        Time.timeScale = 0f;

        // 6. Esperamos exactamente el tiempo configurado para la animación de este alumno (en Unscaled Time)
        yield return new WaitForSecondsRealtime(duracionAnimacion);

        // 7. Ocultamos el panel de muerte del alumno
        if (panelMuerteAlumno != null)
        {
            panelMuerteAlumno.SetActive(false);
        }

        // 8. Mostramos la pantalla general de Game Over / Derrota Final
        if (panelPantallaMuerte != null)
        {
            panelPantallaMuerte.SetActive(true);
        }
    }

    // --- SISTEMA DE INFRACCIONES DE MARK ---
    public void PenalizarDibujo()
    {
        if (dialogoEnCurso || alumnoActual == null) return;

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
            StartCoroutine(MatarJugadorPorAlumno(panelMuerteStalker, animadorMuerteStalker, duracionMuerteStalker));
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

    // --- FUNCIONES DE ESCENAS ---
    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}