using System.Collections;
using UnityEngine;
using TMPro;

public class ScriptAlumno : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidadAparicion = 5f;
    public float posicionYInicial = -8f;
    public float posicionYFinal = -0.65f;
    public KeyCode teclaAparicion = KeyCode.Space;

    [Header("Diálogos de este Alumno")]
    public string[] dialogoPrincipal;
    public string[] frasesGenericas;
    [HideInInspector] public bool primerDialogoDicho = false;

    [Header("Interfaz y Diálogo")]
    public GameObject botonHablarPrefab;
    public Vector3 posicionBoton = new Vector3(-1.96f, 1.82f, 0f);

    public TextMeshProUGUI textoUI;
    public float tiempoPorLetra = 0.05f;

    [Header("Examen")] // NUEVO: Ajustes para la entrega del examen
    public GameObject prefabExamen;
    public float posicionXExamen = 16.15f;
    public float posicionYInicialExamen = 8f; // Altura desde la que empieza a caer
    public float posicionYFinalExamen = 1.25f; // Altura en la que se posa en la mesa
    public float velocidadCaidaExamen = 7f;
    [HideInInspector] public GameObject examenInstanciado;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private GameObject botonHablarInstanciado;

    [HideInInspector] public bool avanzarTexto = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(teclaAparicion))
        {
            StartCoroutine(Aparecer());
        }
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

        if (botonHablarPrefab != null)
        {
            botonHablarInstanciado = Instantiate(botonHablarPrefab, posicionBoton, Quaternion.identity);
        }
    }

    // NUEVA FUNCIÓN: Entrega del examen
    public IEnumerator EntregarExamen()
    {
        if (prefabExamen != null)
        {
            // 1. Definimos de dónde sale y a dónde va
            Vector3 posicionSalida = new Vector3(posicionXExamen, posicionYInicialExamen, 0f);
            Vector3 posicionMesa = new Vector3(posicionXExamen, posicionYFinalExamen, 0f);

            // 2. Creamos el examen arriba del todo
            examenInstanciado = Instantiate(prefabExamen, posicionSalida, Quaternion.identity);

            // 3. Le asignamos la capa 11 al SpriteRenderer del examen
            SpriteRenderer spriteExamen = examenInstanciado.GetComponent<SpriteRenderer>();
            if (spriteExamen != null)
            {
                spriteExamen.sortingOrder = 11;
            }

            // 4. Lo bajamos poco a poco usando la distancia para evitar el bucle infinito
            while (Vector3.Distance(examenInstanciado.transform.position, posicionMesa) > 0.01f)
            {
                examenInstanciado.transform.position = Vector3.MoveTowards(examenInstanciado.transform.position, posicionMesa, velocidadCaidaExamen * Time.deltaTime);
                yield return null;
            }

            // 5. Lo clavamos en su posición final por si hay un error microscópico
            examenInstanciado.transform.position = posicionMesa;
        }
        else
        {
            Debug.LogWarning("Ojo: No has asignado el Prefab del Examen en el Inspector del alumno.");
        }
    }

    public IEnumerator Decir(string[] lineasDiálogo)
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
                yield return new WaitForSeconds(tiempoPorLetra);

                if (avanzarTexto || Input.GetKey(KeyCode.Return))
                {
                    textoUI.text = linea;
                    break;
                }
            }

            animator.SetBool("Hablando", false);
            yield return new WaitUntil(() => !Input.GetKey(KeyCode.Return));
            avanzarTexto = false;
            yield return null;

            yield return new WaitUntil(() => avanzarTexto || Input.GetKeyDown(KeyCode.Return));
        }
        textoUI.text = "";

        if (botonHablarPrefab != null)
        {
            botonHablarInstanciado = Instantiate(botonHablarPrefab, posicionBoton, Quaternion.identity);
        }
    }

    public IEnumerator DecirAleatorio(string[] opcionesAleatorias)
    {
        int indiceAleatorio = Random.Range(0, opcionesAleatorias.Length);
        string lineaElegida = opcionesAleatorias[indiceAleatorio];

        yield return StartCoroutine(Decir(new string[] { lineaElegida }));
    }
}