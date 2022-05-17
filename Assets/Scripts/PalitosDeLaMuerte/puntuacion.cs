using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class puntuacion : MonoBehaviour
{
    public int puntos = 0;
    public TextMeshProUGUI textoPuntuacion;

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
}
