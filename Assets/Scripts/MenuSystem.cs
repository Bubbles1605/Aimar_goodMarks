using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void jugar()
    {
        SceneManager.LoadScene("Clase");
    }
    public void salir()
    {
        Application.Quit();
    }
    public void Opciones()
    {
        SceneManager.LoadScene("Opciones");
    }
    public void creditos()
    {
        SceneManager.LoadScene("Créditos");
    }
}