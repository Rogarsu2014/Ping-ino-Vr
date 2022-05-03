using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class AvionGameLogic : MonoBehaviour
{
    public float restoTiempo = 30;
    private bool sigueContando = false;
    //public Text tiempo;
    public TextMeshProUGUI tiempo;

    public GameObject player;



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
                cambiarPos();

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

    private void cambiarPos()
    {
        player.transform.position = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
        player.transform.position = GameObject.FindGameObjectWithTag("Pico").transform.position;

        //Cambiar el tamaño del avión para que sea más grande

        //Para Fase 2 poner un Canvas dentro del XR Origin y ponerlo en screen space - camera con la distancia que llevas en directo

        GameObject manoI = player.GetNamedChild("LeftHand Controller");
        GameObject manoD = player.GetNamedChild("RightHand Controller");

        manoI.SetActive(false);
        manoD.SetActive(false);

        GameObject ca = GameObject.FindGameObjectWithTag("MainCamera");
        //teamca.GetComponent<>

    }

    private void TerminarFase2()
    {
        //Cuando y <= 0 o el avion se choque contra el mesh del escenario

        //Deten el avion == desactivar todo lo del vuelo

        //Calcular la distancia desde el origen (en línea recta desde el suelo) (resta de x)

        //Guardar esa puntuación y mostrarla por pantalla (nuevo record o no)
    }
}
