using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

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
    public ScriptAlumno cartero;

    [Header("Padre de la Listilla")]
    public int diecesRepelente = 0;

    [Header("Sonidos")]
    public AudioClip entrar_niño;
    public AudioClip entrar_niña;

    [Header("Sonidos_Basicote")]
    public AudioClip Triste_basicote;
    public AudioClip Risa_basicote;
    public AudioClip Enfado_basicote;

    [Header("Sonidos_brainrot")]
    public AudioClip Triste_brainrot;
    public AudioClip Risa_brainrot;
    public AudioClip Hablar_brainrot;

    [Header("Sonidos_Lista")]
    public AudioClip hablar_Lista;
    public AudioClip Enfado_Lista;
    public AudioClip Risa_Lista;


    [Header("Sonidos_gordete")]
    public AudioClip Triste_gordete;
    public AudioClip Risa_gordete;
    public AudioClip Enfado_gordete;
    public AudioClip Hablar_gordete;
    public AudioClip Faliz_gordete;
    public AudioClip comer_manzana;

    [Header("Sonidos_stalker")]
    public AudioClip Triste_stalker;
    public AudioClip Risa_stalker;
    public AudioClip Enfado_stalker;
    public AudioClip Hablar_stalker;
    public AudioClip dar_manzana;

    [Header("Sonidos_mewing")]
    public AudioClip mewingDia1;
    public AudioClip mewingDia2;
    public AudioClip mewingDia3;
    public AudioClip mewingDia4;

    [Header("Sonidos_negrete")]
    public AudioClip Triste_negrete;
    public AudioClip Risa_negrete;
    public AudioClip Hablar_negrete;

    [Header("Sonidos_finales")]
    public AudioClip Final_basicote_camion;
    public AudioClip Final_basicote_bomba;
    public AudioClip Final_brainrot;
    public AudioClip Final_lista;
    public AudioClip Final_stalker;
    public AudioClip Final_mewing;
    public AudioClip Final_negrete;
    public AudioClip Final_gordete;
    public AudioClip Final_Pantalla;
    public AudioClip Victoria;
    public AudioClip juego_negrete;
    public AudioClip Fuego;
    public AudioClip entregarCarta;
    public AudioClip dias;



    public AudioSource audioSource;
    public AudioSource audioSource2;

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
    public GameObject panelEntreDias;

    public GameObject panelCartaAmpliada;
    [HideInInspector] public bool cartaLeida = false;

    [Header("Paneles Extra")]
    public GameObject miPanelEspecifico;

    [Header("Paneles Evento Negrete")]
    public GameObject panelJuegoNegrete;
    public GameObject panelFuegoNegrete;
    private bool panelJuegoNegreteCerrado = false;
    private bool botonFuegoPulsado = false;

    [Header("Paneles Tutorial Día 1")]
    public GameObject panelTutorialBoli;
    public GameObject panelTutorialSellos;
    private bool panelBoliCerrado = false;
    private bool panelSellosCerrado = false;


    [Header("Textos Día 1 - Basicote")]
    public string[] dialogoPresentacionBasicote = { "..." };
    public string[] basicoteReaccionFeliz = { "¡Bua, un aprobadillo! ¡Qué grande eres!", "Me voy al parque." };
    public string[] basicoteReaccionEnfadado = { "¿Un suspenso? ¿En serio?", "Vaya asco de corrección." };
    public string[] basicoteReaccionTriste = { "" };

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
    public GameObject panelPantallaVictoria;
    public TextMeshProUGUI textoGameOver; 
    public float velocidadEscritura = 0.05f;


    [Header("Variables varias")]
    public int nota = 0;
    public bool manzana_comida = false;
    public string[] opcionesSiNo = { "Sí", "No" };
    public bool estoyEmbelesado = false;
    public int diaMewing = 1;

    [Header("Pregunta de Basicote - Día 2")]
    public int decisionBasicoteDia2 = -1;
    public string[] opcionesBasicoteDia2 = {"Camión", "Explosión"};
   
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

                // ==========================================
                // CLIC EN LA CARTA O EN EL CARTERO
                // ==========================================
                if (hit.CompareTag("Carta"))
                {
                    if (cartaLeida) return; // Evita doble clics

                    if (panelCartaAmpliada != null) panelCartaAmpliada.SetActive(true);
                    hit.gameObject.SetActive(false); // Escondemos el sobre de la mesa

                    if (entregarCarta != null) audioSource.PlayOneShot(entregarCarta, 1.3f);
                }
            }
        }
    }

    // --- EL GUION DEL DÍA 1 ---
    public IEnumerator SecuenciaDia1()
    {
        audioSource2.Play();
        yield return new WaitForSeconds(1.5f);
        diaMewing = 1;
        diaActual = 1;
        alumnoActual = basicote;
        basicote.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);

        yield return StartCoroutine(basicote.Aparecer());

        // 1. TEXTOS ANTES DEL PANEL DEL BOLI
        yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] {
            "¿Entonces tú eres Mark, el nuevo profesor?",
            "¡Qué bien!",
            "¿Que YO te tengo que enseñar a corregir? Hoy en día contratan a cualquiera.",
            "Usa el boli rojo para corregir el examen.",
            "Puedes hacer anotaciones, dibujos, comentarios... Lo que necesites."
        }, false)));

        panelBoliCerrado = false;
        if (panelTutorialBoli != null) panelTutorialBoli.SetActive(true);
        yield return new WaitUntil(() => panelBoliCerrado);

        yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] {
            "Puedes poner pegatinas a los exámenes si te apetece.",
            "Haz click sobre el examen para ver una vista ampliada y muévelo con la rueda y el click derecho.",
            "Y haz click sobre el bocadillo saliente de los alumnos si quieres interactuar con nosotros.",
            "Cuando hayas terminado, ponle una nota al examen con los sellos para entregárnoslo."
        }, false)));

        panelSellosCerrado = false;
        if (panelTutorialSellos != null) panelTutorialSellos.SetActive(true);
        yield return new WaitUntil(() => panelSellosCerrado);

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
                if (Risa_brainrot != null) audioSource.PlayOneShot(Risa_brainrot, 1.3f);

                yield return StartCoroutine(ManejarDialogo(brainrot.Decir(brainrotReaccionFeliz, false)));
            }
            else if (nota == 4 || nota == 3)
            {
                brainrot.animator.SetTrigger("Triste");
                if (Triste_brainrot != null) audioSource.PlayOneShot(Triste_brainrot, 1.3f);
                yield return StartCoroutine(ManejarDialogo(brainrot.Decir(brainrotReaccionEnfadado, false)));
            }
            else
            {
                brainrot.animator.SetTrigger("Triste");
                if (Triste_brainrot != null) audioSource.PlayOneShot(Triste_brainrot, 1.3f);
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
                repelente.animator.SetTrigger("Feliz");
                if (Risa_Lista != null) audioSource.PlayOneShot(Risa_Lista, 1.3f);

                yield return StartCoroutine(ManejarDialogo(repelente.Decir(repelenteReaccion10, false)));
                diecesRepelente++;
            }
            else if (nota >= 5)
            {
                repelente.animator.SetTrigger("Enfadado");
                if (Enfado_Lista != null) audioSource.PlayOneShot(Enfado_Lista, 10);

                yield return StartCoroutine(ManejarDialogo(repelente.Decir(repelenteReaccionAprobado, false)));
                if (repelente.animator != null) repelente.animator.SetTrigger("enfado2");
            }
            else
            {
                repelente.animator.SetTrigger("Enfadado");
                if (Enfado_Lista != null) audioSource.PlayOneShot(Enfado_Lista, 10);
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
                if (dar_manzana != null) audioSource.PlayOneShot(dar_manzana, 1.3f);
                manzanaCreada.tag = "ManzanaMesa";
            }

            yield return StartCoroutine(ManejarDialogo(stalker.Decir(dialogoPostManzanaStalker, false)));
            yield return StartCoroutine(stalker.EntregarExamen(1));

            nota = stalker.notaRecibida;

            if (nota < 5)
            {
                stalker.animator.SetTrigger("Triste");
                if (Triste_stalker != null) audioSource.PlayOneShot(Triste_stalker, 10f);
                yield return StartCoroutine(ManejarDialogo(stalker.Decir(stalkerSuspenso, false)));
            }
            else if (nota == 5)
            {
                stalker.animator.SetTrigger("Feliz");
                if (Risa_stalker != null) audioSource.PlayOneShot(Risa_stalker, 12f);
                yield return StartCoroutine(ManejarDialogo(stalker.Decir(stalker_5, false)));
            }
            else
            {
                stalker.animator.SetTrigger("Feliz");
                if (Risa_stalker != null) audioSource.PlayOneShot(Risa_stalker, 10f);
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
                if (Risa_negrete != null) audioSource.PlayOneShot(Risa_negrete, 1.3f);
                yield return StartCoroutine(ManejarDialogo(negrete.Decir(negreteReaccionFeliz, false)));
            }
            else if (nota == 4 || nota == 3)
            {
                negrete.animator.SetTrigger("Triste");
                if (Triste_negrete != null) audioSource.PlayOneShot(Triste_negrete, 1.3f);
                yield return StartCoroutine(ManejarDialogo(negrete.Decir(negreteReaccionEnfadado, false)));
            }
            else
            {
                negrete.animator.SetTrigger("Triste");
                if (Triste_negrete != null) audioSource.PlayOneShot(Triste_negrete, 1.3f);
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
                    if (comer_manzana != null) audioSource.PlayOneShot(comer_manzana, 1.3f);
                    manzana_comida = true;
                }
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "QUE DELICIA.", "Aunque el centro sabía un poco a lente, con un ligero toque a batería de litio.", "En fin, lo que no mata engorda.", "Y si me mata, te vas a cagar, jeje." }, true)));
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
                if (Enfado_gordete != null) audioSource.PlayOneShot(Enfado_gordete, 1.3f);
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¿¡Me has dejado sin comer y encima me pones un cero!?", "Te vas a arrepentir, cabrón." }, false)));
                if (Final_gordete != null) audioSource.PlayOneShot(Final_gordete, 1.3f);
                yield return StartCoroutine(EjecutarMuerte(gordete)); 
                yield break;
            }

            if (nota >= 5)
            {
                gordete.animator.SetTrigger("Feliz");
                if (Faliz_gordete != null) audioSource.PlayOneShot(Faliz_gordete, 1.3f);
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(gordeteReaccionFeliz, false)));
            }
            else if (nota == 4 || nota == 3)
            {
                gordete.animator.SetTrigger("Triste");
                if (Triste_gordete != null) audioSource.PlayOneShot(Triste_gordete, 1.3f);
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(gordeteReaccionEnfadado, false)));
            }
            else
            {
                gordete.animator.SetTrigger("Triste");
                if (Triste_gordete != null) audioSource.PlayOneShot(Triste_gordete, 1.3f);
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
        audioSource2.Play();

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
            if (Triste_brainrot != null) audioSource.PlayOneShot(Triste_brainrot, 1.3f);
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Ojalá te baneen en tis tos." }, false)));
        }
        else if (nota == 4 || nota == 3)
        {
            brainrot.animator.SetTrigger("Triste");
            if (Triste_brainrot != null) audioSource.PlayOneShot(Triste_brainrot, 1.3f);
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Muy poco skibidi de tu parte." }, false)));
        }
        else
        {
            brainrot.animator.SetTrigger("Feliz");
            if (Risa_brainrot != null) audioSource.PlayOneShot(Risa_brainrot, 1.3f);
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
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Hola, profe. Sigo con un poco de hambre, ¿me darías la manzana?" }, false)));
            yield return StartCoroutine(ManejarPregunta(gordete.Preguntar(opcionesSiNo)));
            if (gordete.respuestaElegida == 0)
            {
                GameObject manzanaEnMesa = GameObject.FindWithTag("ManzanaMesa");
                if (manzanaEnMesa != null)
                {
                    Destroy(manzanaEnMesa);
                    if (comer_manzana != null) audioSource.PlayOneShot(comer_manzana, 1.3f);
                    manzana_comida = true;
                }
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "QUE DELICIA.", "Aunque el centro sabía un poco a lente, con un ligero toque a batería de litio.", "En fin, lo que no mata engorda.", "Y si me mata, te vas a cagar, jeje." }, true)));
            }
            else if (gordete.respuestaElegida == 1)
            {
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Tú verás, cara de rosquilla." }, true)));
            }
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Hola, profe. Desde que me comí la manzana no me siento muy bien.", "Cagué un objetivo Canon EF de cincuenta milímetros de longitud y uno coma ocho milímetros de apertura.", "No salió fácilmente." }, true)));
        }

        yield return StartCoroutine(gordete.EntregarExamen(2));
        nota = gordete.notaRecibida;

        if (nota == 4 || nota == 3)
        {
            gordete.animator.SetTrigger("Triste");
            if (Triste_gordete != null) audioSource.PlayOneShot(Triste_gordete, 1.3f);
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Si mi mamá se entera me deja sin la cuarta merienda." }, true)));
        }
        else if (nota < 4)
        {
            gordete.animator.SetTrigger("Triste");
            if (Triste_gordete != null) audioSource.PlayOneShot(Triste_gordete, 1.3f);
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(gordeteReaccionEnfadado, true)));
        }
        else
        {
            gordete.animator.SetTrigger("Feliz");
            if (Risa_gordete != null) audioSource.PlayOneShot(Risa_gordete, 1.3f);
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¡¡QUÉ BIEN!! Hoy me daré un caprichito." }, true)));
        }

        yield return StartCoroutine(gordete.Desaparecer());
        gordete.gameObject.SetActive(false);

        // NEGRETE
        alumnoActual = negrete;
        negrete.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(negrete.Aparecer());

        yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Hola, profe. Te has pasado un poco con el examen de hoy." }, true)));

        yield return StartCoroutine(negrete.EntregarExamen(2));
        nota = negrete.notaRecibida;

        if (nota < 5)
        {
            negrete.animator.SetTrigger("Triste");
            if (Triste_negrete != null) audioSource.PlayOneShot(Triste_negrete, 1.3f);

            yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Jolines, es que no quería." }, true)));
        }
        else
        {
            negrete.animator.SetTrigger("Feliz");
            if (Risa_negrete != null) audioSource.PlayOneShot(Risa_negrete, 1.3f);
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
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Hola, profe. Gracias al nuevo entrenamiento he conseguido estos rasgos perfectos.", "¿Dirías que ahora estás aún más hipnotizado por mi belleza?" }, true)));
           
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
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Hola de nuevo, profe.", "¿Dirías que ahora te hipnotiza mi belleza?" }, true)));
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
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Gracias, profe. Vuelvo a la rutina. (No parece que le importen las notas)" }, true)));
        }

        yield return StartCoroutine(mewing.Desaparecer());
        mewing.gameObject.SetActive(false);

        // STALKER
        alumnoActual = stalker;
        stalker.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(stalker.Aparecer());

        yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Hola, profe. Lucía Fernanda me ha pedido que recoja firmas." }, true)));

        yield return StartCoroutine(stalker.EntregarExamen(2));
        nota = stalker.notaRecibida;

        if (nota < 5)
        {
            stalker.animator.SetTrigger("Triste");
            if (Triste_stalker != null) audioSource.PlayOneShot(Triste_stalker, 1.3f);
            yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Mejoraré por ti." }, true)));
        }
        else
        {
            stalker.animator.SetTrigger("Feliz");
            if (Risa_stalker != null) audioSource.PlayOneShot(Risa_stalker, 1.3f);
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

        yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Hola profe, ¿qué prefieres?", "¿Morir atropellado por un camión, o morir en una explosión?" }, true)));

        yield return StartCoroutine(ManejarPregunta(basicote.Preguntar(opcionesBasicoteDia2)));
        decisionBasicoteDia2 = basicote.respuestaElegida;

        if (decisionBasicoteDia2 == 0)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Hmm... Ya veo." }, true)));
            basicote.panelMuerte = basicote.panelMuerteAlternativo;
        }
        else if (decisionBasicoteDia2 == 1)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Hmm... Ya veo." }, true)));
        }

        yield return StartCoroutine(basicote.EntregarExamen(2));
        nota = basicote.notaRecibida;

        if (nota < 4)
        {
            basicote.animator.SetTrigger("Triste");
            if (Triste_basicote != null) audioSource.PlayOneShot(Triste_basicote, 1.3f);
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Le voy a decir Aitor que te muerda los cojones." }, true)));
        }
        else if (nota == 4)
        {
            basicote.animator.SetTrigger("Enfadado");
            if (Enfado_basicote != null) audioSource.PlayOneShot(Enfado_basicote, 1.3f);
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Gilipollas." }, true)));
        }
        else
        {
            basicote.animator.SetTrigger("Feliz");
            if (Risa_basicote != null) audioSource.PlayOneShot(Risa_basicote, 1.3f);
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "SUUUUUUUUUUU." }, true)));
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
            repelente.animator.SetTrigger("Feliz");
            if (Risa_Lista != null) audioSource.PlayOneShot(Risa_Lista, 1.3f);

            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "Ya me lo esperaba." }, true)));
            diecesRepelente++;
        }
        else if (nota < 10||  nota > 4)
        {
            
             repelente.animator.SetTrigger("Enfadado");
            if (Enfado_Lista != null) audioSource.PlayOneShot(Enfado_Lista, 1.3f);
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "¡¿PERDONA?! ¡El examen está perfecto!" }, true)));
            
        }
        else
        {
            repelente.animator.SetTrigger("Enfadado");
            if (Enfado_Lista != null) audioSource.PlayOneShot(Enfado_Lista, 1.3f);
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "¡¿CÓMO?! Atente a las consecuencias." }, true)));
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
        audioSource2.Play();

        // STALKER
        alumnoActual = stalker;
        stalker.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(stalker.Aparecer());

        yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Lucía Fernanda me ha dicho que ayer no firmaste. Está un poco triste.", "Espero que te guste la sorpresa del examen de hoy." }, true)));

        yield return StartCoroutine(stalker.EntregarExamen(3));
        nota = stalker.notaRecibida;

        if (nota < 4)
        {
            stalker.animator.SetTrigger("Triste");
            if (Triste_stalker != null) audioSource.PlayOneShot(Triste_stalker, 1.3f);
            yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Ya veo que no te ha gustado la sorpresa." }, true)));
        }
        else if (nota == 4 || nota == 5)
        {
            stalker.animator.SetTrigger("Triste");
            if (Triste_stalker != null) audioSource.PlayOneShot(Triste_stalker, 1.3f);

            yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Cuidado con lo que haces. No es una amenaza, es un aviso." }, true)));
        }
        else
        {
            stalker.animator.SetTrigger("Feliz");
            if (Risa_stalker != null) audioSource.PlayOneShot(Risa_stalker, 1.3f);

            yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Mañana te haré otra sorpresa, ya que te ha gustado tanto esta." }, true)));
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
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Hola, profe. Sigo con un poco de hambre, ¿me darías la manzana?" }, false)));
            yield return StartCoroutine(ManejarPregunta(gordete.Preguntar(opcionesSiNo)));
            if (gordete.respuestaElegida == 0)
            {
                GameObject manzanaEnMesa = GameObject.FindWithTag("ManzanaMesa");
                if (manzanaEnMesa != null)
                {
                    Destroy(manzanaEnMesa);
                    if (comer_manzana != null) audioSource.PlayOneShot(comer_manzana, 1.3f);
                    manzana_comida = true;
                }
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "QUÉ DELICIA.", "Aunque el centro sabía un poco a lente, con un ligero toque a batería de litio.", "En fin, lo que no mata engorda.", "Y si me mata, te vas a cagar, jeje." }, true)));
            }
            else if (gordete.respuestaElegida == 1)
            {
                if (Enfado_gordete != null) audioSource.PlayOneShot(Enfado_gordete, 1.3f);
                yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Tú lo has querido, cara de acelga." }, false)));
                if (Final_gordete != null) audioSource.PlayOneShot(Final_gordete, 1.3f);
                yield return StartCoroutine(EjecutarMuerte(gordete));
            }
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Hola, profe. Desde que me comí la manzana no me siento muy bien.", "Cagué un objetivo Canon EF de cincuenta milímetros de longitud y uno coma ocho milímetros de apertura.", "No salió fácilmente." }, true)));
        }

        yield return StartCoroutine(gordete.EntregarExamen(3));
        nota = gordete.notaRecibida;

        if (nota == 4 || nota == 3)
        {
            gordete.animator.SetTrigger("Triste");
            if (Triste_gordete != null) audioSource.PlayOneShot(Triste_gordete, 1.3f);
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Si mi mamá se entera me deja sin la cuarta merienda." }, true)));
        }
        else if (nota < 3)
        {
            gordete.animator.SetTrigger("Triste");
            if (Triste_gordete != null) audioSource.PlayOneShot(Triste_gordete, 1.3f);
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(gordeteReaccionEnfadado, true)));
        }
        else
        {
            gordete.animator.SetTrigger("Feliz");
            if (Risa_gordete != null) audioSource.PlayOneShot(Risa_gordete, 1.3f);
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¡¡QUÉ BIEN!! Hoy me daré un caprichito." }, true)));
        }

        yield return StartCoroutine(gordete.Desaparecer());
        gordete.gameObject.SetActive(false);

        // BASICOTE
        alumnoActual = basicote;
        basicote.gameObject.SetActive(true);

        if (entrar_niño != null) { audioSource.PlayOneShot(entrar_niño, 5f); }

        yield return StartCoroutine(basicote.Aparecer());
        yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Hola profe, tengo una súper pregunta.", "Qué prefieres, apostar tu familia y amigos en una tirada de la ruleta al rojo o al negro" }, true)));

        // Opciones de respuesta.
        yield return StartCoroutine(ManejarPregunta(basicote.Preguntar(new string[] { "Rojo", "Negro" })));

        // Reacción según la respuesta elegida.
        if (basicote.respuestaElegida == 0)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Racista." }, true)));
        }
        else if (basicote.respuestaElegida == 1)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "A ti te gusta el chocolate ehh." }, true)));
        }
        yield return StartCoroutine(basicote.EntregarExamen(3));
        nota = basicote.notaRecibida;
        if (nota < 4)
        {
            basicote.animator.SetTrigger("Triste");
            if (Triste_basicote != null) audioSource.PlayOneShot(Triste_basicote, 1.3f);

            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Vete a toMark por culo." }, true)));
        }
        else if (nota == 4)
        {
            basicote.animator.SetTrigger("Enfadado");
            if (Enfado_basicote != null) audioSource.PlayOneShot(Enfado_basicote, 1.3f);
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "¿Acaso no sabes puto inglés?" }, true)));
        }
        else
        {
            basicote.animator.SetTrigger("Feliz");
            if (Risa_basicote != null) audioSource.PlayOneShot(Risa_basicote, 1.3f);
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "VAMOS, JODER." }, true)));
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
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Hola, profe. Mis rasgos siguen igual de perfectos que ayer.", "¿Dirías que sigues hipnotizado por mi belleza?" }, true)));
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
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Hola de nuevo, profe.", "¿Dirías que ahora te hipnotiza mi belleza?" }, true)));
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
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Gracias, profe. Me voy a hacer mi rutina de mewing. (Sigue sin importarle nada a parte de su físico)", "Se ha hecho tarde,no me dará tiempo a grabar tis tos y cumplir las seis horas de mewing (No parece que le importe la nota)" }, true)));
        }

        yield return StartCoroutine(mewing.Desaparecer());
        mewing.gameObject.SetActive(false);

        // Lista 
        alumnoActual = repelente;
        repelente.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(repelente.Aparecer());

        yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "Buenos días, señor Mark. Recuerde que hoy viene mi padre a una reunión. " }, true)));

        yield return StartCoroutine(repelente.EntregarExamen(3));
        nota = repelente.notaRecibida;
       
        if (nota == 10)
        {
            repelente.animator.SetTrigger("Feliz");
            if (Risa_Lista != null) audioSource.PlayOneShot(Risa_Lista, 1.3f);

            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "Mi año viviendo en Gran Bretaña ha dado sus frutos." }, true)));
            diecesRepelente++;
        }
        else if (nota < 10 || nota > 4)
        {

            repelente.animator.SetTrigger("Enfadado");
            if (Enfado_Lista != null) audioSource.PlayOneShot(Enfado_Lista, 1.3f);
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "¡¿QUÉ?! Esta tarde tendrás más que palabras con mi padre." }, true)));

        }
        else
        {
            repelente.animator.SetTrigger("Enfadado");
            if (Enfado_Lista != null) audioSource.PlayOneShot(Enfado_Lista, 1.3f);
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "¡¿CÓMO?! Mi padre va a desintegrarte." }, true)));
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
            if (Triste_brainrot != null) audioSource.PlayOneShot(Triste_brainrot, 1.3f);
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Ojalá te baneen en todas las redes sociales." }, false)));
        }
        else if (nota == 4 || nota == 3)
        {
            brainrot.animator.SetTrigger("Triste");
            if (Triste_brainrot != null) audioSource.PlayOneShot(Triste_brainrot, 1.3f);
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Debería haber estudiado viendo tis tos en inglés." }, false)));
        }
        else
        {
            brainrot.animator.SetTrigger("Feliz");
            if (Risa_brainrot != null) audioSource.PlayOneShot(Risa_brainrot, 1.3f);
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Holy moly profe, vaya aura de corrección." }, false)));
        }

        yield return StartCoroutine(brainrot.Desaparecer());
        brainrot.gameObject.SetActive(false);

        // NEGRETE
        alumnoActual = negrete;
        negrete.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(negrete.Aparecer());

        yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Hola, profe. ¿Has visto a mi hámster? Creo que se ha escapado." }, true)));

        yield return StartCoroutine(negrete.EntregarExamen(3));
        nota = negrete.notaRecibida;

        if (nota < 4)
        {
            negrete.animator.SetTrigger("Triste");
            if (Triste_negrete != null) audioSource.PlayOneShot(Triste_negrete, 1.3f);
            yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Bua, con lo que me gusta a mí jugar y acariciar a mi car aunque suelte tanto pelo." }, true)));
        }
        else if (nota ==3 || nota ==4)
        {
            negrete.animator.SetTrigger("Triste");
            if (Triste_negrete != null) audioSource.PlayOneShot(Triste_negrete, 1.3f);
            yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Oh, no, con lo que me gustan a mí los hauses para merendar." }, true)));
        }
        else
        {
            negrete.animator.SetTrigger("Feliz");
            if (Risa_negrete != null) audioSource.PlayOneShot(Risa_negrete, 1.3f);
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
        yield return StartCoroutine(ManejarDialogo(padreLista.Decir(new string[] { "Hola, buenos días, soy el padre de Sara.", "Como ya le comenté, vengo a revisar las notas de los últimos exámenes de mi hija.", "Sara lleva estudiando desde el año pasado para asegurarse de sacar dieces en todo.", "¿Me los podría enseñar?","..."}, true)));
            // El padre reacciona según los dieces conseguidos.
        switch (diecesRepelente) 
        { 
            case 0:yield return StartCoroutine(ManejarDialogo(padreLista.Decir( new string[]{"Ni un solo diez. Es usted un incompetente. Ya me he cansado."},true)));break;

            case 1:yield return StartCoroutine(ManejarDialogo(padreLista.Decir(new string[]{ "¿Y estos exámenes sin dieces? Sus correcciones son un insulto para mi familia. " },true))); break;

            case 2: yield return StartCoroutine(ManejarDialogo(padreLista.Decir( new string[] { "¿Y esta nota? Todo lo que esté por debajo del 10 es una decepción." },true) ));break;

            case 3:yield return StartCoroutine( ManejarDialogo(padreLista.Decir(new string[]{ "Veo que no es necesario nada más que una enhorabuena por sus clases y los resultados de Sara. Que tenga buen día.\r\n" },true)));break;
        }

            // Si no tiene los tres dieces, el padre provoca el Game Over.
        if (diecesRepelente < 3)
        {
            if (Final_lista != null) audioSource.PlayOneShot(Final_lista, 1.3f);
            yield return StartCoroutine(EjecutarMuerte(padreLista));
            yield break;
        }

            // Si tiene los tres dieces, el padre termina su turno sin matarte.
        yield return StartCoroutine(padreLista.Desaparecer());
        padreLista.gameObject.SetActive(false);
   

        yield return StartCoroutine(SecuenciaDia4());
    }
    public IEnumerator SecuenciaDia4()
    {
        diaActual = 4;
        reaccionZonaStalkerActivada = false;
        yaPenalizadoHoy = false;
        yield return StartCoroutine(MostrarCartelDia(4));
        audioSource2.Play();

        // GORDETE
        alumnoActual = gordete;
        gordete.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(gordete.Aparecer());

        yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Buenos días, profe. ¿Has desayunado bien?" }, true)));
      
        yield return StartCoroutine(gordete.EntregarExamen(4));
        nota = gordete.notaRecibida;

        if (nota == 4 || nota == 3)
        {
            gordete.animator.SetTrigger("Triste");
            if (Triste_gordete != null) audioSource.PlayOneShot(Triste_gordete, 1.3f);
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Jo, profe, me acabo de acordar, era Cristian Culón." }, true)));
        }
        else if (nota < 3)
        {
            gordete.animator.SetTrigger("Triste");
            if (Triste_gordete != null) audioSource.PlayOneShot(Triste_gordete, 1.3f);
            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "Esta nota no es nada kawaii." }, true)));
        }
        else
        {
            gordete.animator.SetTrigger("Feliz");
            if (Faliz_gordete != null) audioSource.PlayOneShot(Faliz_gordete, 1.3f);

            yield return StartCoroutine(ManejarDialogo(gordete.Decir(new string[] { "¡Sugooooi, profe!" }, true)));
        }

        yield return StartCoroutine(gordete.Desaparecer());
        gordete.gameObject.SetActive(false);

        // STALKER
        alumnoActual = stalker;
        stalker.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(stalker.Aparecer());

        yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Hola, profe. Te va a encantar mi examen." }, true)));

        yield return StartCoroutine(stalker.EntregarExamen(4));
        nota = stalker.notaRecibida;

        if (nota < 3)
        {
            stalker.animator.SetTrigger("Triste");
            if (Triste_stalker != null) audioSource.PlayOneShot(Triste_stalker, 1.3f);

            yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Quería saber de qué color era tu hígado." }, true)));
        }
        else if (nota == 4 || nota == 3)
        {
            stalker.animator.SetTrigger("Triste");
            if (Triste_stalker != null) audioSource.PlayOneShot(Triste_stalker, 1.3f);

            yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Adiós, primer amor." }, true)));
        }
        else
        {
            stalker.animator.SetTrigger("Feliz");
            if (Risa_stalker != null) audioSource.PlayOneShot(Risa_stalker, 1.3f);

            yield return StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "¡¿SIGNIFICA ESTO QUE ES RECÍPROCO?!" }, true)));
        }

        yield return StartCoroutine(stalker.Desaparecer());
        stalker.gameObject.SetActive(false);

        // MEWING
        alumnoActual = mewing;
        mewing.gameObject.SetActive(true);
        mewing.animator.SetInteger("Dia", diaMewing);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(mewing.Aparecer());

        if (diaMewing == 4)
        {
            if (Final_mewing != null) audioSource.PlayOneShot(Final_mewing, 1.3f);

            yield return EjecutarMuerte(mewing);
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Buenos días, Mark. Otra vez por aquí." }, true)));
            
        }

        yield return StartCoroutine(mewing.EntregarExamen(4));
        nota = mewing.notaRecibida;

        if (nota >= 5)
        {
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Gracias por inflarme la notilla.", "Te prometo que la próxima vez estudiaré más." }, true)));
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(mewing.Decir(new string[] { "Quizá sea un buen momento para estudiar más en serio.", "No voy a tener tiempo de jugar con Sergio..." }, true)));
        }

        yield return StartCoroutine(mewing.Desaparecer());
        mewing.gameObject.SetActive(false);

        // Lista 
        alumnoActual = repelente;
        repelente.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(repelente.Aparecer());

        yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "Buenos días, señor Mark." }, true)));

        yield return StartCoroutine(repelente.EntregarExamen(4));
        nota = repelente.notaRecibida;
  
        if (nota == 10)
        {
            repelente.animator.SetTrigger("Feliz");
            if (Risa_Lista != null) audioSource.PlayOneShot(Risa_Lista, 1.3f);

            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "Qué alegría me das. ¡Viva la meritocracia!" }, true)));
        }
        else if (nota < 10 || nota > 4)
        {

            repelente.animator.SetTrigger("Enfadado");
            if (Enfado_Lista != null) audioSource.PlayOneShot(Enfado_Lista, 1.3f);

            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "¿Y ESTA PUTA MIERDA DE NOTA?", "NO ME MEREZCO ESTA FALTA DE RESPETO." }, true)));

        }
        else
        {
            repelente.animator.SetTrigger("Enfadado");
            if (Enfado_Lista != null) audioSource.PlayOneShot(Enfado_Lista, 1.3f);
            yield return StartCoroutine(ManejarDialogo(repelente.Decir(new string[] { "HIJO DE PUTA, TE VOY A MATAR." }, true)));
        }

        yield return StartCoroutine(repelente.Desaparecer());
        repelente.gameObject.SetActive(false);

        // BASICOTE
        alumnoActual = basicote;
        basicote.gameObject.SetActive(true);

        if (entrar_niño != null) { audioSource.PlayOneShot(entrar_niño, 5f); }

        yield return StartCoroutine(basicote.Aparecer());
        yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Hola, profe, tengo una duda.", "¿Te gustan mis súper preguntas?" }, true)));

        yield return StartCoroutine(ManejarPregunta(basicote.Preguntar(opcionesSiNo)));

        if (basicote.respuestaElegida == 0)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "¡Genialístico! Te preparo una para el próximo día." }, true)));
        }
        else if (basicote.respuestaElegida == 1)
        {
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "¡ME CAGO EN TODO!", "¿Recuerdas lo que me respondiste anteayer?", "Disfruta :)." }, true)));
            if (decisionBasicoteDia2 == 0)
            {
                if (Final_basicote_camion != null) audioSource.PlayOneShot(Final_basicote_camion, 5f);
            }
            else
            {
                if (Final_basicote_bomba != null) audioSource.PlayOneShot(Final_basicote_bomba, 5f);
            }

                yield return StartCoroutine(EjecutarMuerte(basicote));
        }
        yield return StartCoroutine(basicote.EntregarExamen(4));
        nota = basicote.notaRecibida;
        if (nota < 3)
        {
            basicote.animator.SetTrigger("Triste");
            if (Triste_basicote != null) audioSource.PlayOneShot(Triste_basicote, 1.3f);
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Vaya, ¿esta mierda me merezco? Sí que soy estúpido." }, true)));
        }
        else if (nota == 4 || nota == 3)
        {
            basicote.animator.SetTrigger("Enfadado");
            if (Enfado_basicote != null) audioSource.PlayOneShot(Enfado_basicote, 1.3f);
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "¿Esta nota es una calentada tuya o de verdad merezco semejante mierda?" }, true)));
        }
        else
        {
            basicote.animator.SetTrigger("Feliz");
            if (Risa_basicote != null) audioSource.PlayOneShot(Risa_basicote, 1.3f);
            yield return StartCoroutine(ManejarDialogo(basicote.Decir(new string[] { "Ya sabía yo que soy un culto." }, true)));
        }

        yield return StartCoroutine(basicote.Desaparecer());
        basicote.gameObject.SetActive(false);

        // BRAINROT
        alumnoActual = brainrot;
        brainrot.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(brainrot.Aparecer());

        int faltas = PlayerPrefs.GetInt("FaltasTungTung", 0);

        if (faltas == 1)
        {
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(dialogoDia3Brainrot_molesto, false)));
        }
        else if (faltas == 2)
        {
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(dialogoDia3Brainrot_enfadado, false)));
        }
        else
        {
            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] {"..."}, false)));
        }

        yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Skibidi toilet, profe." }, false)));
        yield return StartCoroutine(brainrot.EntregarExamen(4));
        nota = brainrot.notaRecibida;

        if (nota < 4)
        {
            brainrot.animator.SetTrigger("Triste");
            if (Triste_brainrot != null) audioSource.PlayOneShot(Triste_brainrot, 1.3f);

            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Joder, voy a tener que hacer lock in con esta asignatura." }, false)));
        }
        else if (nota == 4 || nota == 3)
        {
            brainrot.animator.SetTrigger("Triste");
            if (Triste_brainrot != null) audioSource.PlayOneShot(Triste_brainrot, 1.3f);

            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Das mucho cringe, tío." }, false)));
        }
        else
        {
            brainrot.animator.SetTrigger("Feliz");
            if (Risa_brainrot != null) audioSource.PlayOneShot(Risa_brainrot, 1.3f);

            yield return StartCoroutine(ManejarDialogo(brainrot.Decir(new string[] { "Soy el goat. Es lo que hay." }, false)));
        }

        yield return StartCoroutine(brainrot.Desaparecer());
        brainrot.gameObject.SetActive(false);

        // NEGRETE
        alumnoActual = negrete;
        negrete.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(negrete.Aparecer());

        yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Epa profe, hoy me juego el curso. Sé bueno." }, true)));

        yield return StartCoroutine(negrete.EntregarExamen(4));
        nota = negrete.notaRecibida;

        if (nota < 5)
        {
            negrete.animator.SetTrigger("Triste");
            if (Triste_negrete != null) audioSource.PlayOneShot(Triste_negrete, 1.3f);

            yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Joder, profe, con esta nota repito curso."," ¿Jugamos a un minijuego rápido? Si tú ganas, suspendo y repito, pero si yo gano, me apruebas, ¿Te parece?","No te preocupes, que es muy fácil."," Yo voy a decir muchas palabras, y cuando oigas la palabra “fuego” aprieta el botón rápido."," ¿Lo has pillado? Empezamos." }, true)));
            negrete.animator.SetTrigger("no triste");

            panelJuegoNegreteCerrado = false; 
            if (panelJuegoNegrete != null) panelJuegoNegrete.SetActive(true);

            yield return new WaitUntil(() => panelJuegoNegreteCerrado);
            audioSource2.mute = true;
            if (juego_negrete != null) audioSource.PlayOneShot(juego_negrete, 1.3f);

            
            yield return new WaitForSeconds(29f);

            
            if (Fuego != null) audioSource.PlayOneShot(Fuego, 1.3f);
            yield return new WaitForSeconds(0.8f);
            botonFuegoPulsado = false;
            if (panelFuegoNegrete != null) panelFuegoNegrete.SetActive(true);
            float contadorFuego = 0f;
            while (contadorFuego < 2f && !botonFuegoPulsado)
            {
                contadorFuego += Time.deltaTime; 
                yield return null; 
            }

            if (panelFuegoNegrete != null) panelFuegoNegrete.SetActive(false);

            if (botonFuegoPulsado)
            {
                audioSource2.mute=false;
                negrete.animator.SetTrigger("Feliz");
                yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "Has ganado." }, true)));
            }
            else
            {
                if (Final_negrete != null) audioSource.PlayOneShot(Final_negrete, 1.3f);
                yield return StartCoroutine(EjecutarMuerte(negrete));
                yield break; 
            }
        }
        else
        {
            negrete.animator.SetTrigger("Feliz");
            if (Risa_negrete != null) audioSource.PlayOneShot(Risa_negrete, 1.3f);

            yield return StartCoroutine(ManejarDialogo(negrete.Decir(new string[] { "TOMA YAAA PASO DE CURSO" }, true)));
        }

        yield return StartCoroutine(negrete.Desaparecer());
        negrete.gameObject.SetActive(false);

        // CARTERO
        alumnoActual = cartero;
        cartero.gameObject.SetActive(true);
        if (entrar_niño != null) audioSource.PlayOneShot(entrar_niño, 5f);
        yield return StartCoroutine(cartero.Aparecer());

        yield return StartCoroutine(ManejarDialogo(cartero.Decir(new string[] { "Tronco, esta carta es para ti." }, true)));
        yield return new WaitUntil(() => cartaLeida);

        yield return StartCoroutine(ManejarDialogo(cartero.Decir(new string[] { "¡Enhorabuena, tío!", "Nos vemos por aquí." }, false)));

        yield return StartCoroutine(cartero.Desaparecer());
        cartero.gameObject.SetActive(false);

        // --- PANTALLA DE VICTORIA ---
        if (panelPantallaVictoria != null)
        {
            if (Victoria != null) audioSource.PlayOneShot(Victoria, 1f);
            panelPantallaVictoria.SetActive(true);
        }

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
        if (numeroDia > 1 && panelEntreDias != null)
        {
            yield return new WaitForSeconds(2.5f);
            audioSource2.Stop();
            panelEntreDias.SetActive(true);
            if (dias != null) audioSource.PlayOneShot(dias, 10f);

            CanvasGroup canvasGroup = panelEntreDias.GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = panelEntreDias.AddComponent<CanvasGroup>();

            float duracionFade = 0.5f;
            float tiempo = 0f;

            canvasGroup.alpha = 0f;
            while (tiempo < duracionFade)
            {
                tiempo += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, tiempo / duracionFade);
                yield return null; 
            }
            canvasGroup.alpha = 1f;

            yield return new WaitForSeconds(4.5f);

            tiempo = 0f;
            while (tiempo < duracionFade)
            {
                tiempo += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
                yield return null;
            }
            canvasGroup.alpha = 0f; 

            panelEntreDias.SetActive(false);
        }

        if (objetoCartelDias != null)
        {
            objetoCartelDias.SetActive(true);
            yield return null;
            objetoCartelDias.GetComponent<Animator>().SetInteger("Dia", numeroDia);
            yield return new WaitForSeconds(duracionCartel);
            objetoCartelDias.SetActive(false);
        }
    }
    // SISTEMA MAESTRO DE MUERTES

    public IEnumerator EjecutarMuerte(ScriptAlumno asesino)
    {
        ForzarSoltarHerramientas();
        GameObject panelAsesino = null;

        // 1. Limpiamos el texto por si venimos de otra muerte anterior
        if (textoGameOver != null) textoGameOver.text = "";

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
            audioSource2.mute = true;
            if (Final_Pantalla != null) audioSource.PlayOneShot(Final_Pantalla, 0.8f);
            panelAsesino.SetActive(false);
        }

        ActivarPantallaMuerte();

        string fraseMuerte = "..."; // Frase por defecto

        if (asesino == gordete) fraseMuerte = "Compartir es vivir.";
        else if (asesino == stalker) fraseMuerte = "Mira dónde firmas";
        else if (asesino == basicote) fraseMuerte = "No le quites la ilusión a un niño.";
        else if (asesino == brainrot) fraseMuerte = "El que avisa no es traidor.";
        else if (asesino == padreLista) fraseMuerte = "¿Corregiste justamente?.";
        else if (asesino == negrete) fraseMuerte = "Tus reflejos no fueron los más rápidos.";
        else if (asesino == mewing) fraseMuerte = "Igual deberías haber sido más amable.";

        StartCoroutine(EscribirTextoGameOver(fraseMuerte));
    }

    private IEnumerator SecuenciaMuerteTungTung()
    {
        yield return StartCoroutine(ManejarDialogo(alumnoActual.Decir(new string[] { "TÚ TE LO HAS BUSCADO.", "HAS INTENTADO DESTROZAR A TUNG-TUNG TRES VECES.", "AHORA SUFRIRÁS SU IRA." }, false)));
        if (Final_brainrot != null) audioSource.PlayOneShot(Final_brainrot, 1.3f);
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

    public void ActivarReaccionZonaStalker()
    {
        if (diaActual != 2) return;
        if (alumnoActual != stalker) return;

        if (stalker == null || !stalker.gameObject.activeInHierarchy)
            return;

        if (reaccionZonaStalkerActivada) return;
        if (dialogoEnCurso || esperandoRespuesta) return;

        reaccionZonaStalkerActivada = true; 

        StartCoroutine(ManejarDialogo(stalker.Decir(new string[] { "Acabas de venderme tu alma.", "Por fin eres mío." }, true)));

        if (Final_stalker != null) audioSource.PlayOneShot(Final_stalker, 1.3f);

        StartCoroutine(EjecutarMuerte(stalker));
    }
    public void ActivarPantallaMuerte()
    {
        if (panelPantallaMuerte != null)
        {
            panelPantallaMuerte.SetActive(true);
        }
    }

    // BOTONES DE LA PANTALLA DE MUERTE
    public void ReiniciarJuego()
    {
        audioSource2.mute = false;

        StartCoroutine(RutinaReiniciar());
    }

    private IEnumerator RutinaReiniciar()
    {

        if (panelPantallaMuerte != null)
        {
            Animator anim = panelPantallaMuerte.GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("transicion");
        }

        yield return new WaitForSecondsRealtime(0.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
        audioSource2.mute = false;

        StartCoroutine(RutinaVolverAlMenu());
    }

    private IEnumerator RutinaVolverAlMenu()
    {
        if (panelPantallaMuerte != null)
        {
            Animator anim = panelPantallaMuerte.GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("transicion");
        }

        yield return new WaitForSecondsRealtime(0.5f);

        SceneManager.LoadScene("Menu");
    }

    public void CerrarCartaYGanar()
    {
        if (panelCartaAmpliada != null)
        {
            panelCartaAmpliada.SetActive(false);
        }

        cartaLeida = true;
    }
  
    public void AbrirPanelEspecifico()
    {
        if (miPanelEspecifico != null)
        {
            miPanelEspecifico.SetActive(true);
        }
    }

    public void BotonMenuPanelEspecifico()
    {
        StartCoroutine(RutinaMenuPanelEspecifico());
    }

    private IEnumerator RutinaMenuPanelEspecifico()
    {

        if (miPanelEspecifico != null)
        {
            Animator anim = miPanelEspecifico.GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("salir");
        }

        yield return new WaitForSecondsRealtime(0.5f);

        SceneManager.LoadScene("Menu");
    }

    public void BotonCerrarJuegoPanelEspecifico()
    {
        StartCoroutine(RutinaCerrarJuegoPanelEspecifico());
    }

    private IEnumerator RutinaCerrarJuegoPanelEspecifico()
    {

        if (miPanelEspecifico != null)
        {
            Animator anim = miPanelEspecifico.GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("salir");
        }

        yield return new WaitForSecondsRealtime(0.5f);
        Application.Quit();

    }
    // BOTONES DEL EVENTO NEGRETE (DÍA 4)
    public void CerrarPanelJuegoNegrete()
    {
        if (panelJuegoNegrete != null)
        {
            panelJuegoNegrete.SetActive(false);
        }
        panelJuegoNegreteCerrado = true;
    }

    public void PulsarBotonFuego()
    {
        botonFuegoPulsado = true;
    }
    // BOTONES DE TUTORIAL (DÍA 1)
    public void CerrarPanelTutorialBoli()
    {
        if (panelTutorialBoli != null)
        {
            panelTutorialBoli.SetActive(false);
        }
        panelBoliCerrado = true;
    }

    public void CerrarPanelTutorialSellos()
    {
        if (panelTutorialSellos != null)
        {
            panelTutorialSellos.SetActive(false);
        }
        panelSellosCerrado = true;
    }

    private IEnumerator EscribirTextoGameOver(string frase)
    {
        if (textoGameOver == null) yield break; 

        textoGameOver.text = ""; 

        foreach (char letra in frase.ToCharArray())
        {
            textoGameOver.text += letra;
            yield return new WaitForSecondsRealtime(velocidadEscritura);
        }
    }
}