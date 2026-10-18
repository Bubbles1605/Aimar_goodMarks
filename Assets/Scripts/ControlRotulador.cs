using UnityEngine;

public class ControladorRotulador : MonoBehaviour
{
    [Header("Boli")]
    public GameObject trazoPrefab;
    public GameObject puntoPrefab;
    public float distanciaMinima = 0.02f;
    public Vector2 offsetPuntaBoli = Vector2.zero;

    [Header("Sello")]
    public GameObject marcaSelloPrefab;

    //BOLI RELATED
    private bool usandoBoli = false;
    private GameObject boliFisicoEnMesa;
    private Vector3 posicionOriginalBoli;
    private Quaternion rotacionOriginalBoli;

    //SELLO RELATED
    private bool usandoSello = false;
    private GameObject selloFisicoEnMesa;
    private Vector3 posicionOriginalSello;

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

        if (Input.GetMouseButtonDown(0))
        {
            GameObject selloClicado = ObtenerObjetoTocado(posRaton, "Sello");
            GameObject boliClicado = ObtenerObjetoTocado(posRaton, "Boli");

            if (selloClicado != null && !usandoSello && !usandoBoli)
            {
                usandoSello = true;
                selloFisicoEnMesa = selloClicado;
                posicionOriginalSello = selloFisicoEnMesa.transform.position;
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
                    Instantiate(marcaSelloPrefab, posRaton, Quaternion.identity);
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
        selloFisicoEnMesa.transform.position = posicionOriginalSello;
        usandoSello = false;
        selloFisicoEnMesa = null;
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