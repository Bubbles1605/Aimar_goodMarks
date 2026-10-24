using System.Collections;
using UnityEngine;
using TMPro;

public class ScriptAlumno : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidadAparicion = 5f;
    public float posicionYInicial = -8f;
    public float posicionYFinal = -0.65f;

    [Header("Diálogos de este Alumno")]
    public string[] dialogoPrincipal;
    public string[] frasesGenericas;

    [Header("Interfaz y Diálogo")]
    public GameObject botonHablarPrefab;
    public Vector3 posicionBoton = new Vector3(-1.96f, 1.82f, 0f);

    public TextMeshProUGUI textoUI;
    public float tiempoPorLetra = 0.05f;

    [Header("Examen")]
    public GameObject prefabExamen;
    public float posicionXExamen = 16.15f;
    public float posicionYInicialExamen = 8f;
    public float posicionYFinalExamen = 1.25f;
    public float velocidadCaidaExamen = 7f;
    [HideInInspector] public GameObject examenInstanciado;

    // LA VARIABLE MÁGICA
    [HideInInspector] public bool examenCalificado = false;

    private SpriteRenderer spriteRenderer;
    public Animator animator;
    private GameObject botonHablarInstanciado;

    [HideInInspector] public bool avanzarTexto = false;
    [HideInInspector] public int notaRecibida = -1;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public IEnumerator Aparecer()
    {
        ScriptDias director = FindFirstObjectByType<ScriptDias>();
        if (director != null)
        {
            director.alumnoActual = this;
        }

        if (spriteRenderer != null) spriteRenderer.sortingOrder = 2;
        transform.position = new Vector3(-4f, posicionYInicial, 0f);
        Vector3 posicionObjetivo = new Vector3(-4f, posicionYFinal, 0f);

        while (Vector3.Distance(transform.position, posicionObjetivo) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, velocidadAparicion * Time.deltaTime);
            yield return null;
        }

        transform.position = posicionObjetivo;
    }

    public IEnumerator Desaparecer()
    {
        Vector3 posicionObjetivo = new Vector3(-4f, posicionYInicial, 0f);

        while (Vector3.Distance(transform.position, posicionObjetivo) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, velocidadAparicion * Time.deltaTime);
            yield return null;
        }

        transform.position = posicionObjetivo;
    }

    public IEnumerator EntregarExamen()
    {
        examenCalificado = false; // Lo reseteamos por seguridad al empezar

        if (prefabExamen != null)
        {
            Vector3 posicionSalida = new Vector3(posicionXExamen, posicionYInicialExamen, 0f);
            Vector3 posicionMesa = new Vector3(posicionXExamen, posicionYFinalExamen, 0f);

            examenInstanciado = Instantiate(prefabExamen, posicionSalida, Quaternion.identity);
            examenInstanciado.SetActive(true);

            SpriteRenderer spriteExamen = examenInstanciado.GetComponentInChildren<SpriteRenderer>();
            if (spriteExamen != null)
            {
                spriteExamen.sortingOrder = 11;
            }

            while (Vector3.Distance(examenInstanciado.transform.position, posicionMesa) > 0.01f)
            {
                examenInstanciado.transform.position = Vector3.MoveTowards(examenInstanciado.transform.position, posicionMesa, velocidadCaidaExamen * Time.deltaTime);
                yield return null;
            }

            examenInstanciado.transform.position = posicionMesa;

            if (spriteExamen != null)
            {
                spriteExamen.sortingOrder = 1;
            }
        }
        else
        {
            Debug.LogWarning("Ojo: No has asignado el Prefab del Examen en el Inspector del alumno.");
        }

        if (botonHablarPrefab != null)
        {
            botonHablarInstanciado = Instantiate(botonHablarPrefab, posicionBoton, Quaternion.identity);
        }

        // --- MAGIA: La secuencia se pausa aquí hasta que el jugador ponga el sello ---
        yield return new WaitUntil(() => examenCalificado);
        yield return new WaitForSeconds(1f);

        // --- RETIRAR EL EXAMEN ---
        if (examenInstanciado != null)
        {
            Vector3 posicionDestino = new Vector3(posicionXExamen, posicionYInicialExamen, 0f);

            // Subimos la capa para que pase por encima del alumno al irse volando
            SpriteRenderer spriteExamen = examenInstanciado.GetComponentInChildren<SpriteRenderer>();
            if (spriteExamen != null) spriteExamen.sortingOrder = 11;

            // Bucle hacia arriba
            while (Vector3.Distance(examenInstanciado.transform.position, posicionDestino) > 0.01f)
            {
                examenInstanciado.transform.position = Vector3.MoveTowards(examenInstanciado.transform.position, posicionDestino, velocidadCaidaExamen * Time.deltaTime);
                yield return null;
            }

            // Lo destruimos para liberar memoria (destruirá también la nota pegada)
            Destroy(examenInstanciado);
            examenInstanciado = null;
        }
    }

    public IEnumerator Decir(string[] lineasDiálogo, bool regenerarBoton)
    {
        if (botonHablarInstanciado != null)
        {
            Animator animBoton = botonHablarInstanciado.GetComponentInChildren<Animator>();
            if (animBoton != null) animBoton.SetTrigger("Desaparecer");

            yield return new WaitForSeconds(0.333f);

            Destroy(botonHablarInstanciado);
            botonHablarInstanciado = null;
        }

        textoUI.text = "";
        foreach (string linea in lineasDiálogo)
        {
            animator.SetBool("Hablando", true);
            textoUI.text = "";

            avanzarTexto = false;

            foreach (char letra in linea.ToCharArray())
            {
                textoUI.text += letra;

                // Si mantienes pulsada la 'P', omite el tiempo de espera entre letras
                if (!Input.GetKey(KeyCode.P))
                {
                    yield return new WaitForSeconds(tiempoPorLetra);
                }
            }

            animator.SetBool("Hablando", false);

            avanzarTexto = false;
            yield return new WaitUntil(() => !Input.GetKey(KeyCode.Space));
            yield return null;

            // La tecla 'P' también permite avanzar al instante a la siguiente línea de diálogo
            yield return new WaitUntil(() => avanzarTexto || Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.P));
        }
        textoUI.text = "";

        if (regenerarBoton && botonHablarPrefab != null)
        {
            botonHablarInstanciado = Instantiate(botonHablarPrefab, posicionBoton, Quaternion.identity);
        }
    }

    public IEnumerator DecirAleatorio(string[] opcionesAleatorias)
    {
        int indiceAleatorio = Random.Range(0, opcionesAleatorias.Length);
        string lineaElegida = opcionesAleatorias[indiceAleatorio];

        yield return StartCoroutine(Decir(new string[] { lineaElegida }, true));
    }
}