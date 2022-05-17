using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public Canvas canvasTutorial;
    public Canvas canvasAvanceFase;
    public Canvas canvasHeadsUp;

    public GameObject jugador;

    public GameObject totem;

    // Start is called before the first frame update
    void Start()
    {
        //GameLogic.inicializaPartida(canvasTutorial, canvasAvanceFase, canvasHeadsUp, jugador);
        canvasTutorial.gameObject.SetActive(true);
        canvasAvanceFase.gameObject.SetActive(true);
        canvasHeadsUp.gameObject.SetActive(false);

        Vector3 posicion;
        posicion.x = (float)14.524;
        posicion.y = (float)0.426;
        posicion.z = (float)-6.47;

        jugador.gameObject.transform.position = posicion;
    }

    public void inicioJuego()
    {
        //GameLogic.avanceFase(canvasTutorial, canvasAvanceFase, canvasHeadsUp, jugador);
        canvasTutorial.gameObject.SetActive(false);
        canvasAvanceFase.gameObject.SetActive(false);
        canvasHeadsUp.gameObject.SetActive(true);

        Vector3 posicion;
        posicion.x = (float)14.524;
        posicion.y = (float)0.07;
        posicion.z = (float)-2.328;

        jugador.gameObject.transform.position = posicion;
    }

    public static void finJuego(GameObject jugador)
    {
        Vector3 posicion;
        posicion.x = (float)14.524;
        posicion.y = (float)0.426;
        posicion.z = (float)-6.47;

        jugador.gameObject.transform.position = posicion;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
    }
}
