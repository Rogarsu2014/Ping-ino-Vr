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
    public float restoTiempo = 30; //Variable usada para contar el tiempo que queda
    private bool sigueContando = false; //Comprobaci�n de que seguimos en la fase 1 (construcci�n)
    private bool cambioDeFase = true; //Comprobaci�n de que seguimos en la fase 1.5 (interludio)
    private bool fase2 = false; //Comprobaci�n de que seguimos en la fase 2 (vuelo)
    
    public bool noChocado = true; //Comprobaci�n de que el avi�n no se ha chocado durante la fase de vuelo

    public TextMeshProUGUI tiempo; //El texto que muestra el tiempo
    public Canvas textoTutorial; //Canvas donde se encuentra el texto de un tutorial
    public TextMeshProUGUI distancia; //Texto que muestra la distancia recorrida durante la fase de vuelo
    public TextMeshProUGUI newRecord; //Texto que aparece cuando se ha superado el record de distancia
    public TextMeshProUGUI puntos; //texto que muestra la puntuaci�n al final del juego

    private GameObject[] aviones; //Array de aviones que de momento no esta en uso debido a la falta del multijugador
    
    public GameObject player; //El jugador (XR Origin)
    private GameObject avion1;//El avi�n 

    private GameObject manoI; //El mando izquierdo
    private GameObject manoD; //El mando derecho

    private GameObject ad1p; //El primer pivote del ala Derecha
    private GameObject ai1p; //El primer pivote del ala Izquierda

    public GameObject[] escena; //Diversos elementos de la escena agrupados para poder ser desactivados al entrar en la fase 2
    [SerializeField] InputActionReference triggerD; //Acci�n referente al gatillo derecho
    [SerializeField] InputActionReference triggerI; //Acci�n referente al gatillo izquierdo

    public GameObject cubo; //El cubo con el que pasamos autom�ticamente de la fase 1 a la fase 1.5

    private bool final = false; //El booleano que nos permite volver al men� al terminar la partida

    public GameObject[] audioSources; //Array de los audioSources de la escena con los sonidos que suena al saber que puntuaci�n se ha conseguido.


    private void Start()
    {
        sigueContando = true;
        //Es posible que en multijugador haya que no empezar el contador al principio
        //Sino comprobar la sincornizaci�n de los jugadores antes de comenzarlo.
    }
    void FixedUpdate()
    {
        if (sigueContando)//Si estamos en la fase 1
        {
            if (restoTiempo > 0)//y queda tiempo
            {
                restoTiempo -= Time.deltaTime;//Resta tiempo
                MostrarTiempo(restoTiempo);//Muestralo
            }
            else //Si no queda tiempo
            {
                sigueContando = false; //cambiamos de fase
                interludio(); //Activamos el interludio

                //Desactivamos los tutoriales de la fase anterior
                escena[4].gameObject.SetActive(false); 
                escena[6].gameObject.SetActive(false);

                textoTutorial.gameObject.SetActive(true); //Activamos el tutorial de esta fase

                restoTiempo = 20; //Ponemos 20 segundos para la siguiente fase

            }
        }
        else if (cambioDeFase) //Si estamos en la fase 1.5
        {
            if (restoTiempo > 0)// y queda tiempo
            {
                restoTiempo -= Time.deltaTime;//Resta tiempo
                MostrarTiempo(restoTiempo);//Muestralo
            }
            else //Si no queda tiempo
            {
                cambioDeFase = false; //Cambiamos a la fase 2
                CambioFase(); //Activamos la funcion que cambia de fase
                restoTiempo = 10; //Ponemos 10 segundos que vamos a esperar cuando acabe la fase 2
                DesactivarTodo(); //Desactivamos todos los objetos del tutorial
                distancia.gameObject.SetActive(true); //Activamos el texto de la distancia
            }
        }
        else if (fase2)//Si estamos en la fase 2
        {
            ActualizarPosicionesJugadores(); //Actualizamos la posicion del jugador
            if (noChocado) //Si no se ha chocado
            {
                ActualizarDistancias(); //Actualizamos las distancias

                //Hacemos una comprobaci�n de si se ha pulsado alg�n gatillo para cambiar la orientaci�n del pico
                if (triggerD.action.IsPressed() && !triggerI.action.IsPressed())
                {
                    giroPicoDerecha(triggerD.action.ReadValue<float>());
                }
                else if (!triggerD.action.IsPressed() && triggerI.action.IsPressed())
                {
                    giroPicoIzquierda(triggerI.action.ReadValue<float>());
                }
            }
            else //Si se ha chocado
            {
                fase2 = false; //Terminamos la fase 2
                TerminarFase2(); //Programa final
            }
        }
        else if (final) //Si estamos esperando al final
        {
            if (restoTiempo > 0)// y queda tiempo
            {
                restoTiempo -= Time.deltaTime;//Resta tiempo
                MostrarTiempo(restoTiempo);//Muestralo
            }
            else //si no queda tiempo
            {
                StartCoroutine(LoadYourAsyncScene()); //Volver al menu principal
            }
        }
    }

    public void MostrarTiempo(float tiempoResto) //Funci�n que muestra el tiempo
    {
        tiempoResto += 1; //Le sumamos uno para que acabe la cuenta atr�s en 0
        float minutos = Mathf.FloorToInt(tiempoResto / 60); //Guardamos los minutos
        float segundos = Mathf.FloorToInt(tiempoResto % 60);//Guardar los segundos
        tiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos); //Ponemos los valores en un texto
    }

    public void CambioFase()//Cambio a la fase 2
    {
        if (aviones == null) //Si no hay aviones guardados 
        {
            aviones = GameObject.FindGameObjectsWithTag("Avion");//Obtenemos todos los aviones de la escena
        }
        int i = 0;
        foreach(GameObject avion in aviones)//Por cada avi�n recogido
        {
            XRGrabInteractable[] interactables = avion.GetComponentsInChildren<XRGrabInteractable>();//cogemos todos los componentes XRGrabInteractable
            foreach(XRGrabInteractable grab in interactables)
            {
                grab.enabled = false;// y los desactivamos, para que en la fase 2 el jugador no los pueda agarrar
            }

            //Ahora mismo el c�digo funciona bien porque hay un solo avi�n, se tendr�a que modificar para un posible multijugador

            avion.transform.position = new Vector3(30*i, 2000, 0);//Ponemos el avi�n alto
            avion.transform.rotation = Quaternion.Euler(0,180,0);//Rotamos el avi�n 180 grados 


            avion.GetComponent<Rigidbody>().useGravity = true; //Activamos la gravedad
            avion.GetComponent<AircraftPhysics>().enabled = true;//Activamos las f�sicas 


            avion1 = avion; //El avi�n 1 va a ser a partir de aqui con el que trabajaremos
            ad1p = avion1.GetNamedChild("AD1Pivot"); //Cogemos el pivote derecho
            ai1p = avion1.GetNamedChild("AI1Pivot"); //Cogemos el pivote izquierdo

            i += 1;
        }

        fase2 = true; //Activamos fase 2

        //Reactivamos las manos
        manoI.SetActive(true);
        manoD.SetActive(true);
    }

    private void ActualizarPosicionesJugadores() //Actualizar la posici�n del jugador y el avi�n
    {
        //Poner al jugador donde esta el avion (en la punta)
        Vector3 pos = avion1.GetNamedChild("PicoPivot").transform.position; //Pillamos la posici�n del pico
        pos.y -= 1f; //Bajamos un poco el jugador
        pos.z -= 0.5f; //Hacemos retroceder al jugador un poco
        player.transform.position = pos; //le asignamos esta posici�n al jugador

        //Cambiar la rotacion del ala

        Vector3 rot = ad1p.transform.localRotation.eulerAngles; //Cogemos la rotaci�n del pivote derecho
        float manoDZ = manoD.transform.rotation.eulerAngles.z; //Cogemos la rotaci�n del mando derecho
        rot = new Vector3(rot.x, rot.y, -manoDZ); //Hacemos una mezcla pero solo usando la rotaci�n en Z del mando
        ad1p.transform.localRotation = Quaternion.Euler(rot); //Se la asignamos al pivote


        Vector3 rot2 = ai1p.transform.localRotation.eulerAngles; //Cogemos la rotaci�n del pivote izquierdo
        float manoIZ = manoI.transform.rotation.eulerAngles.z; //Cogemos la rotaci�n del mando izquierdo
        rot = new Vector3(rot.x, rot.y, -manoIZ); //Hacemos una mezcla pero solo usando la rotaci�n en Z del mando
        ai1p.transform.localRotation = Quaternion.Euler(rot); //Se la asignamos al pivote

    }

    private void ActualizarDistancias() //Actualizamos el texto de la distancia
    {
        distancia.text = ((int)player.transform.position.z).ToString();
    }

    private void CheckColision() //Funci�n que se llama desde ColisionAvioneta.cs para indicar que el avi�n se ha chocado (Deprecado)
    {
        noChocado = false;
    }

    private void interludio() //Entramos en la fase 1.5
    {
        manoI = player.GetNamedChild("LeftHand Controller"); //Cogemos el mando izquierdo
        manoD = player.GetNamedChild("RightHand Controller"); //Cogemos el mando derecho


        manoI.SetActive(false);  //Desactivamos el mando izquierdo
        manoD.SetActive(false); //Desactivamos el mando derecho

        escena[0].gameObject.SetActive(true); //Activamos la mano con la animaci�n (tutorial)
        escena[1].gameObject.SetActive(true); //Activamos el avi�n con la animaci�n (tutorial)

    }

    private void TerminarFase2() //Terminamos la partida como tal
    {
        avion1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; //Detenemos el avi�n por completo, congelando las constraints del rigidbody
        avion1 .GetComponent<AircraftPhysics>().enabled = false; //Detenemos el script de las f�sicas

        //Cogemos la distancia recorrida
        int puntuacion1;
        puntuacion1 = int.Parse(distancia.text);

        if (!File.Exists(Application.dataPath + "/MaxScoreAvion.txt")) //Si no existe el txt lo crea
        {
            StreamWriter write = new StreamWriter(Application.dataPath + "/MaxScoreAvion.txt", false);
            write.WriteLine("0");
            write.Close();
        }
        //Leemos desde un txt la puntuaci�n m�xima mediante un StreamReader
        StreamReader read = new StreamReader(Application.dataPath + "/MaxScoreAvion.txt");
        int maxScore = int.Parse(read.ReadLine());
        read.Close();

        distancia.gameObject.SetActive(false); //Desactivamos el texto que muestra la distancia

        //Leemos la puntuaci�n y la ponemos en un texto
        puntos.text = "Puntuaci�n: " + puntuacion1.ToString();
        puntos.gameObject.SetActive(true);

        if(puntuacion1 > 0)//Si la puntuaci�n es positiva
        {
            if (puntuacion1 > maxScore) // y adem�s hemos hecho un nuevo record
            {
                audioSources[2].GetComponent<AudioSource>().Play(); //Activamos el sonido "Big Win"
                newRecord.gameObject.SetActive(true); //Activamos el texto que pone "nuevo record"

                //Sobreescribimos la puntuaci�n en el txt mediante un StreamWriter
                StreamWriter write = new StreamWriter(Application.dataPath + "/MaxScoreAvion.txt", false); 
                write.WriteLine(puntuacion1);
                write.Close();
            }
            else //Si no hemos hecho record
            {
                audioSources[0].GetComponent<AudioSource>().Play(); //Activamos el sonido "Win"
            }
        }
        else //Si la distancia recorrida es negativa (hemos ido hacia detr�s)
        {
            audioSources[1].GetComponent<AudioSource>().Play(); //Activamos el sonido "Lose"
        }
        

        final = true; //Pasamos al final
    }

    private void DesactivarTodo() //Desactivar todos los elementos de lal escena
    {
        foreach(GameObject g in escena)
        {
            g.SetActive(false);
        }
    }

    private void giroPicoDerecha(float f) //Giramos el pico con el gatillo derecho
    {
        Vector3 rot = avion1.GetNamedChild("PicoPivot").transform.localRotation.eulerAngles; //Recogemos la rotaci�n del pico
        float prevRot = rot.x; //guardamos la rotaci�n actual
        rot.x += f; //Modificamos la rotaci�n
        if(prevRot <= 90f && rot.x > 90f) //Si la rotaci�n anterior era menor a 90 y la nueva es mayor que 90
        {
            rot.x = 90f; //No le dejamos y la ponemos a 90, ya que si no el pico podr�a meterse dentro del avi�n y eso no es muy realista
        }
        avion1.GetNamedChild("PicoPivot").transform.localRotation = Quaternion.Euler(rot); //Aplicamos la rotaci�n
    }

    private void giroPicoIzquierda(float f) //Giramos el pico con el gatillo izquierda
    {
        Vector3 rot = avion1.GetNamedChild("PicoPivot").transform.localRotation.eulerAngles;//Recogemos la rotaci�n del pico
        float prevRot = rot.x; //guardamos la rotaci�n actual
        rot.x -= f; //Modificamos la rotaci�n
        if (prevRot >= 270f && rot.x < 270f) //Si la rotaci�n anterior era mayor que 270 y la nueva es menor que 270
        {
            rot.x = 270f; //No le dejamos y la ponemos a 270, ya que si no el pico podr�a meterse dentro del avi�n y eso no es muy realista
        }
        avion1.GetNamedChild("PicoPivot").transform.localRotation = Quaternion.Euler(rot); //Aplicamos la rotaci�n
    }

    public void CeroTiempo() //Poner el tiempo a 0 (se usa cuando agarramos el cubo)
    {
        restoTiempo = 0;
        cubo.gameObject.SetActive(false); //Desactivamos el cubo
    }

    private IEnumerator LoadYourAsyncScene() //Funci�n que carga la escena del men� (sacada de la documentaci�n de Unity)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
