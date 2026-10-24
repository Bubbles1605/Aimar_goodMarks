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

    [Header("Zoom Examen")]
    public GameObject panelFondoOscuro;
    public GameObject botonCerrarZoom;
    public float factorEscalaZoom = 1.8f;

    private bool examenAmpliado = false;
    private Vector3 posicionOriginalExamen;
    private Vector3 escalaOriginalExamen;
    private int sortingOrderOriginal;
    private GameObject examenEnZoom;

    //BOLI RELATED
    private bool usandoBoli = false;
    private GameObject boliFisicoEnMesa;
    private Vector3 posicionOriginalBoli;
    private Quaternion rotacionOriginalBoli;

    //SELLO RELATED
    private bool usandoSello = false;
    private GameObject selloFisicoEnMesa;
    private Vector3 posicionOriginalSello;
    private int indiceSelloActual = -1;

    //ACTO DE DIBUJAR
    private LineRenderer lineaActual;
    private Vector2 puntoAnterior;
    private bool estaDibujando = false;

    void Update()
    {
        Vector2 posRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (usandoSello && selloFisicoEnMesa != null)
        {
            selloFisicoEnMesa.transform.position = new Vector3(posRaton.x, posRaton.y, selloFisicoEnMesa.transform.position.z);
        }

        if (usandoBoli && boliFisicoEnMesa != null)
        {
            boliFisicoEnMesa.transform.position = new Vector3(posRaton.x + offsetPuntaBoli.x, posRaton.y + offsetPuntaBoli.y, boliFisicoEnMesa.transform.position.z);
        }

        GameObject examenTocado = ObtenerObjetoTocado(posRaton, "Examen");

        if (Input.GetMouseButtonDown(0))
        {
            GameObject selloClicado = ObtenerObjetoTocado(posRaton, "Sello");
            GameObject boliClicado = ObtenerObjetoTocado(posRaton, "Boli");

            if (selloClicado != null && !usandoSello && !usandoBoli && !examenAmpliado)
            {
                usandoSello = true;
                selloFisicoEnMesa = selloClicado;
                posicionOriginalSello = selloFisicoEnMesa.transform.position;
                indiceSelloActual = ObtenerIndiceSello(selloClicado);
            }
            else if (boliClicado != null && !usandoBoli && !usandoSello && !examenAmpliado)
            {
                usandoBoli = true;
                boliFisicoEnMesa = boliClicado;
                posicionOriginalBoli = boliFisicoEnMesa.transform.position;
                rotacionOriginalBoli = boliFisicoEnMesa.transform.rotation;

                boliFisicoEnMesa.transform.rotation = Quaternion.identity;
                Cursor.visible = false;
            }
            else if (examenTocado != null)
            {
                ScriptDias director = FindFirstObjectByType<ScriptDias>();
                ScriptAlumno alumnoActual = (director != null) ? director.alumnoActual : null;

                if (usandoSello)
                {
                    if (alumnoActual != null && alumnoActual.examenCalificado)
                    {
                        Debug.Log("¡Este examen ya tiene una nota puesta!");
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
                    }
                    SoltarSello();
                }
                else if (usandoBoli)
                {
                    EmpezarPunto(posRaton, examenTocado.transform);
                    estaDibujando = true;
                }
                else if (!usandoSello && !usandoBoli && !examenAmpliado)
                {
                    AmpliarExamen(examenTocado);
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (examenTocado != null && usandoBoli)
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
            else
            {
                CortarTrazo();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            CortarTrazo();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (usandoSello) SoltarSello();
            if (usandoBoli) SoltarBoli();
        }
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
        if (!examenAmpliado || examenEnZoom == null) return;

        examenEnZoom.transform.position = posicionOriginalExamen;
        examenEnZoom.transform.localScale = escalaOriginalExamen;

        SpriteRenderer sr = examenEnZoom.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = sortingOrderOriginal;

        if (panelFondoOscuro != null) panelFondoOscuro.SetActive(false);
        if (botonCerrarZoom != null) botonCerrarZoom.SetActive(false);

        examenAmpliado = false;
        examenEnZoom = null;
    }

    void SoltarSello()
    {
        if (selloFisicoEnMesa != null)
        {
            selloFisicoEnMesa.transform.position = posicionOriginalSello;
        }
        usandoSello = false;
        selloFisicoEnMesa = null;
        indiceSelloActual = -1;
    }

    void SoltarBoli()
    {
        if (boliFisicoEnMesa != null)
        {
            boliFisicoEnMesa.transform.position = posicionOriginalBoli;
            boliFisicoEnMesa.transform.rotation = rotacionOriginalBoli;
        }

        Cursor.visible = true;
        usandoBoli = false;
        boliFisicoEnMesa = null;
        CortarTrazo();
    }

    GameObject ObtenerObjetoTocado(Vector2 posicion, string tagBuscado)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(posicion);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag(tagBuscado)) return hit.gameObject;
        }
        return null;
    }

    int ObtenerIndiceSello(GameObject sello)
    {
        string nombre = sello.name;
        for (int i = 10; i >= 0; i--)
        {
            if (nombre.Contains(i.ToString()))
            {
                return i;
            }
        }
        return 0;
    }

    void EmpezarPunto(Vector2 pos, Transform padre)
    {
        GameObject nuevoPunto = Instantiate(puntoPrefab, pos, Quaternion.identity);
        if (padre != null) nuevoPunto.transform.SetParent(padre, true);

        // Ponemos el punto en capa 12 para que viaje por encima del folio
        Renderer renderPunto = nuevoPunto.GetComponent<Renderer>();
        if (renderPunto != null) renderPunto.sortingOrder = 12;

        // Si el punto resulta ser también un LineRenderer, le aplicamos el arreglo
        LineRenderer lrPunto = nuevoPunto.GetComponent<LineRenderer>();
        if (lrPunto != null)
        {
            lrPunto.useWorldSpace = false;
            Vector3 posLocal = nuevoPunto.transform.InverseTransformPoint(pos);
            for (int i = 0; i < lrPunto.positionCount; i++)
            {
                lrPunto.SetPosition(i, posLocal);
            }
        }

        puntoAnterior = pos;
        lineaActual = null;
    }

    void ContinuarTrazo(Vector2 pos, Transform padre)
    {
        if (Vector2.Distance(pos, puntoAnterior) > distanciaMinima)
        {
            if (lineaActual == null)
            {
                // Instanciamos exactamente en la posición del folio para anclarlo bien
                GameObject nuevoTrazo = Instantiate(trazoPrefab, padre.position, Quaternion.identity);
                if (padre != null) nuevoTrazo.transform.SetParent(padre, true);

                lineaActual = nuevoTrazo.GetComponent<LineRenderer>();

                // --- LA MAGIA ESTÁ AQUÍ ---
                lineaActual.useWorldSpace = false; // Desconectamos del mundo global, sigue al padre
                lineaActual.sortingOrder = 12; // Capa visual por encima del folio que vuela
                lineaActual.startColor = Color.red;
                lineaActual.endColor = Color.red;
                lineaActual.positionCount = 2;

                // Traducimos la posición absoluta del ratón a posición local (relativa al folio)
                Vector3 puntoAnteriorLocal = lineaActual.transform.InverseTransformPoint(puntoAnterior);
                Vector3 posLocal = lineaActual.transform.InverseTransformPoint(pos);

                lineaActual.SetPosition(0, puntoAnteriorLocal);
                lineaActual.SetPosition(1, posLocal);
            }
            else
            {
                lineaActual.positionCount++;
                Vector3 posLocal = lineaActual.transform.InverseTransformPoint(pos);
                lineaActual.SetPosition(lineaActual.positionCount - 1, posLocal);
            }
            puntoAnterior = pos;
        }
    }

    void CortarTrazo()
    {
        lineaActual = null;
        estaDibujando = false;
    }
}