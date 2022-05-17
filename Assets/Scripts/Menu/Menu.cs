using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public int escena; //La escena a la que se va a ir

    public void Llamada() //El m�todo que se llama para ejecutar la corutina que activa la llamada a la siguiente escena
    {
        StartCoroutine(LoadYourAsyncScene());
    }
    public IEnumerator LoadYourAsyncScene() //Funci�n que carga la escena del men� (sacada de la documentaci�n de Unity)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(escena);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void Salir() //Funci�n que se llama para salir del juego
    {
        Application.Quit();
    }
}
