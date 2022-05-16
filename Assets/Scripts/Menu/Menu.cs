using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public int escena; //La escena a la que se va a ir

    public void Llamada() //El método que se llama para ejecutar la corutina que activa la llamada a la siguiente escena
    {
        StartCoroutine(LoadYourAsyncScene());
    }
    public IEnumerator LoadYourAsyncScene() //Función que carga la escena del menú (sacada de la documentación de Unity)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(escena);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void Salir() //Función que se llama para salir del juego
    {
        Application.Quit();
    }
}
