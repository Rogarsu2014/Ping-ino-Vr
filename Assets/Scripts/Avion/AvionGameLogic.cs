using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using System.IO;

public class AvionGameLogic : MonoBehaviour
{
    public float restoTiempo = 30;
    private bool sigueContando = false;
    private bool cambioDeFase = true;
    private bool noChocado = true;

    public TextMeshProUGUI tiempo;
    public Canvas textoTutorial;
    public TextMeshProUGUI distancia;
    public TextMeshProUGUI newRecord;
    public TextMeshProUGUI puntos;

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

    public GameObject[] escena;



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

                textoTutorial.gameObject.SetActive(true);

                restoTiempo = 15;//U otro valor si queremos resetear el timer

            }
        }else if (cambioDeFase)
        {
            if (restoTiempo > 0)
            {
                restoTiempo -= Time.deltaTime;
                MostrarTiempo(restoTiempo);
                //Animacion tutorial
            }
            else
            {
                cambioDeFase = false;
                CambioFase();
                restoTiempo = 0;
                DesactivarTodo();
                distancia.gameObject.SetActive(true);
            }
        }
        else if (fase2)
        {
            ActualizarPosicionesJugadores();
            if (noChocado)
            {
                ActualizarDistancias();
            }
            else
            {
                fase2 = false;
                TerminarFase2();
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
        pos.y -= 1f;
        pos.z -= 0.5f;
        player.transform.position = pos;

        //Cambiar la rotacion del ala

        Vector3 rot = ad1p.transform.localRotation.eulerAngles;
        float manoDZ = manoD.transform.rotation.eulerAngles.z;
        rot = new Vector3(rot.x, rot.y, -manoDZ);
        ad1p.transform.localRotation = Quaternion.Euler(rot);


        Vector3 rot2 = ai1p.transform.localRotation.eulerAngles;
        float manoIZ = manoI.transform.rotation.eulerAngles.z;
        rot = new Vector3(rot.x, rot.y, -manoIZ);
        ai1p.transform.localRotation = Quaternion.Euler(rot);

    }

    private void ActualizarDistancias()
    {
        distancia.text = ((int)player.transform.position.z).ToString();
    }

    private void CheckColision()
    {
        noChocado = false;
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

        escena[0].gameObject.SetActive(true);
        escena[1].gameObject.SetActive(true);

        //GameObject ca = GameObject.FindGameObjectWithTag("MainCamera");
        //teamca.GetComponent<>

    }

    private void TerminarFase2()
    {
        //Deten el avion == desactivar todo lo del vuelo
        avion1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        avion1 .GetComponent<AircraftPhysics>().enabled = false;

        //Calcular la distancia desde el origen (en línea recta desde el suelo) (resta de x)
        int puntuacion1;
        puntuacion1 = int.Parse(distancia.text);

        //Guardar esa puntuación y mostrarla por pantalla (nuevo record o no)
        StreamReader read = new StreamReader("MaxScoreAvion.txt");
        int maxScore = int.Parse(read.ReadLine());
        read.Close();

        distancia.gameObject.SetActive(false);

        puntos.text = "Puntuación: " + puntuacion1.ToString();
        puntos.gameObject.SetActive(true);


        if(puntuacion1 > maxScore)
        {
            newRecord.gameObject.SetActive(true);
            StreamWriter write = new StreamWriter("MaxScoreAvion.txt", false);
            write.WriteLine(puntuacion1);
            write.Close();
        }

        //Despues de 10 segundos volver al menu principal
    }

    private void DesactivarTodo()
    {
        foreach(GameObject g in escena)
        {
            g.SetActive(false);
        }
    }
}
