using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public Canvas canvasTutorial;
    public Canvas canvasAvanceFase;
    public Canvas canvasHeadsUp;
    public Canvas canvasCreditos;

    public GameObject jugador;

    float tiempoCreditos = 10;

    // Start is called before the first frame update
    void Start()
    {
        //GameLogic.inicializaPartida(canvasTutorial, canvasAvanceFase, canvasHeadsUp, jugador);
        canvasTutorial.gameObject.SetActive(true);
        canvasAvanceFase.gameObject.SetActive(true);
        canvasHeadsUp.gameObject.SetActive(false);
        canvasCreditos.gameObject.SetActive(false);

        Vector3 posicion;
        posicion.x = (float)14.524;
        posicion.y = (float)0.426;
        posicion.z = (float)-6.47;

        jugador.gameObject.transform.position = posicion;
    }

    void Update()
    {
        if (canvasCreditos.gameObject.activeSelf)
        {
            TextMeshProUGUI textoCreditos = new TextMeshProUGUI();
            //TextMeshProUGUI puntos = canvasHeadsUp.transform.Find("Puntuacion").gameObject.GetComponent<TextMeshPro>;
            //textoCreditos.text = string.Format("Tu Puntuacion: " + ;

            if (tiempoCreditos > 0)
            {
                tiempoCreditos -= Time.deltaTime;
            }
            else
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
            }
        }
    }

    public void inicioJuego()
    {
        //GameLogic.avanceFase(canvasTutorial, canvasAvanceFase, canvasHeadsUp, jugador);
        canvasTutorial.gameObject.SetActive(false);
        canvasAvanceFase.gameObject.SetActive(false);
        canvasHeadsUp.gameObject.SetActive(true);
        canvasCreditos.gameObject.SetActive(false);

        Vector3 posicion;
        posicion.x = (float)14.524;
        posicion.y = (float)0.07;
        posicion.z = (float)-2.328;

        jugador.gameObject.transform.position = posicion;
    }

    public static void finJuego(GameObject jugador, Canvas canvasCreditos, Canvas canvasHeadsUp)
    {
        canvasHeadsUp.gameObject.SetActive(false);
        canvasCreditos.gameObject.SetActive(true);

        Vector3 posicion;
        posicion.x = (float)14.524;
        posicion.y = (float)0.426;
        posicion.z = (float)-6.47;

        jugador.gameObject.transform.position = posicion;
    }
}
