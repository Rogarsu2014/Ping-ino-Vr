using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AvionGameLogic : MonoBehaviour
{
    public float restoTiempo = 30;
    private bool sigueContando = false;
    //public Text tiempo;
    public TextMeshProUGUI tiempo;


    private void Start()
    {
        sigueContando = true; //Para inicializar el contador
        //Es posible que en multijugador haya que no empezar el contador al principio
        //Sino comprobar la sincornización de los jugadores antes de comenzarlo.
    }
    void FixedUpdate()
    {
        if (sigueContando)
        {
            if (restoTiempo > 0)
            {
                restoTiempo -= Time.deltaTime;
                MostrarTiempo(restoTiempo);
            }
            else
            {
                //Llamar a metodo cambiar de fase
                restoTiempo = 0;//U otro valor si queremos resetear el timer
                sigueContando = false;
            }
        }
    }

    public void MostrarTiempo(float tiempoResto)
    {
        tiempoResto += 1;
        float minutos = Mathf.FloorToInt(tiempoResto / 60);
        float segundos = Mathf.FloorToInt(tiempoResto % 60);
        tiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}
