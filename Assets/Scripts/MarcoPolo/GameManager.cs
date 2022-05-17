using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
 
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

    public void randomDianaPos(GameObject d)
    {
        Vector3 pos = new Vector3(Random.Range(-30f, 30f), 4, Random.Range(-30f, 30f));
        d.transform.position = pos;
    }


    public void addPoints()
    {
        points += 1;
        print(points);
        print(points.ToString());
        textMeshPuntos.text = points.ToString();
    }

    private void FixedUpdate()
    {
        if ( inicioJuego)
        { 
        foreach(var d in dianas)
        {
            lookAtPlayer(d, player.transform);
        }
        tiempoResto -= Time.deltaTime;

        float segundos = Mathf.FloorToInt(tiempoResto);

        textMeshTiempo.text = string.Format("{0:00}", segundos);

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
