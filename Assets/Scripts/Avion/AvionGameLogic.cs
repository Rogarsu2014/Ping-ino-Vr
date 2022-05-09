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
    private GameObject[] aviones;
    
    //Movimiento del jugador
    public GameObject player;
    private bool fase2 = false;
    private GameObject avion1;


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

                //Cambiar el tamaño del jugador
                player.transform.localScale -= new Vector3(0.03f, 0.03f, 0.03f);
            }
            else
            {
                //Llamar a metodo cambiar de fase
                CambioFase();
                restoTiempo = 0;//U otro valor si queremos resetear el timer
                sigueContando = false;
            }
        }else if (fase2)
        {
            //ActualizarPosicionesJugadores();
        }
    }

    public void MostrarTiempo(float tiempoResto)
    {
        tiempoResto += 1;
        float minutos = Mathf.FloorToInt(tiempoResto / 60);
        float segundos = Mathf.FloorToInt(tiempoResto % 60);
        tiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public void CambioFase()
    {
        if (aviones == null)
        {
            aviones = GameObject.FindGameObjectsWithTag("Avion");
        }
        int i = 0;
        foreach(GameObject avion in aviones)
        {
            //Desactivar que se pueda agarrar el avion
            XRGrabInteractable[] interactables = avion.GetComponentsInChildren<XRGrabInteractable>();
            foreach(XRGrabInteractable grab in interactables)
            {
                grab.enabled = false;
            }

            //Cambiar las posiciones a lo alto
            avion.transform.position = new Vector3(30*i, 200, 0);


            //Poner al jugador donde el avion
            //Habrá que cambiarlo para el multijugador (tanto esto como la asignacion del avion al personaje)
            //player.Origin = avion; //Esto no funciona
            avion1 = avion;

            //Cambiamos i
            i += 1;
        }

        //Pasar a la fase 2
        fase2 = true;
    }

    private void ActualizarPosicionesJugadores()
    {
        //Poner al jugador donde esta el avion (en la punta)
        Vector3 pos = avion1.GetNamedChild("PicoPivot").transform.position;
        //pos.y -= player.CameraYOffset;
        player.transform.position = pos;

        //Cambiar la rotacion del jugador
        Vector3 rot = avion1.transform.rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y + 180, rot.z);
        player.transform.rotation = Quaternion.Euler(rot);

        //Cambiar el tamaño del jugador
        player.transform.localScale -= new Vector3(0.03f, 0.03f, 0.03f);
    }
}
