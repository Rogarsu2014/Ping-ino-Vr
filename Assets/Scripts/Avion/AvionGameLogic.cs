using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    
    public GameObject player;
    private bool fase2 = false;
    private GameObject avion1;

    private GameObject manoI;
    private GameObject manoD;

    private GameObject ad1p;
    private GameObject ai1p;

    public GameObject[] escena;
    [SerializeField] InputActionReference triggerD;
    [SerializeField] InputActionReference triggerI;

    public GameObject cubo;

    private bool final = false;

    public GameObject[] audioSources;


    private void Start()
    {
        sigueContando = true;
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
                sigueContando = false;
                cambiarPos();

                escena[4].gameObject.SetActive(false);
                escena[6].gameObject.SetActive(false);

                textoTutorial.gameObject.SetActive(true);

                restoTiempo = 20;

            }
        }
        else if (cambioDeFase)
        {
            if (restoTiempo > 0)
            {
                restoTiempo -= Time.deltaTime;
                MostrarTiempo(restoTiempo);
            }
            else
            {
                cambioDeFase = false;
                CambioFase();
                restoTiempo = 10;
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
                if (triggerD.action.IsPressed() && !triggerI.action.IsPressed())
                {
                    giroPicoDerecha(triggerD.action.ReadValue<float>());
                }
                else if (!triggerD.action.IsPressed() && triggerI.action.IsPressed())
                {
                    giroPicoIzquierda(triggerI.action.ReadValue<float>());
                }
            }
            else
            {
                fase2 = false;
                TerminarFase2();
            }
        }
        else if (final)
        {
            if (restoTiempo > 0)
            {
                restoTiempo -= Time.deltaTime;
                MostrarTiempo(restoTiempo);
            }
            else
            {
                StartCoroutine(LoadYourAsyncScene());
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
            XRGrabInteractable[] interactables = avion.GetComponentsInChildren<XRGrabInteractable>();
            foreach(XRGrabInteractable grab in interactables)
            {
                grab.enabled = false;
            }

            avion.transform.position = new Vector3(30*i, 2000, 0);
            avion.transform.rotation = Quaternion.Euler(0,180,0);


            avion.GetComponent<Rigidbody>().useGravity = true;
            avion.GetComponent<AircraftPhysics>().enabled = true;


            avion1 = avion;
            ad1p = avion1.GetNamedChild("AD1Pivot");
            ai1p = avion1.GetNamedChild("AI1Pivot");

            i += 1;
        }

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
        manoI = player.GetNamedChild("LeftHand Controller");
        manoD = player.GetNamedChild("RightHand Controller");


        manoI.SetActive(false);
        manoD.SetActive(false);

        escena[0].gameObject.SetActive(true);
        escena[1].gameObject.SetActive(true);

    }

    private void TerminarFase2()
    {
        avion1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        avion1 .GetComponent<AircraftPhysics>().enabled = false;

        int puntuacion1;
        puntuacion1 = int.Parse(distancia.text);

        StreamReader read = new StreamReader("MaxScoreAvion.txt");
        int maxScore = int.Parse(read.ReadLine());
        read.Close();

        distancia.gameObject.SetActive(false);

        puntos.text = "Puntuación: " + puntuacion1.ToString();
        puntos.gameObject.SetActive(true);

        if(puntuacion1 > 0)
        {
            if (puntuacion1 > maxScore)
            {
                audioSources[2].GetComponent<AudioSource>().Play();
                newRecord.gameObject.SetActive(true);
                StreamWriter write = new StreamWriter("MaxScoreAvion.txt", false);
                write.WriteLine(puntuacion1);
                write.Close();
            }
            else
            {
                audioSources[0].GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            audioSources[1].GetComponent<AudioSource>().Play();
        }
        

        final = true;
    }

    private void DesactivarTodo()
    {
        foreach(GameObject g in escena)
        {
            g.SetActive(false);
        }
    }

    private void giroPicoDerecha(float f)
    {
        Vector3 rot = avion1.GetNamedChild("PicoPivot").transform.localRotation.eulerAngles;
        float prevRot = rot.x;
        rot.x += f;
        if(prevRot <= 90f && rot.x > 90f)
        {
            rot.x = 90f;
        }
        avion1.GetNamedChild("PicoPivot").transform.localRotation = Quaternion.Euler(rot);
    }

    private void giroPicoIzquierda(float f)
    {
        Vector3 rot = avion1.GetNamedChild("PicoPivot").transform.localRotation.eulerAngles;
        float prevRot = rot.x;
        rot.x -= f;
        if (prevRot >= 270f && rot.x < 270f)
        {
            rot.x = 270f;
        }
        avion1.GetNamedChild("PicoPivot").transform.localRotation = Quaternion.Euler(rot);
    }

    public void CeroTiempo()
    {
        restoTiempo = 0;
        cubo.gameObject.SetActive(false);
    }

    private IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
