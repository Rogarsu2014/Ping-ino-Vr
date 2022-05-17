using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class puntuacion : MonoBehaviour
{
    public static int puntos = 0;
    public TextMeshProUGUI textoPuntuacion;

    void Start()
    {
        puntos = 0;
    }

        public void sumaPuntos ()
    {
        puntos += 10;

        muestraPuntuacion(puntos);
    }

    public void restaPuntos()
    {
        puntos -= 2;

        muestraPuntuacion(puntos);
    }

    void muestraPuntuacion(int puntos)
    {
        textoPuntuacion.text = string.Format("{0:0}", puntos);
    }

    public static int GetPuntos()
    {
        return puntos;
    }
}
