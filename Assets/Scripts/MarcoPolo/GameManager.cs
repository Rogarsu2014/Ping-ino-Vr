using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
  
/* Esta clase GameManager controla el funcionamiento del juego Marco Polo */
public class GameManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> dianas;
    private int points;

    public TextMeshProUGUI textMeshPuntos;
    public TextMeshProUGUI textMeshTiempo;

    float tiempoResto;
    float tiempoFinal;
    bool inicioJuego;

    // Start is called before the first frame update
    void Start()
    {
        tiempoResto = 30f;
        tiempoFinal = 10f;
        points = 0;
        foreach (var diana in dianas)
        {
            randomDianaPos(diana);
        }
        inicioJuego = false;
        
    }

    //Funcion que coloca la diana en una posición aleatoria
    public void randomDianaPos(GameObject d)
    {
        Vector3 pos = new Vector3(Random.Range(-30f, 30f), 4, Random.Range(-30f, 30f));
        d.transform.position = pos;
    }

    //Función que añade puntos y cambia el valor del texto donde se muestra
    public void addPoints()
    {
        points += 1;
        print(points);
        print(points.ToString());
        textMeshPuntos.text = points.ToString();
    }


    private void FixedUpdate()
    {
        //Se comprueba si el juego a comenzado (comienza cuando se toma un arma)
        if ( inicioJuego)
        { 
            //Cada diana se orienta hacia el jugador
            foreach(var d in dianas)
            {
                lookAtPlayer(d, player.transform);
            }
            //Se calcula y muestra el tiempo que falta
            tiempoResto -= Time.deltaTime;
            float segundos = Mathf.FloorToInt(tiempoResto);
            textMeshTiempo.text = string.Format("{0:00}", segundos);

            //Cuando el tiempo acaba se llama a gameOver()
            if(tiempoResto <=0)
            {
                gameOver();
            }

        }
    }

    private void lookAtPlayer(GameObject diana, Transform t)
    {
        Quaternion _lookRotation = Quaternion.LookRotation((t.position - diana.transform.position).normalized);
        diana.transform.rotation = _lookRotation;
    }

    public void startGame()
    {
        inicioJuego = true;
    }

    //Envia al jugador a otra plataforma y muestra la puntuación final, al cabo de unos segundos, devuelve al menú 
    void gameOver()
    {
        player.transform.position = new Vector3(5000, 0, 0);
        tiempoFinal -= Time.deltaTime;
        float finales = Mathf.FloorToInt(tiempoFinal);

        textMeshTiempo.text = string.Format("{0:00}", finales);
        textMeshPuntos.text = string.Format("Puntuación final: {000}",points.ToString());
        textMeshPuntos.color = Color.blue;
        if(tiempoFinal <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
