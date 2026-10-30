using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ScriptDias : MonoBehaviour
{
    [Header("Alumnos")]
    public ScriptAlumno basicote;
    public ScriptAlumno brainrot;
    public ScriptAlumno repelente;
    public ScriptAlumno stalker;
    public ScriptAlumno negrete;
    public ScriptAlumno gordete;
    public ScriptAlumno mewing;
    public ScriptAlumno padreLista;

    [Header("Padre de la Listilla")]
    public int diecesRepelente = 0;

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

    [Header("Control del Tiempo")]
    public int diaActual = 1;

    [Header("Transición de Días")]
    public GameObject objetoCartelDias;
    public float duracionCartel = 3f;



    [Header("Textos Día 1 - Basicote")]
    public string[] dialogoPresentacionBasicote = { "..." };
    public string[] basicoteReaccionFeliz = { "¡Bua, un aprobadillo! ¡Qué grande eres!", "Me voy al parque." };
    public string[] basicoteReaccionEnfadado = { "¿Un suspenso? ¿En serio?", "Vaya asco de corrección." };
    public string[] basicoteReaccionTriste = { "Negrito feo" };

    [Header("Textos - Brainrot")]
    public string[] dialogoPresentacionBrainrot = { "Skibidi toilet profe.", "¿Ese examen es W o L?" };
    public string[] dialogoDia2Brainrot_molesto = { "Hola profe, tung tung está enfadadillo", "Pero no te preocupes porque yo pienso que eres un skibidi con aura." };
    public string[] dialogoDia2Brainrot = { "Skibidi profe, estuve practicando para el examen escribiendo un montón de comentarios en tis tos." };
    public string[] brainrotReaccionFeliz = { "¡GYAAAAAT! Eres el GOAT." };
    public string[] brainrotReaccionEnfadado = { "Ya sabía yo que das mucho cringe." };
    public string[] brainrotReaccionTriste = { "Bro really said suspenso. -1000 de aura." };
    public string[] dialogoDia3Brainrot_molesto = { "Hola profe, tung tung está enfadadillo", "Pero no te preocupes porque yo pienso que eres un skibidi con aura." };
    public string[] dialogoDia3Brainrot = { "Skibidi profe, estudié este examen chateando con mis amigos ingleses por ronblon. " };
    public string[] dialogoDia3Brainrot_enfadado = { "Ayer volviste a tocar a tung tung. Último aviso. No lo hagas." };
    

    [Header("Textos - Listilla")]
    public string[] dialogoPresentacionListilla = { "Buenos días, profesor Mark.", "Espero mi merecido diez." };
    public string[] repelenteReaccion10 = { "Como debe ser.", "Mi padre estará orgulloso." };
    public string[] repelenteReaccionAprobado = { "¿Un triste aprobado?", "Esto es inaceptable." };
    public string[] repelenteReaccionSuspenso = { "¿TÚ SABES QUIÉN ES MI PADRE?", "Estás muerto." };

    [Header("Textos - Stalker")]
    public string[] dialogoPreManzanaStalker = { "Hola.", "Me llamo Luna.", "Tú eres Mark.", "Toma, para ti. Es una manzana, es para ti y SOLO para ti." };
    public string[] dialogoPostManzanaStalker = { "Ah, ya, mis ejercicios. Toma." };
    public string[] stalker_5 = { "Mejoraré por ti.", "Ya lo verás." };
    public string[] stalker_sonrojada = { "Vaya, muchas gracias." };
    public string[] stalkerSuspenso = { "Mejoraré por ti.", "Ya lo verás." };

    [Header("Textos - Negrete")]
    public string[] dialogoPresentacionNegrete = { "Epa profe, ya estoy aquí.", "A ver qué nota me cae." };
    public string[] negreteReaccionFeliz = { "¡Toma ya! ¡Ese diez sabe a gloria!" };
    public string[] negreteReaccionEnfadado = { "¿Un aprobado raspado? Me esperaba más de ti." };
    public string[] negreteReaccionTriste = { "Menuda estafa de corrección, tío." };

    [Header("Textos - Gordete")]
    public string[] dialogoPresentacionGordete = { "Hola." };
    public string[] gordeteReaccionFeliz = { "Me encanta esta nota.", "Me encanta el queso también." };
    public string[] gordeteReaccionEnfadado = { "Esta nota me da ganas de morderte." };
    public string[] gordeteReaccionTriste = { "Esta nota me da arcadas." };

    [Header("Textos - Mewing")]
    public string[] dialogoPresentacionMewing = { "...", "(Hace el gesto de callar con el dedo en los labios y marca mandíbula)" };
    public string[] mewingReaccionFeliz = { "(Asiente en completo silencio con una pose de gigachad)" };
    public string[] mewingReaccionEnfadado = { "(Mira fijamente con desprecio supremo sin abrir la boca)" };
    public string[] mewingReaccionTriste = { "(Sigue haciendo mewing intensamente a pesar del suspenso)" };

    [Header("Evento Manzana (Stalker)")]
    public GameObject prefabManzana;
    public Vector2 posicionManzanaMesa;

    [Header("Pantalla de Muerte / Game Over")]
    public GameObject panelPantallaMuerte;

    [Header("Variables varias")]
    public int nota = 0;
    public bool manzana_comida = false;
    public string[] opcionesSiNo = { "Sí", "No" };
    public bool estoyEmbelesado = false;
    public int diaMewing = 1;

    [Header("Pregunta de Basicote - Día 2")]
    public int decisionBasicoteDia2 = -1;
    public string[] opcionesBasicoteDia2 =
    {
    "Opción A",
    "Opción B"
    };

    // Variable para saber si el jugador ha firmado en la zona de la Stalker

    private bool reaccionZonaStalkerActivada = false;

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
        if (Time.timeScale == 0f) return;

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
                        string[] frasesQueToca = null;

                        if (diaActual == 1) frasesQueToca = alumnoActual.frasesGenericasDia1;
                        else if (diaActual == 2) frasesQueToca = alumnoActual.frasesGenericasDia2;
                        else if (diaActual == 3) frasesQueToca = alumnoActual.frasesGenericasDia3;
                        else if (diaActual == 4) frasesQueToca = alumnoActual.frasesGenericasDia4;

                        if (frasesQueToca != null && frasesQueToca.Length > 0)
                        {
                            StartCoroutine(ManejarDialogo(alumnoActual.DecirAleatorio(frasesQueToca)));
                        }
                    }
                    break;
                }
            }
        }
    }

    // --- EL GUION DEL DÍA 1 ---
    public IEnumerator SecuenciaDia1()
    {
        diaMewing = 1;
        diaActual = 1;
        alumnoActual = basicote;
        basicote.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);

        yield return StartCoroutine(basicote.Aparecer());

        yield return StartCoroutine(ManejarDialogo(basicote.Decir(dialogoPresentacionBasicote, false)));
        yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "¿Te enteras, o qué?" }, false)));

        yield return StartCoroutine(ManejarPregunta(basicote.Preguntar(opcionesSiNo)));

        if (basicote.respuestaElegida == 0)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "PUES EMPIEZA, JODER." }, true)));
        }
        else if (basicote.respuestaElegida == 1)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "ME LA SUDA." }, true)));
        }

        yield return StartCoroutine(basicote.EntregarExamen(1));

        nota = basicote.notaRecibida;

        if (nota >= 5)
        {
            basicote.animator.SetTrigger("Feliz");
            if (Risa_basicote != null) audioSource.PlayOneShot(Risa_basicote, 1.3f);

            yield return StartCoroutine(ManejarDialogo(basicote.Decir(basicoteReaccionFeliz, false)));
        }
        else if (nota == 4 || nota == 3)
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
        basicote.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

        // BRAINROT
        if (brainrot != null)
        {
            alumnoActual = brainrot;
            brainrot.gameObject.SetActive(true);
            yaPenalizadoHoy = false;
            if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);

            yield return StartCoroutine(brainrot.Aparecer());
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(dialogoPresentacionBrainrot, false)));
            yield return StartCoroutine(brainrot.EntregarExamen(1));

            nota = brainrot.notaRecibida;

            if (nota >= 5)
            {
                brainrot.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(brainrot.Decir(brainrotReaccionFeliz, false)));
            }
            else if (nota == 4 || nota == 3)
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
            brainrot.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }

        // REPELENTE
        if (repelente != null)
        {
            alumnoActual = repelente;
            repelente.gameObject.SetActive(true);
            if (entrar_niña != null) audioSource.PlayOneShot(entrar_niña, 5f);

            yield return StartCoroutine(repelente.Aparecer());
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(dialogoPresentacionListilla, false)));
            yield return StartCoroutine(repelente.EntregarExamen(1));

            nota = repelente.notaRecibida;
            if (nota == 10)
            {
                diecesRepelente++;
            }
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
            repelente.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }

        // STALKER DÍA 1
        if (stalker != null)
        {
            alumnoActual = stalker;
            stalker.gameObject.SetActive(true);
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
            yield return StartCoroutine(stalker.EntregarExamen(1));

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
            stalker.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }

        // NEGRETE
        if (negrete != null)
        {
            alumnoActual = negrete;
            negrete.gameObject.SetActive(true);
            if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);

            yield return StartCoroutine(negrete.Aparecer());
            yield return StartCoroutine(ManejarDialogo(negrete.Decir(dialogoPresentacionNegrete, false)));
            yield return StartCoroutine(negrete.EntregarExamen(1));

            nota = negrete.notaRecibida;

            if (nota >= 5)
            {
                negrete.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(negrete.Decir(negreteReaccionFeliz, false)));
            }
            else if (nota == 4 || nota == 3)
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
            negrete.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }

        // GORDETE (CON MUERTE)
        if (gordete != null)
        {
            alumnoActual = gordete;
            gordete.gameObject.SetActive(true);
            if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);

            yield return StartCoroutine(gordete.Aparecer());
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(dialogoPresentacionGordete, false)));

            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¿Me das la manzana que tienes en tu mesa? Tengo hambre." }, false)));

            yield return StartCoroutine(ManejarPregunta(gordete.Preguntar(opcionesSiNo)));

            if (gordete.respuestaElegida == 0)
            {
                GameObject manzanaEnMesa = GameObject.FindWithTag("ManzanaMesa");
                if (manzanaEnMesa != null)
                {
                    Destroy(manzanaEnMesa);
                    manzana_comida = true;
                }
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "QUE DELICIA.", "Aunque el centro sabía un poco a lente con un ligero toque a batería de litio.", "En fin, lo que no mata engorda.", "Y si me mata, te vas a cagar, jeje." }, true)));
            }
            else if (gordete.respuestaElegida == 1)
            {
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Pues tu puta madre." }, true)));
                manzana_comida = false;
            }

            yield return StartCoroutine(gordete.EntregarExamen(1));

            nota = gordete.notaRecibida;

            if (nota == 0 && manzana_comida == false)
            {
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¿¡Me has dejado sin comer y encima me pones un cero!?", "Te vas a arrepentir, cabrón." }, false)));
                yield return StartCoroutine(EjecutarMuerte(gordete)); 
                yield break;
            }

            if (nota >= 5)
            {
                gordete.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(gordeteReaccionFeliz, false)));
            }
            else if (nota == 4 || nota == 3)
            {
                gordete.animator.SetTrigger("Triste");
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(gordeteReaccionEnfadado, false)));
            }
            else
            {
                gordete.animator.SetTrigger("Triste");
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(gordeteReaccionTriste, false)));
            }

            yield return StartCoroutine(gordete.Desaparecer());
            gordete.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }

        // MEWING
        if (mewing != null)
        {
            alumnoActual = mewing;
            mewing.gameObject.SetActive(true);

            mewing.animator.SetInteger("Dia", 1);
           
            if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);

            yield return StartCoroutine(mewing.Aparecer());
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(dialogoPresentacionMewing, false)));

            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "¿Estás totalmente embelesado por mis rasgos perfectos?" }, false)));

            yield return StartCoroutine(ManejarPregunta(mewing.Preguntar(opcionesSiNo)));

            if (mewing.respuestaElegida == 0)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Todo mi esfuerzo ha dado sus frutos." }, true)));
                estoyEmbelesado = true;
            }
            else if (mewing.respuestaElegida == 1)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Endureceré mis rutinas de looksmaxing al doble de tiempo." }, true)));
                estoyEmbelesado = false;
                diaMewing++;
            }

            yield return StartCoroutine(mewing.EntregarExamen(1));

            nota = mewing.notaRecibida;

            if (nota >= 5)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(mewingReaccionFeliz, false)));
            }
            else if (nota == 4 || nota == 3)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(mewingReaccionEnfadado, false)));
            }
            else
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(mewingReaccionTriste, false)));
            }

            yield return StartCoroutine(mewing.Desaparecer());
            mewing.gameObject.SetActive(false);
        }
        yield return StartCoroutine(SecuenciaDia2());
    }

    // --- DÍA 2 ---
    public IEnumerator SecuenciaDia2()
    {
        diaActual = 2;
        reaccionZonaStalkerActivada = false;
        yaPenalizadoHoy = false;
        yield return StartCoroutine(MostrarCartelDia(2));

        // BRAINROT
        alumnoActual = brainrot;
        brainrot.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(brainrot.Aparecer());

        int faltas = PlayerPrefs.GetInt("FaltasTungTung", 0);

        if (faltas > 0)
        {
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(dialogoDia2Brainrot_molesto, false)));
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(dialogoDia2Brainrot, false)));
        }

        yield return StartCoroutine(brainrot.EntregarExamen(2));
        nota = brainrot.notaRecibida;

        if (nota < 4)
        {
            brainrot.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Ojalá te baneen en tis tos." }, false)));
        }
        else if (nota == 4 || nota == 3)
        {
            brainrot.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Muy poco skibidi de tu parte." }, false)));
        }
        else
        {
            brainrot.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Toma jeroma pastilla de goma." }, false)));
        }

        yield return StartCoroutine(brainrot.Desaparecer());
        brainrot.gameObject.SetActive(false);

        // GORDETE
        alumnoActual = gordete;
        gordete.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(gordete.Aparecer());

        if (manzana_comida == false)
        {
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Hola profe, sigo con un poco de hambre, ¿me darías la manzana?" }, false)));
            yield return StartCoroutine(ManejarPregunta(gordete.Preguntar(opcionesSiNo)));
            if (gordete.respuestaElegida == 0)
            {
                GameObject manzanaEnMesa = GameObject.FindWithTag("ManzanaMesa");
                if (manzanaEnMesa != null)
                {
                    Destroy(manzanaEnMesa);
                    manzana_comida = true;
                }
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "QUE DELICIA.", "Aunque el centro sabía un poco a lente con un ligero toque a batería de litio.", "En fin, lo que no mata engorda.", "Y si me mata, te vas a cagar, jeje." }, true)));
            }
            else if (gordete.respuestaElegida == 1)
            {
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Tú verás, cara de rosquilla." }, true)));
            }
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Hola profe, la manzana de ayer me sentó un poco mal.", "Cagué un objetivo Canon EF de cincuenta milímetros de longitud y uno coma ocho milímetros de apertura.", "No salió fácilmente." }, true)));
        }

        yield return StartCoroutine(gordete.EntregarExamen(2));
        nota = gordete.notaRecibida;

        if (nota == 4 || nota == 3)
        {
            gordete.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Si mi mamá se entera me deja sin la cuarta merienda." }, true)));
        }
        else if (nota < 4)
        {
            gordete.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(gordeteReaccionEnfadado, true)));
        }
        else
        {
            gordete.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¡¡QUÉ BIEN!! Hoy me daré un caprichito." }, true)));
        }

        yield return StartCoroutine(gordete.Desaparecer());
        gordete.gameObject.SetActive(false);

        // NEGRETE
        alumnoActual = negrete;
        negrete.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(negrete.Aparecer());

        yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Hola profe, te has pasado un poco con el examen de hoy." }, true)));

        yield return StartCoroutine(negrete.EntregarExamen(2));
        nota = negrete.notaRecibida;

        if (nota < 5)
        {
            negrete.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Jolines, es que no quería." }, true)));
        }
        else
        {
            negrete.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "No tienes ni idea de corregir, ¡qué bien!" }, true)));
        }

        yield return StartCoroutine(negrete.Desaparecer());
        negrete.gameObject.SetActive(false);

        // MEWING
        alumnoActual = mewing;
        mewing.gameObject.SetActive(true);
        
        mewing.animator.SetInteger("Dia", diaMewing);
            
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(mewing.Aparecer());

        if (estoyEmbelesado == false)
        {
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Hola profe, gracias al nuevo entrenamiento he conseguido estos rasgos perfectos." }, true)));
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "¿Dirías que ahora estás aún más hipnotizado por mi belleza?" }, true)));
            yield return StartCoroutine(ManejarPregunta(mewing.Preguntar(opcionesSiNo)));

            if (mewing.respuestaElegida == 0)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "¡BIEN, BIEN! He de seguir así." }, true)));
                estoyEmbelesado = true;
            }
            else if (mewing.respuestaElegida == 1)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Endureceré mis rutinas de looksmaxing al doble de tiempo." }, true)));
                estoyEmbelesado = false;
                diaMewing++;
            }
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Hola de nuevo, profe." }, true)));
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "¿Dirías que ahora te hipnotiza mi belleza?" }, true)));
            yield return StartCoroutine(ManejarPregunta(mewing.Preguntar(opcionesSiNo)));

            if (mewing.respuestaElegida == 0)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Todo mi esfuerzo está dando sus frutos." }, true)));
                estoyEmbelesado = true;
            }
            else if (mewing.respuestaElegida == 1)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Endureceré mis rutinas de looksmaxing al triple de tiempo." }, true)));
                estoyEmbelesado = false;
                diaMewing++;
            }
        }

        yield return StartCoroutine(mewing.EntregarExamen(2));
        nota = mewing.notaRecibida;

        if (nota >= 5)
        {
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Gracias, Mark. Si quieres te paso mi rutina." }, true)));
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Gracias profe, vuelvo a la rutina. (No parece que le importen las notas)" }, true)));
        }

        yield return StartCoroutine(mewing.Desaparecer());
        mewing.gameObject.SetActive(false);

        // STALKER
        alumnoActual = stalker;
        stalker.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(stalker.Aparecer());

        yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Hola profe, Lucía Fernanda me ha pedido que recoja firmas." }, true)));

        yield return StartCoroutine(stalker.EntregarExamen(2));
        nota = stalker.notaRecibida;
      
        if (nota < 5)
        {
            stalker.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Mejoraré por ti." }, true)));
        }
        else
        {
            stalker.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Me gusta que me pongas buena nota." }, true)));
        }

        yield return StartCoroutine(stalker.Desaparecer());
        stalker.gameObject.SetActive(false);
        // BASICOTE
        alumnoActual = basicote;
        basicote.gameObject.SetActive(true);

        if (entrar_niño != null)
        {
            audioSource.PlayOneShot(entrar_niño, 5f);
        }

        yield return StartCoroutine(basicote.Aparecer());

        yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Hola profe, que prefieres…" }, true)));

        yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "¿?" }, true)));

        yield return StartCoroutine(ManejarPregunta(basicote.Preguntar(opcionesBasicoteDia2)));
        decisionBasicoteDia2 = basicote.respuestaElegida;

        if (decisionBasicoteDia2 == 0)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "[RESPUESTA DE BASICOTE A LA OPCIÓN A]" }, true))); 
        }
        else if (decisionBasicoteDia2 == 1)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "[RESPUESTA DE BASICOTE A LA OPCIÓN B]" }, true)));
        }

        yield return StartCoroutine(basicote.EntregarExamen(2));
        nota = basicote.notaRecibida;

        if (nota < 4)
        {
            basicote.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Le voy a decir Aitor que te muerda los cojones." }, true)));
        }
        else if (nota == 4)
        {
            basicote.animator.SetTrigger("Enfadado");
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Gilipollas." }, true)));
        }
        else
        {
            basicote.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Suuuuuuu." }, true)));
        }

        yield return StartCoroutine(basicote.Desaparecer());
        basicote.gameObject.SetActive(false);

        // Lista 
        alumnoActual = repelente;
        repelente.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(repelente.Aparecer());

        yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "Buenos días señor Mark. Este examen debe de puntuarse con un diez." }, true)));

        yield return StartCoroutine(repelente.EntregarExamen(2));
        nota = repelente.notaRecibida;
        if (nota == 10)
        {
            diecesRepelente++;
        }
        if (nota == 10)
        {
            repelente.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "Ya me lo esperaba." }, true)));
        }
        else if (nota < 10||  nota > 4)
        {
            
             repelente.animator.SetTrigger("Enfadado");
             yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "¿PERDONA? ¡El examen está perfecto!" }, true)));
            
        }
        else
        {
            repelente.animator.SetTrigger("Enfadado");
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "¿CÓMO? Atente a las consecuencias." }, true)));
        }

        yield return StartCoroutine(repelente.Desaparecer());
        repelente.gameObject.SetActive(false);
        yield return StartCoroutine(SecuenciaDia3());

    }
    public IEnumerator SecuenciaDia3()
    {
        diaActual = 3;
        reaccionZonaStalkerActivada = false;
        yaPenalizadoHoy = false;
        yield return StartCoroutine(MostrarCartelDia(3));

        // STALKER
        alumnoActual = stalker;
        stalker.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(stalker.Aparecer());

        yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Lucía Fernanda me ha dicho que ayer no firmaste. Está un poco triste." }, true)));
        yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Espero que te guste la sorpresa del examen de hoy." }, true)));

        yield return StartCoroutine(stalker.EntregarExamen(3));
        nota = stalker.notaRecibida;

        if (nota < 4)
        {
            stalker.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Ya veo que no te ha gustado la sorpresa" }, true)));
        }
        else if (nota == 4 || nota == 5)
        {
            stalker.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Cuidado con lo que haces, no es una amenaza es un aviso." }, true)));
        }
        else
        {
            stalker.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Mañana te haré otra sorpresa ya que te ha gustado tanto." }, true)));
        }

        yield return StartCoroutine(stalker.Desaparecer());
        stalker.gameObject.SetActive(false);

        // GORDETE
        alumnoActual = gordete;
        gordete.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(gordete.Aparecer());

        if (manzana_comida == false)
        {
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Hola profe, sigo con un poco de hambre, ¿me darías la manzana?" }, false)));
            yield return StartCoroutine(ManejarPregunta(gordete.Preguntar(opcionesSiNo)));
            if (gordete.respuestaElegida == 0)
            {
                GameObject manzanaEnMesa = GameObject.FindWithTag("ManzanaMesa");
                if (manzanaEnMesa != null)
                {
                    Destroy(manzanaEnMesa);
                    manzana_comida = true;
                }
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "QUÉ DELICIA.", "Aunque el centro sabía un poco a lente con un ligero toque a batería de litio.", "En fin, lo que no mata engorda.", "Y si me mata, te vas a cagar, jeje." }, true)));
            }
            else if (gordete.respuestaElegida == 1)
            {
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Tú verás, cara de acelga." }, true)));
            }
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Hola profe, la manzana de ayer me sentó un poco mal.", "Cagué un objetivo Canon EF de cincuenta milímetros de longitud y uno coma ocho milímetros de apertura.", "No salió fácilmente." }, true)));
        }

        yield return StartCoroutine(gordete.EntregarExamen(3));
        nota = gordete.notaRecibida;

        if (nota == 4 || nota == 3)
        {
            gordete.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Si mi mamá se entera me deja sin la cuarta merienda." }, true)));
        }
        else if (nota < 3)
        {
            gordete.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(gordeteReaccionEnfadado, true)));
        }
        else
        {
            gordete.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¡¡QUÉ BIEN!! Hoy me daré un caprichito." }, true)));
        }

        yield return StartCoroutine(gordete.Desaparecer());
        gordete.gameObject.SetActive(false);
        // BASICOTE
        alumnoActual = basicote;
        basicote.gameObject.SetActive(true);

        if (entrar_niño != null) { audioSource.PlayOneShot(entrar_niño, 5f); }

        yield return StartCoroutine(basicote.Aparecer());
        yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Hola profe, tengo una super pregunta.", "¿Qué prefieres?" }, true)));

        // Opciones de respuesta.
        yield return StartCoroutine(ManejarPregunta(basicote.Preguntar(new string[] { "[OPCIÓN A]", "[OPCIÓN B]" })));

        // Reacción según la respuesta elegida.
        if (basicote.respuestaElegida == 0)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "[RESPUESTA DE BASICOTE A LA OPCIÓN A]" }, true)));
        }
        else if (basicote.respuestaElegida == 1)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "[RESPUESTA DE BASICOTE A LA OPCIÓN B]" }, true)));
        }
        yield return StartCoroutine(basicote.EntregarExamen(3));
        nota = stalker.notaRecibida;
        if (nota < 4)
        {
            basicote.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Vete a toMark por culo." }, true)));
        }
        else if (nota == 4)
        {
            basicote.animator.SetTrigger("Enfadado");
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "¿Sabes puto inglés?" }, true)));
        }
        else
        {
            basicote.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "VAMOS JODEEER" }, true)));
        }

        yield return StartCoroutine(basicote.Desaparecer());
        basicote.gameObject.SetActive(false);

        // MEWING
        alumnoActual = mewing;
        mewing.gameObject.SetActive(true);
        mewing.animator.SetInteger("Dia", diaMewing);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(mewing.Aparecer());

        if (estoyEmbelesado == true)
        {
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Hola profe, mis rasgos siguen igual de perfectos que ayer." }, true)));
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "¿Dirías que sigues hipnotizado por mi belleza?" }, true)));
            yield return StartCoroutine(ManejarPregunta(mewing.Preguntar(opcionesSiNo)));

            if (mewing.respuestaElegida == 0)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "¡BIEN, BIEN! He de seguir así." }, true)));
                estoyEmbelesado = true;
            }
            else if (mewing.respuestaElegida == 1)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Endureceré mis rutinas de looksmaxing al doble de tiempo." }, true)));
                estoyEmbelesado = false;
                diaMewing++;
            }
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Hola de nuevo, profe." }, true)));
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "¿Dirías que ahora te hipnotiza mi belleza?" }, true)));
            yield return StartCoroutine(ManejarPregunta(mewing.Preguntar(opcionesSiNo)));

            if (mewing.respuestaElegida == 0)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Todo mi esfuerzo está dando sus frutos." }, true)));
                estoyEmbelesado = true;
            }
            else if (mewing.respuestaElegida == 1)
            {
                yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Endureceré mis rutinas de looksmaxing al triple de tiempo." }, true)));
                estoyEmbelesado = false;
                diaMewing++;
            }
        }

        yield return StartCoroutine(mewing.EntregarExamen(3));
        nota = mewing.notaRecibida;

        if (nota >= 5)
        {
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Gracias, Mark. Si quieres te paso mi rutina." }, true)));
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Gracias profe. Me voy a hacer mi rutina de mewing (Sigue sin importarle nada a parte de su físico)", "Se ha hecho tarde,no me dará tiempo a grabar tis tos y cumplir las 6 horas de mewing (No parece que le importe la nota)." }, true)));
        }

        yield return StartCoroutine(mewing.Desaparecer());
        mewing.gameObject.SetActive(false);

        // Lista 
        alumnoActual = repelente;
        repelente.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(repelente.Aparecer());

        yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "Buenos días señor Mark. Recuerde que hoy viene mi padre a una reunión. " }, true)));

        yield return StartCoroutine(repelente.EntregarExamen(3));
        nota = repelente.notaRecibida;
        if (nota == 10)
        {
            diecesRepelente++;
        }
        if (nota == 10)
        {
            repelente.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "Mi año viviendo en Gran Bretaña ha dado sus frutos." }, true)));
        }
        else if (nota < 10 || nota > 4)
        {

            repelente.animator.SetTrigger("Enfadado");
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "¿QUÉ? Esta tarde tendrás más que palabras con mi padre." }, true)));

        }
        else
        {
            repelente.animator.SetTrigger("Enfadado");
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "¿CÓMO? Mi padre va a desintegrarte." }, true)));
        }

        yield return StartCoroutine(repelente.Desaparecer());
        repelente.gameObject.SetActive(false);

        // BRAINROT
        alumnoActual = brainrot;
        brainrot.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(brainrot.Aparecer());

        int faltas = PlayerPrefs.GetInt("FaltasTungTung", 0);

        if (faltas ==1)
        {
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(dialogoDia3Brainrot_molesto, false)));
        }
        else if(faltas ==2 )
        {
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(dialogoDia3Brainrot_enfadado, false)));
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(dialogoDia3Brainrot, false)));
        }

        yield return StartCoroutine(brainrot.EntregarExamen(3));
        nota = brainrot.notaRecibida;

        if (nota < 4)
        {
            brainrot.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Ojalá te baneen en todas las redes sociales." }, false)));
        }
        else if (nota == 4 || nota == 3)
        {
            brainrot.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Debería haber estudiado viendo tis tos en inglés." }, false)));
        }
        else
        {
            brainrot.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Holy moly profe, vaya aura de corrección." }, false)));
        }

        yield return StartCoroutine(brainrot.Desaparecer());
        brainrot.gameObject.SetActive(false);

        // NEGRETE
        alumnoActual = negrete;
        negrete.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(negrete.Aparecer());

        yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Hola profe, ¿has visto a mi hámster? Creo que se ha escapado." }, true)));

        yield return StartCoroutine(negrete.EntregarExamen(3));
        nota = negrete.notaRecibida;

        if (nota < 4)
        {
            negrete.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Bua, con lo que me gusta a mí jugar y acariciar a mi car aunque suelte tanto pelo." }, true)));
        }
        else if (nota ==3 || nota ==4)
        {
            negrete.animator.SetTrigger("Triste");
            yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Oh no, con lo que me gustan a mí los hauses para merendar." }, true)));
        }
        else
        {
            negrete.animator.SetTrigger("Feliz");
            yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Sabía que gato y plátano se escribían así, soy la monda." }, true)));
        }

        yield return StartCoroutine(negrete.Desaparecer());
        negrete.gameObject.SetActive(false);

        // ------- PADRE DE LA LISTILLA -------
        
        alumnoActual = padreLista;
        padreLista.gameObject.SetActive(true);

        if (entrar_niño != null)
        {
                audioSource.PlayOneShot(entrar_niño, 5f);
        }

        yield return StartCoroutine(padreLista.Aparecer());
        yield return StartCoroutine(ManejarDialogo(padreLista.Decir(new string[] { "Hola buenos días, soy el padre de Sara. Como ya le comenté vengo a revisar las notas de los últimos exámenes. Sara lleva estudiando desde el año pasado para asegurarse de que saca dieces en todo. ¿Me los podría enseñar?\r\n" }, true)));
            // El padre reacciona según los dieces conseguidos.
        switch (diecesRepelente) 
        { 
            case 0:yield return StartCoroutine(ManejarDialogo(padreLista.Decir( new string[]{"Ni un solo dies es usted un incompetente, ya me he cansado."},true)));break;

            case 1:yield return StartCoroutine(ManejarDialogo(padreLista.Decir(new string[]{ "¿Y estos exámenes sin dieces? Sus correcciones son un insulto para mi familia. " },true))); break;

            case 2: yield return StartCoroutine(ManejarDialogo(padreLista.Decir( new string[] { "¿Y esta nota? Todo lo que esté por debajo del 10 es una decepción." },true) ));break;

            case 3:yield return StartCoroutine( ManejarDialogo(padreLista.Decir(new string[]{ "Veo que no es necesario nada más que una enhorabuena por sus clases y los resultados de Sara. Que tenga buen día.\r\n" },true)));break;
        }

            // Si no tiene los tres dieces, el padre provoca el Game Over.
        if (diecesRepelente < 3)
        {
            yield return StartCoroutine(EjecutarMuerte(padreLista));
            yield break;
        }

            // Si tiene los tres dieces, el padre termina su turno sin matarte.
        yield return StartCoroutine(padreLista.Desaparecer());
        padreLista.gameObject.SetActive(false);
        
    }

    private void ForzarSoltarHerramientas()
    {
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

    public IEnumerator MostrarCartelDia(int numeroDia)
    {
        if (objetoCartelDias != null)
        {
            objetoCartelDias.SetActive(true);
            yield return null;
            objetoCartelDias.GetComponent<Animator>().SetInteger("Dia", numeroDia);
            yield return new WaitForSeconds(duracionCartel);
            objetoCartelDias.SetActive(false);
        }
    }

  

    // =========================================================
    // SISTEMA MAESTRO DE MUERTES
    // =========================================================
    public IEnumerator EjecutarMuerte(ScriptAlumno asesino)
    {
        GameObject panelAsesino = null;

        if (asesino != null)
        {
            panelAsesino = asesino.panelMuerte;

            asesino.DesplegarPanelMuerte(); 
            float tiempoEspera = asesino.duracionAnimacionMuerte; 

            Destroy(asesino.gameObject); 
            alumnoActual = null;

            yield return new WaitForSeconds(tiempoEspera); 
        }

        if (panelAsesino != null)
        {
            panelAsesino.SetActive(false);
        }

        ActivarPantallaMuerte(); 
    }

    private IEnumerator SecuenciaMuerteTungTung()
    {
        // 1. Primero dice su frase amenazante y esperamos a que termine
        yield return StartCoroutine(ManejarDialogo(alumnoActual.Decir(new string[] { "TÚ TE LO HAS BUSCADO.", "HAS INTENTADO DESTROZAR A TUNG-TUNG TRES VECES.", "AHORA SUFRIRÁS SU IRA." }, false)));

        // 2. Una vez que ha terminado de hablar y la boca se para, ENTONCES lo matamos
        yield return StartCoroutine(EjecutarMuerte(brainrot));
    }

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
            StartCoroutine(SecuenciaMuerteTungTung());
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

    // Muerte de Stalker al tocar su zona especial.
    public void ActivarReaccionZonaStalker()
    {
        // Solo funciona durante el día 2.
        if (diaActual != 2) return;

        // Stalker debe ser el alumno actual.
        if (alumnoActual != stalker) return;

        if (stalker == null || !stalker.gameObject.activeInHierarchy)
            return;

        // Evita activar la muerte varias veces.
        if (reaccionZonaStalkerActivada) return;

        // No interrumpimos otros diálogos o preguntas.
        if (dialogoEnCurso || esperandoRespuesta) return;

        // Marcamos la muerte como activada.
        reaccionZonaStalkerActivada = true;

        // Ejecutamos el sistema de muerte existente.
        StartCoroutine(EjecutarMuerte(stalker));
    }
    public void ActivarPantallaMuerte()
    {
        if (panelPantallaMuerte != null)
        {
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