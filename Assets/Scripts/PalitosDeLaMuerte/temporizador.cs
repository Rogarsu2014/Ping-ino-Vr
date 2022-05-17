using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class temporizador : MonoBehaviour
{
    public float tiempo = 120;
    public TextMeshProUGUI textoTiempo;

    // Update is called once per frame
    void Update()
    {
        if (tiempo > 0)
        {
            tiempo -= Time.deltaTime;
        }
        else
        {
            tiempo = 0;
        }

        muestraTiempo(tiempo);
    }

    void muestraTiempo(float tiempoAMostrar)
    {
        if (tiempoAMostrar < 0)
        {
            tiempoAMostrar = 0;
        }

        float minutos = Mathf.FloorToInt(tiempoAMostrar / 60);
        float segundos = Mathf.FloorToInt(tiempoAMostrar % 60);

        textoTiempo.text = string.Format("{0:0}:{1:00}", minutos, segundos);
    }
}