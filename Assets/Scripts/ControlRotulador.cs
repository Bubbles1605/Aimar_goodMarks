using UnityEngine;

public class ControladorRotulador : MonoBehaviour
{
    [Header("Boli")]
    public GameObject trazoPrefab;
    public GameObject puntoPrefab;
    public float distanciaMinima = 0.02f;
    public Vector2 offsetPuntaBoli = Vector2.zero;

    [Header("Sello y Examen")]
    public GameObject marcaSelloPrefab;     // Prefab base de la nota que se imprime en el examen
    public Sprite[] spritesNotas;           // Las 11 imágenes de las notas (del 0 al 10)
    public Vector3 escalaNotaEstampada = new Vector3(1.5f, 1.5f, 1.5f); // Tamaño grande de la nota en el examen

    [Tooltip("Posición exacta en la pantalla/mundo donde aparecerá la nota automáticamente")]
    public Vector2 posicionNotaExamen;      // ¡Modifica esto cómodamente desde el Inspector de Unity!

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
    private bool examenYaEstampado = false;  // Controla si ya se ha colocado una nota en el examen

    //ACTO DE DIBUJAR
    private LineRenderer lineaActual;
    private Vector2 puntoAnterior;
    private bool estaDibujando = false;

    void Update()
    {
        Vector2 posRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Si estamos usando un sello, el sello físico de la mesa sigue al ratón
        if (usandoSello && selloFisicoEnMesa != null)
        {
            selloFisicoEnMesa.transform.position = new Vector3(posRaton.x, posRaton.y, selloFisicoEnMesa.transform.position.z);
        }

        // Si estamos usando el boli, el boli sigue al ratón con su offset
        if (usandoBoli && boliFisicoEnMesa != null)
        {
            boliFisicoEnMesa.transform.position = new Vector3(posRaton.x + offsetPuntaBoli.x, posRaton.y + offsetPuntaBoli.y, boliFisicoEnMesa.transform.position.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject selloClicado = ObtenerObjetoTocado(posRaton, "Sello");
            GameObject boliClicado = ObtenerObjetoTocado(posRaton, "Boli");

            // 1. Coger un sello de la mesa
            if (selloClicado != null && !usandoSello && !usandoBoli)
            {
                usandoSello = true;
                selloFisicoEnMesa = selloClicado;
                posicionOriginalSello = selloFisicoEnMesa.transform.position;

                indiceSelloActual = ObtenerIndiceSello(selloClicado);
            }
            // 2. Coger el boli de la mesa
            else if (boliClicado != null && !usandoBoli && !usandoSello)
            {
                usandoBoli = true;
                boliFisicoEnMesa = boliClicado;
                posicionOriginalBoli = boliFisicoEnMesa.transform.position;
                rotacionOriginalBoli = boliFisicoEnMesa.transform.rotation;

                boliFisicoEnMesa.transform.rotation = Quaternion.identity;
                Cursor.visible = false;
            }
            // 3. Hacer clic sobre el examen
            else if (EstaSobreExamen(posRaton))
            {
                if (usandoSello)
                {
                    // Comprobamos si ya se puso una nota antes en este examen
                    if (examenYaEstampado)
                    {
                        Debug.Log("¡Este examen ya tiene una nota puesta!");
                        SoltarSello();
                        return; // Sale de la función sin hacer nada más
                    }

                    if (marcaSelloPrefab != null)
                    {
                        // Usamos la posición fija configurada en el Inspector
                        Vector3 posicionFinal = new Vector3(posicionNotaExamen.x, posicionNotaExamen.y, 0f);

                        GameObject nuevaNota = Instantiate(marcaSelloPrefab, posicionFinal, Quaternion.identity);
                        nuevaNota.transform.localScale = escalaNotaEstampada;

                        SpriteRenderer sr = nuevaNota.GetComponent<SpriteRenderer>();
                        if (sr != null && spritesNotas != null && indiceSelloActual >= 0 && indiceSelloActual < spritesNotas.Length)
                        {
                            sr.sprite = spritesNotas[indiceSelloActual];
                            sr.color = Color.white;
                            sr.sortingOrder = 10;
                        }

                        // Marcamos que ya se ha puesto una nota para bloquear futuros sellos
                        examenYaEstampado = true;
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

        // Clic derecho para cancelar y devolver el sello o boli a la mesa
        if (Input.GetMouseButtonDown(1))
        {
            if (usandoSello) SoltarSello();
            if (usandoBoli) SoltarBoli();
        }
    }

    void SoltarSello()
    {
        if (selloFisicoEnMesa != null)
        {
            selloFisicoEnMesa.transform.position = posicionOriginalSello; // Devuelve el sello a la mesa
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

    bool EstaSobreExamen(Vector2 posicion)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(posicion);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Examen")) return true;
        }
        return false;
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