using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crono : MonoBehaviour
{

    public float tiempo = 60;
    public bool inGame = false;
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        iniciaTiempo();
    }

    // Update is called once per frame
    void Update()
    {
         if (inGame)
        {
            if (tiempo > 0)
            {
                tiempo -= Time.deltaTime;
            }
            else
            {
                inGame = false;
                tiempo = 0;
                audioSource.Stop();
                StartCoroutine(LoadYourAsyncScene());
            }
        }
    }


    public IEnumerator LoadYourAsyncScene() //Funci�n que carga la escena del men� (sacada de la documentaci�n de Unity)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


    public void iniciaTiempo()
    {
        inGame = true;
    }
}
