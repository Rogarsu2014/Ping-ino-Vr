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

    //public GameObject player;
    private GameObject manoI;
    private GameObject manoD;

    private GameObject ad1p;
    private GameObject ai1p;



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
                sigueContando = false;
                cambiarPos();
                CambioFase();

                restoTiempo = 0;//U otro valor si queremos resetear el timer

            }
        }else if (fase2)
        {
            ActualizarPosicionesJugadores();
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
            avion.transform.position = new Vector3(30*i, 2000, 0);
            avion.transform.rotation = Quaternion.Euler(0,180,0);

            //avion.transform.localScale += new Vector3(3f, 3f, 3f);

            avion.GetComponent<Rigidbody>().useGravity = true;
            avion.GetComponent<AircraftPhysics>().enabled = true;


            //Poner al jugador donde el avion
            //Habrá que cambiarlo para el multijugador (tanto esto como la asignacion del avion al personaje)
            //player.Origin = avion; //Esto no funciona
            avion1 = avion;
            ad1p = avion1.GetNamedChild("AD1Pivot");
            ai1p = avion1.GetNamedChild("AI1Pivot");

            //Cambiamos i
            i += 1;
        }

        //Pasar a la fase 2
        fase2 = true;
        manoI.SetActive(true);
        manoD.SetActive(true);
    }

    private void ActualizarPosicionesJugadores()
    {
        //Poner al jugador donde esta el avion (en la punta)
        Vector3 pos = avion1.GetNamedChild("PicoPivot").transform.position;
        //pos.y -= player.GetNamedChild("Main Camera").transform.position.y + 1f;
        player.transform.position = pos;

        //Cambiar la rotacion del ala

        Vector3 rot = ad1p.transform.localRotation.eulerAngles;
        float manoDZ = manoD.transform.rotation.eulerAngles.z;
        rot = new Vector3(rot.x, rot.y, manoDZ);
        ad1p.transform.localRotation = Quaternion.Euler(rot);


        Vector3 rot2 = ai1p.transform.localRotation.eulerAngles;
        float manoIZ = manoI.transform.rotation.eulerAngles.z;
        rot = new Vector3(rot.x, rot.y, manoIZ);
        ai1p.transform.localRotation = Quaternion.Euler(rot);

    }

    private void cambiarPos()
    {
        
        //player.transform.position = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        //player.transform.rotation = Quaternion.Euler(0, 180, 0);
        //player.transform.position = GameObject.FindGameObjectWithTag("Pico").transform.position;

        //Cambiar el tamaño del avión para que sea más grande

        //Para Fase 2 poner un Canvas dentro del XR Origin y ponerlo en screen space - camera con la distancia que llevas en directo

        manoI = player.GetNamedChild("LeftHand Controller");
        manoD = player.GetNamedChild("RightHand Controller");


        manoI.SetActive(false);
        manoD.SetActive(false);



        //GameObject ca = GameObject.FindGameObjectWithTag("MainCamera");
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
