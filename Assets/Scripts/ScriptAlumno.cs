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

    [Header("Diálogos de este Alumno")] // Cada uno tiene los suyos en el Inspector
    public string[] dialogoPrincipal;
    public string[] frasesGenericas;
    [HideInInspector] public bool primerDialogoDicho = false;

    [Header("Interfaz y Diálogo")]
    public GameObject botonHablarPrefab;
    public Vector3 posicionBoton = new Vector3(-1.96f, 1.82f, 0f);

    public TextMeshProUGUI textoUI;
    public float tiempoPorLetra = 0.05f;

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