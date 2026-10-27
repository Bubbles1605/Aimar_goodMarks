using System.Collections;
using System.Collections.Generic; // NUEVO: Necesario para usar Listas
using UnityEngine;
using TMPro;
using UnityEngine.UI; // NUEVO: Necesario para detectar clics en los botones

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

    [Header("Sistema de Preguntas (NUEVO)")]
    public GameObject prefabBotonRespuesta; // El prefab del botón que hará de opción
    public Transform contenedorRespuestas;  // Un panel en tu Canvas donde se apilarán los botones
    [HideInInspector] public int respuestaElegida = -1; // Guardará qué botón se ha pulsado
    private List<GameObject> botonesInstanciados = new List<GameObject>(); // Lista para borrarlos luego

    [Header("Examen")]
    public GameObject prefabExamen;
    public float posicionXExamen = 16.15f;
    public float posicionYInicialExamen = 8f;
    public float posicionYFinalExamen = 1.25f;
    public float velocidadCaidaExamen = 7f;
    [HideInInspector] public GameObject examenInstanciado;

    [Header("Sistema de Muerte")]
    public GameObject panelMuerte; // Arrastra aquí el panel desde el Canvas
    public float duracionAnimacionMuerte = 2.0f;

    // LA VARIABLE MÁGICA
    [HideInInspector] public bool examenCalificado = false;

    // --- NUEVO: APARTADO DE SONIDOS ---
    [Header("Sonidos")]
    public AudioClip sonidoMoverPapel;
    private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    public Animator animator;
    private GameObject botonHablarInstanciado;

    [HideInInspector] public bool avanzarTexto = false;
    [HideInInspector] public int notaRecibida = -1;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Creamos un AudioSource automáticamente si el alumno no lo tiene
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
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

    public void DesplegarPanelMuerte()
    {
        if (panelMuerte != null)
        {
            panelMuerte.SetActive(true);
        }

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

        // 1. Destruimos el botón viejo si ya había uno en la mesa
        if (botonHablarInstanciado != null)
        {
            Destroy(botonHablarInstanciado);
            botonHablarInstanciado = null;
        }

        // 2. Ahora sí, instanciamos el nuevo limpiamente
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
            // --- NUEVO SONIDO: RETIRAN EL EXAMEN ---
            if (sonidoMoverPapel != null) audioSource.PlayOneShot(sonidoMoverPapel);

            Vector3 posicionDestino = new Vector3(posicionXExamen, posicionYInicialExamen, 0f);

            SpriteRenderer spriteExamen = examenInstanciado.GetComponentInChildren<SpriteRenderer>();
            if (spriteExamen != null) spriteExamen.sortingOrder = 11;

            while (Vector3.Distance(examenInstanciado.transform.position, posicionDestino) > 0.01f)
            {
                examenInstanciado.transform.position = Vector3.MoveTowards(examenInstanciado.transform.position, posicionDestino, velocidadCaidaExamen * Time.deltaTime);
                yield return null;
            }

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

                if (!Input.GetKey(KeyCode.P))
                {
                    yield return new WaitForSeconds(tiempoPorLetra);
                }
            }

            animator.SetBool("Hablando", false);

            avanzarTexto = false;
            yield return new WaitUntil(() => !Input.GetKey(KeyCode.Space));
            yield return null;

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

    // --- NUEVO: FUNCIÓN PARA ELEGIR RESPUESTAS ---
    public IEnumerator Preguntar(string[] opciones)
    {
        // 1. Ocultar el botón de hablar si estuviera activo
        if (botonHablarInstanciado != null)
        {
            Destroy(botonHablarInstanciado);
            botonHablarInstanciado = null;
        }

        // 2. Limpiar las opciones anteriores por seguridad
        respuestaElegida = -1;
        botonesInstanciados.Clear();

        // 3. Crear un botón por cada opción que hayamos escrito
        for (int i = 0; i < opciones.Length; i++)
        {
            int indiceActual = i; // Guardamos el número de este botón para que sepa quién es

            // Creamos el botón dentro del contenedor
            GameObject nuevoBoton = Instantiate(prefabBotonRespuesta, contenedorRespuestas);

            // Le ponemos el texto de la respuesta
            TextMeshProUGUI textoBoton = nuevoBoton.GetComponentInChildren<TextMeshProUGUI>();
            if (textoBoton != null)
            {
                textoBoton.text = opciones[i];
            }

            // Le añadimos la orden de qué hacer al pulsarlo
            Button botonUI = nuevoBoton.GetComponent<Button>();
            if (botonUI != null)
            {
                botonUI.onClick.AddListener(() =>
                {
                    respuestaElegida = indiceActual; // Al pulsar, guarda qué botón fue
                });
            }

            botonesInstanciados.Add(nuevoBoton); // Lo guardamos en la lista para borrarlo luego
        }

        // 4. Pausar el juego hasta que el jugador pulse una respuesta
        yield return new WaitUntil(() => respuestaElegida != -1);

        // 5. El jugador ya ha elegido, así que destruimos todos los botones
        foreach (GameObject btn in botonesInstanciados)
        {
            Destroy(btn);
        }
        botonesInstanciados.Clear();
    }
}