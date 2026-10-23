using UnityEngine;

public class ControladorRotulador : MonoBehaviour
{
    [Header("Boli")]
    public GameObject trazoPrefab;
    public GameObject puntoPrefab;
    public float distanciaMinima = 0.02f;
    public Vector2 offsetPuntaBoli = Vector2.zero;

    [Header("Sello")]
    public GameObject marcaSelloPrefab;     // Prefab base de la nota en el examen
    public Sprite[] spritesNotas;           // Las 11 imágenes de las notas (del 0 al 10)

    [Header("Configuracion Cursor Sello")]
    public GameObject cursorSelloVisual;    // Objeto flotante que sigue al ratón al coger un sello
    public Sprite imagenCursorSello;        // Imagen aparte para el cursor del sello
    public Vector3 escalaNotaEstampada = new Vector3(1.5f, 1.5f, 1.5f); // Tamaño grande de la nota en el examen

    //BOLI RELATED
    private bool usandoBoli = false;
    private GameObject boliFisicoEnMesa;
    private Vector3 posicionOriginalBoli;
    private Quaternion rotacionOriginalBoli;

    //SELLO RELATED
    private bool usandoSello = false;
    private GameObject selloFisicoEnMesa;
    private Vector3 posicionOriginalSello;
    private int indiceSelloActual = -1;     // Guarda qué número de nota (0 al 10) se ha cogido

    //ACTO DE DIBUJAR
    private LineRenderer lineaActual;
    private Vector2 puntoAnterior;
    private bool estaDibujando = false;

    void Start()
    {
        if (cursorSelloVisual != null)
        {
            cursorSelloVisual.SetActive(false);
        }
    }

    void Update()
    {
        Vector2 posRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // El cursor visual del sello sigue al ratón
        if (usandoSello && cursorSelloVisual != null)
        {
            cursorSelloVisual.transform.position = new Vector3(posRaton.x, posRaton.y, 0f);
        }

        if (usandoBoli && boliFisicoEnMesa != null)
        {
            boliFisicoEnMesa.transform.position = new Vector3(posRaton.x + offsetPuntaBoli.x, posRaton.y + offsetPuntaBoli.y, boliFisicoEnMesa.transform.position.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject selloClicado = ObtenerObjetoTocado(posRaton, "Sello");
            GameObject boliClicado = ObtenerObjetoTocado(posRaton, "Boli");

            if (selloClicado != null && !usandoSello && !usandoBoli)
            {
                usandoSello = true;
                selloFisicoEnMesa = selloClicado;
                posicionOriginalSello = selloFisicoEnMesa.transform.position;

                // Detectamos qué número de sello es (del 0 al 10)
                indiceSelloActual = ObtenerIndiceSello(selloClicado);

                // Activamos el cursor flotante exclusivo del sello con su imagen aparte
                if (cursorSelloVisual != null)
                {
                    cursorSelloVisual.SetActive(true);
                    SpriteRenderer srCursor = cursorSelloVisual.GetComponent<SpriteRenderer>();
                    if (srCursor != null && imagenCursorSello != null)
                    {
                        srCursor.sprite = imagenCursorSello;
                    }
                }
            }
            else if (boliClicado != null && !usandoBoli && !usandoSello)
            {
                usandoBoli = true;
                boliFisicoEnMesa = boliClicado;
                posicionOriginalBoli = boliFisicoEnMesa.transform.position;
                rotacionOriginalBoli = boliFisicoEnMesa.transform.rotation;

                boliFisicoEnMesa.transform.rotation = Quaternion.identity;
                Cursor.visible = false;
            }
            else if (EstaSobreExamen(posRaton))
            {
                if (usandoSello)
                {
                    GameObject examenObjeto = ObtenerObjetoExamen(posRaton);

                    if (marcaSelloPrefab != null)
                    {
                        Transform padreExamen = (examenObjeto != null) ? examenObjeto.transform : null;

                        // Instanciamos la nota vinculada al examen
                        GameObject nuevaNota = Instantiate(marcaSelloPrefab, posRaton, Quaternion.identity, padreExamen);

                        // Aplicamos el tamaño grande
                        nuevaNota.transform.localScale = escalaNotaEstampada;

                        // Asignamos la imagen correspondiente de la lista (0 al 10) y forzamos el color blanco
                        SpriteRenderer sr = nuevaNota.GetComponent<SpriteRenderer>();
                        if (sr != null && spritesNotas != null && indiceSelloActual >= 0 && indiceSelloActual < spritesNotas.Length)
                        {
                            sr.sprite = spritesNotas[indiceSelloActual];
                            sr.color = Color.white;
                        }
                    }

                    SoltarSello();
                }
                else if (usandoBoli)
                {
                    EmpezarPunto(posRaton);
                    estaDibujando = true;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (EstaSobreExamen(posRaton) && usandoBoli)
            {
                if (!estaDibujando)
                {
                    EmpezarPunto(posRaton);
                    estaDibujando = true;
                }
                else
                {
                    ContinuarTrazo(posRaton);
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

    void SoltarSello()
    {
        usandoSello = false;
        indiceSelloActual = -1;
        if (cursorSelloVisual != null)
        {
            cursorSelloVisual.SetActive(false);
        }
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

    bool EstaSobreExamen(Vector2 posicion)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(posicion);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Examen")) return true;
        }
        return false;
    }

    GameObject ObtenerObjetoExamen(Vector2 posicion)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(posicion);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Examen")) return hit.gameObject;
        }
        return null;
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

    // Lee de 10 a 0 para que el "10" tenga prioridad sobre el "0" y el "1"
    int ObtenerIndiceSello(GameObject sello)
    {
        string nombre = sello.name;
        for (int i = 10; i >= 0; i--)
        {
            if (nombre.Contains(i.ToString()))
            {
                return i; // Retorna directamente el número como índice (0 al 10)
            }
        }
        return 0;
    }

    void EmpezarPunto(Vector2 pos)
    {
        Instantiate(puntoPrefab, pos, Quaternion.identity);
        puntoAnterior = pos;
        lineaActual = null;
    }

    void ContinuarTrazo(Vector2 pos)
    {
        if (Vector2.Distance(pos, puntoAnterior) > distanciaMinima)
        {
            if (lineaActual == null)
            {
                GameObject nuevoTrazo = Instantiate(trazoPrefab, Vector3.zero, Quaternion.identity);
                lineaActual = nuevoTrazo.GetComponent<LineRenderer>();
                lineaActual.startColor = Color.red;
                lineaActual.endColor = Color.red;
                lineaActual.positionCount = 2;
                lineaActual.SetPosition(0, puntoAnterior);
                lineaActual.SetPosition(1, pos);
            }
            else
            {
                lineaActual.positionCount++;
                lineaActual.SetPosition(lineaActual.positionCount - 1, pos);
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