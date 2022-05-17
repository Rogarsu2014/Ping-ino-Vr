using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Canvas canvasTutorial;
    public Canvas canvasAvanceFase;
    public Canvas canvasHeadsUp;

    public GameObject jugador;

    public bool inGame = false;

    // Start is called before the first frame update
    void Start()
    {
        canvasTutorial.gameObject.SetActive(true);
        canvasAvanceFase.gameObject.SetActive(true);
        canvasHeadsUp.gameObject.SetActive(false);

        Vector3 posicion;
        posicion.x = (float)14.524;
        posicion.y = (float)0.426;
        posicion.z = (float)-6.47;

        jugador.gameObject.transform.position = posicion;
    }

    // Update is called once per frame
    void Update()
    {
        if (inGame)
        {

        }
    }

    public void avanceFase()
    {
        inGame = true;

        canvasTutorial.gameObject.SetActive(false);
        canvasAvanceFase.gameObject.SetActive(false);
        canvasHeadsUp.gameObject.SetActive(true);

        Vector3 posicion;
        posicion.x = (float)14.524;
        posicion.y = (float)0.07;
        posicion.z = (float)-2.316;

        jugador.gameObject.transform.position = posicion;
    }
}
