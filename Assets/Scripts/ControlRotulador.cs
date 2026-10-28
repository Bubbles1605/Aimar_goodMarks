using UnityEngine;

public class ControladorRotulador : MonoBehaviour
{
    [Header("Boli")]
    public GameObject trazoPrefab;
    public GameObject puntoPrefab;
    public float distanciaMinima = 0.02f;
    public Vector2 offsetPuntaBoli = Vector2.zero;

    [Header("Sello y Examen")]
    public GameObject marcaSelloPrefab;
    public Sprite[] spritesNotas;
    public Vector3 escalaNotaEstampada = new Vector3(1.5f, 1.5f, 1.5f);

    [Tooltip("Posición exacta en la pantalla/mundo donde aparecerá la nota automáticamente")]
    public Vector2 posicionNotaExamen;

    [Header("Pegatinas")]
    public GameObject pegatinaPrefab;

    [Header("Zoom Examen")]
    public GameObject panelFondoOscuro;
    public GameObject botonCerrarZoom;
    public float factorEscalaZoom = 1.8f;
    public float maxFactorEscalaZoom = 4.0f;
    public float velocidadScrollZoom = 1.5f;

    private bool examenAmpliado = false;
    private Vector3 posicionOriginalExamen;
    private Vector3 escalaOriginalExamen;
    private int sortingOrderOriginal;
    private GameObject examenEnZoom;

    private Vector3 posicionRatonAnterior_Arrastre;
    private bool estaArrastrando = false;

    // BOLI RELATED
    private bool usandoBoli = false;
    private GameObject boliFisicoEnMesa;
    private Vector3 posicionOriginalBoli;
    private Quaternion rotacionOriginalBoli;

    // SELLO RELATED
    private bool usandoSello = false;
    private GameObject selloFisicoEnMesa;
    private Vector3 posicionOriginalSello;
    private int indiceSelloActual = -1;

    // PEGATINA RELATED
    private bool usandoPegatina = false;
    private GameObject pegatinaEnMano;

    // ACTO DE DIBUJAR
    private LineRenderer lineaActual;
    private Vector2 puntoAnterior;
    private bool estaDibujando = false;

    [Header("Sonidos")]
    public AudioClip sonidoCogerBoli;
    public AudioClip sonidoPonerSello;
    public AudioClip sonidoPonerPegatina;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        Vector2 posRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ScriptDias director = FindFirstObjectByType<ScriptDias>();
        bool bloqueadoPorDialogo = (director != null && (director.dialogoEnCurso || director.esperandoRespuesta));

        // --- SISTEMA DE ARRASTRE DEL ZOOM ---
        if (examenAmpliado && examenEnZoom != null)
        {
            if (!bloqueadoPorDialogo)
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll != 0f)
                {
                    Vector3 nuevaEscala = examenEnZoom.transform.localScale + (Vector3.one * scroll * velocidadScrollZoom);
                    float limiteMinimo = escalaOriginalExamen.x * factorEscalaZoom;
                    float limiteMaximo = escalaOriginalExamen.x * maxFactorEscalaZoom;
                    nuevaEscala.x = Mathf.Clamp(nuevaEscala.x, limiteMinimo, limiteMaximo);
                    nuevaEscala.y = Mathf.Clamp(nuevaEscala.y, limiteMinimo, limiteMaximo);
                    nuevaEscala.z = Mathf.Clamp(nuevaEscala.z, limiteMinimo, limiteMaximo);
                    examenEnZoom.transform.localScale = nuevaEscala;
                }
            }

            if (!bloqueadoPorDialogo && Input.GetMouseButtonDown(1))
            {
                if (!usandoSello && !usandoBoli && !usandoPegatina)
                {
                    estaArrastrando = true;
                    posicionRatonAnterior_Arrastre = posRaton;
                }
            }

            if (Input.GetMouseButton(1) && estaArrastrando)
            {
                Vector3 diferenciaRaton = (Vector3)posRaton - posicionRatonAnterior_Arrastre;
                examenEnZoom.transform.position += diferenciaRaton;
                posicionRatonAnterior_Arrastre = posRaton;
            }

            if (Input.GetMouseButtonUp(1))
            {
                estaArrastrando = false;
            }
        }

        // Actualizamos posiciones de las herramientas en mano SIEMPRE
        if (usandoSello && selloFisicoEnMesa != null)
        {
            selloFisicoEnMesa.transform.position = new Vector3(posRaton.x, posRaton.y, selloFisicoEnMesa.transform.position.z);
        }

        if (usandoBoli && boliFisicoEnMesa != null)
        {
            boliFisicoEnMesa.transform.position = new Vector3(posRaton.x + offsetPuntaBoli.x, posRaton.y + offsetPuntaBoli.y, boliFisicoEnMesa.transform.position.z);
        }

        if (usandoPegatina && pegatinaEnMano != null)
        {
            pegatinaEnMano.transform.position = new Vector3(posRaton.x, posRaton.y, 0f);
        }

        GameObject examenTocado = ObtenerObjetoTocado(posRaton, "Examen");

        // CLICS IZQUIERDOS
        if (!bloqueadoPorDialogo && Input.GetMouseButtonDown(0))
        {
            GameObject selloClicado = ObtenerObjetoTocado(posRaton, "Sello");
            GameObject boliClicado = ObtenerObjetoTocado(posRaton, "Boli");
            GameObject pegatinaClicada = ObtenerObjetoTocado(posRaton, "Pegatina");

            if (selloClicado != null && !usandoSello && !usandoBoli && !usandoPegatina && !examenAmpliado)
            {
                usandoSello = true;
                selloFisicoEnMesa = selloClicado;
                posicionOriginalSello = selloFisicoEnMesa.transform.position;
                indiceSelloActual = ObtenerIndiceSello(selloClicado);
            }
            else if (boliClicado != null && !usandoBoli && !usandoSello && !usandoPegatina && !examenAmpliado)
            {
                usandoBoli = true;
                boliFisicoEnMesa = boliClicado;
                posicionOriginalBoli = boliFisicoEnMesa.transform.position;
                rotacionOriginalBoli = boliFisicoEnMesa.transform.rotation;

                if (sonidoCogerBoli != null) audioSource.PlayOneShot(sonidoCogerBoli);

                boliFisicoEnMesa.transform.rotation = Quaternion.identity;
                Cursor.visible = false;
            }
            else if (pegatinaClicada != null && !usandoPegatina && !usandoBoli && !usandoSello && !examenAmpliado)
            {
                usandoPegatina = true;
                if (pegatinaPrefab != null)
                {
                    pegatinaEnMano = Instantiate(pegatinaPrefab, posRaton, Quaternion.identity);
                    SpriteRenderer sr = pegatinaEnMano.GetComponent<SpriteRenderer>();
                    if (sr != null) sr.sortingOrder = 35;
                }
            }
            else if (examenTocado != null)
            {
                ScriptAlumno alumnoActual = (director != null) ? director.alumnoActual : null;

                if (usandoSello)
                {
                    if (alumnoActual != null && alumnoActual.examenCalificado)
                    {
                        SoltarSello();
                        return;
                    }

                    if (marcaSelloPrefab != null)
                    {
                        Vector3 posicionFinal = new Vector3(posicionNotaExamen.x, posicionNotaExamen.y, 0f);
                        GameObject nuevaNota = Instantiate(marcaSelloPrefab, posicionFinal, Quaternion.identity);
                        nuevaNota.transform.localScale = escalaNotaEstampada;

                        if (alumnoActual != null && alumnoActual.examenInstanciado != null)
                        {
                            nuevaNota.transform.SetParent(alumnoActual.examenInstanciado.transform, true);
                        }

                        SpriteRenderer sr = nuevaNota.GetComponent<SpriteRenderer>();
                        if (sr != null && spritesNotas != null && indiceSelloActual >= 0 && indiceSelloActual < spritesNotas.Length)
                        {
                            sr.sprite = spritesNotas[indiceSelloActual];
                            sr.color = Color.white;
                            sr.sortingOrder = 12;
                        }

                        if (alumnoActual != null)
                        {
                            alumnoActual.notaRecibida = indiceSelloActual;
                            alumnoActual.examenCalificado = true;
                        }

                        if (sonidoPonerSello != null) audioSource.PlayOneShot(sonidoPonerSello);
                    }
                    SoltarSello();
                }
                else if (usandoPegatina)
                {
                    if (pegatinaEnMano != null)
                    {
                        if (alumnoActual != null && alumnoActual.examenInstanciado != null)
                        {
                            pegatinaEnMano.transform.SetParent(alumnoActual.examenInstanciado.transform, true);
                        }
                        else
                        {
                            pegatinaEnMano.transform.SetParent(examenTocado.transform, true);
                        }

                        SpriteRenderer sr = pegatinaEnMano.GetComponent<SpriteRenderer>();
                        if (sr != null) sr.sortingOrder = 15;

                        if (sonidoPonerPegatina != null) audioSource.PlayOneShot(sonidoPonerPegatina);

                        usandoPegatina = false;
                        pegatinaEnMano = null;
                    }
                }
                else if (usandoBoli)
                {
                    if (!ComprobarDibujoProhibido(posRaton))
                    {
                        EmpezarPunto(posRaton, examenTocado.transform);
                        estaDibujando = true;
                    }
                }
                else if (!usandoSello && !usandoBoli && !usandoPegatina && !examenAmpliado)
                {
                    if (alumnoActual != null)
                    {
                        if (!alumnoActual.examenCalificado && alumnoActual.examenEnMesa)
                        {
                            AmpliarExamen(examenTocado);
                        }
                    }
                }
            }
        }
        else if (!bloqueadoPorDialogo && Input.GetMouseButton(0))
        {
            if (examenTocado != null && usandoBoli)
            {
                if (!ComprobarDibujoProhibido(posRaton))
                {
                    if (!estaDibujando)
                    {
                        EmpezarPunto(posRaton, examenTocado.transform);
                        estaDibujando = true;
                    }
                    else
                    {
                        ContinuarTrazo(posRaton, examenTocado.transform);
                    }
                }
            }
            else
            {
                CortarTrazo();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            CortarTrazo();
        }

        // CLIC DERECHO
        if (Input.GetMouseButtonDown(1))
        {
            SoltarTodo();
        }


    }

    bool ComprobarDibujoProhibido(Vector2 pos)
    {
        GameObject dibujoTocado = ObtenerObjetoTocado(pos, "DibujoTungTung");
        if (dibujoTocado != null)
        {
            ScriptDias director = FindFirstObjectByType<ScriptDias>();
            if (director != null)
            {
                director.PenalizarDibujo();
            }
            CortarTrazo();
            return true;
        }
        return false;
    }

    void AmpliarExamen(GameObject examen)
    {
        examenAmpliado = true;
        examenEnZoom = examen;

        posicionOriginalExamen = examen.transform.position;
        escalaOriginalExamen = examen.transform.localScale;

        examen.transform.position = Vector3.zero;
        examen.transform.localScale = escalaOriginalExamen * factorEscalaZoom;

        SpriteRenderer sr = examen.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sortingOrderOriginal = sr.sortingOrder;
            sr.sortingOrder = 30;
        }

        if (panelFondoOscuro != null) panelFondoOscuro.SetActive(true);
        if (botonCerrarZoom != null) botonCerrarZoom.SetActive(true);
    }

    public void CerrarZoom()
    {
        if (!examenAmpliado) return;

        if (examenEnZoom != null)
        {
            examenEnZoom.transform.position = posicionOriginalExamen;
            examenEnZoom.transform.localScale = escalaOriginalExamen;

            SpriteRenderer sr = examenEnZoom.GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = sortingOrderOriginal;
        }

        if (panelFondoOscuro != null) panelFondoOscuro.SetActive(false);
        if (botonCerrarZoom != null) botonCerrarZoom.SetActive(false);

        examenAmpliado = false;
        examenEnZoom = null;
        estaArrastrando = false;
    }

    public void SoltarTodo()
    {
        if (usandoSello) SoltarSello();
        if (usandoBoli) SoltarBoli();
        if (usandoPegatina) SoltarPegatina();
    }

    void SoltarSello() { if (selloFisicoEnMesa != null) { selloFisicoEnMesa.transform.position = posicionOriginalSello; } usandoSello = false; selloFisicoEnMesa = null; indiceSelloActual = -1; }
    void SoltarPegatina() { if (pegatinaEnMano != null) { Destroy(pegatinaEnMano); } usandoPegatina = false; pegatinaEnMano = null; }
    public void SoltarBoli() { if (boliFisicoEnMesa != null) { boliFisicoEnMesa.transform.position = posicionOriginalBoli; boliFisicoEnMesa.transform.rotation = rotacionOriginalBoli; } Cursor.visible = true; usandoBoli = false; boliFisicoEnMesa = null; CortarTrazo(); }

    GameObject ObtenerObjetoTocado(Vector2 posicion, string tagBuscado) { Collider2D[] hits = Physics2D.OverlapPointAll(posicion); foreach (Collider2D hit in hits) { if (hit.CompareTag(tagBuscado)) return hit.gameObject; } return null; }
    int ObtenerIndiceSello(GameObject sello) { string nombre = sello.name; for (int i = 10; i >= 0; i--) { if (nombre.Contains(i.ToString())) return i; } return 0; }

    void EmpezarPunto(Vector2 pos, Transform padre) { GameObject nuevoPunto = Instantiate(puntoPrefab, pos, Quaternion.identity); if (padre != null) nuevoPunto.transform.SetParent(padre, true); Renderer renderPunto = nuevoPunto.GetComponent<Renderer>(); if (renderPunto != null) renderPunto.sortingOrder = 12; LineRenderer lrPunto = nuevoPunto.GetComponent<LineRenderer>(); if (lrPunto != null) { lrPunto.useWorldSpace = false; Vector3 posLocal = nuevoPunto.transform.InverseTransformPoint(pos); for (int i = 0; i < lrPunto.positionCount; i++) lrPunto.SetPosition(i, posLocal); } puntoAnterior = pos; lineaActual = null; }
    void ContinuarTrazo(Vector2 pos, Transform padre) { if (Vector2.Distance(pos, puntoAnterior) > distanciaMinima) { if (lineaActual == null) { GameObject nuevoTrazo = Instantiate(trazoPrefab, padre.position, Quaternion.identity); if (padre != null) nuevoTrazo.transform.SetParent(padre, true); lineaActual = nuevoTrazo.GetComponent<LineRenderer>(); lineaActual.useWorldSpace = false; lineaActual.sortingOrder = 12; lineaActual.startColor = Color.red; lineaActual.endColor = Color.red; lineaActual.positionCount = 2; Vector3 puntoAnteriorLocal = lineaActual.transform.InverseTransformPoint(puntoAnterior); Vector3 posLocal = lineaActual.transform.InverseTransformPoint(pos); lineaActual.SetPosition(0, puntoAnteriorLocal); lineaActual.SetPosition(1, posLocal); } else { lineaActual.positionCount++; Vector3 posLocal = lineaActual.transform.InverseTransformPoint(pos); lineaActual.SetPosition(lineaActual.positionCount - 1, posLocal); } puntoAnterior = pos; } }
    void CortarTrazo() { lineaActual = null; estaDibujando = false; }
}