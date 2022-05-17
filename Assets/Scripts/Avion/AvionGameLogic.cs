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
    private bool sigueContando = false; //Comprobación de que seguimos en la fase 1 (construcción)
    private bool cambioDeFase = true; //Comprobación de que seguimos en la fase 1.5 (interludio)
    private bool fase2 = false; //Comprobación de que seguimos en la fase 2 (vuelo)
    
    public bool noChocado = true; //Comprobación de que el avión no se ha chocado durante la fase de vuelo

    public TextMeshProUGUI tiempo; //El texto que muestra el tiempo
    public Canvas textoTutorial; //Canvas donde se encuentra el texto de un tutorial
    public TextMeshProUGUI distancia; //Texto que muestra la distancia recorrida durante la fase de vuelo
    public TextMeshProUGUI newRecord; //Texto que aparece cuando se ha superado el record de distancia
    public TextMeshProUGUI puntos; //texto que muestra la puntuación al final del juego

    private GameObject[] aviones; //Array de aviones que de momento no esta en uso debido a la falta del multijugador
    
    public GameObject player; //El jugador (XR Origin)
    private GameObject avion1;//El avión 

    private GameObject manoI; //El mando izquierdo
    private GameObject manoD; //El mando derecho

    private GameObject ad1p; //El primer pivote del ala Derecha
    private GameObject ai1p; //El primer pivote del ala Izquierda

    public GameObject[] escena; //Diversos elementos de la escena agrupados para poder ser desactivados al entrar en la fase 2
    [SerializeField] InputActionReference triggerD; //Acción referente al gatillo derecho
    [SerializeField] InputActionReference triggerI; //Acción referente al gatillo izquierdo

    public GameObject cubo; //El cubo con el que pasamos automáticamente de la fase 1 a la fase 1.5

    private bool final = false; //El booleano que nos permite volver al menú al terminar la partida

    public GameObject[] audioSources; //Array de los audioSources de la escena con los sonidos que suena al saber que puntuación se ha conseguido.


    private void Start()
    {
        sigueContando = true;
        //Es posible que en multijugador haya que no empezar el contador al principio
        //Sino comprobar la sincornización de los jugadores antes de comenzarlo.
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

                //Hacemos una comprobación de si se ha pulsado algún gatillo para cambiar la orientación del pico
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

    public void MostrarTiempo(float tiempoResto) //Función que muestra el tiempo
    {
        tiempoResto += 1; //Le sumamos uno para que acabe la cuenta atrás en 0
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
        foreach(GameObject avion in aviones)//Por cada avión recogido
        {
            XRGrabInteractable[] interactables = avion.GetComponentsInChildren<XRGrabInteractable>();//cogemos todos los componentes XRGrabInteractable
            foreach(XRGrabInteractable grab in interactables)
            {
                grab.enabled = false;// y los desactivamos, para que en la fase 2 el jugador no los pueda agarrar
            }

            //Ahora mismo el código funciona bien porque hay un solo avión, se tendría que modificar para un posible multijugador

            avion.transform.position = new Vector3(30*i, 2000, 0);//Ponemos el avión alto
            avion.transform.rotation = Quaternion.Euler(0,180,0);//Rotamos el avión 180 grados 


            avion.GetComponent<Rigidbody>().useGravity = true; //Activamos la gravedad
            avion.GetComponent<AircraftPhysics>().enabled = true;//Activamos las físicas 


            avion1 = avion; //El avión 1 va a ser a partir de aqui con el que trabajaremos
            ad1p = avion1.GetNamedChild("AD1Pivot"); //Cogemos el pivote derecho
            ai1p = avion1.GetNamedChild("AI1Pivot"); //Cogemos el pivote izquierdo

            i += 1;
        }

        fase2 = true; //Activamos fase 2

        //Reactivamos las manos
        manoI.SetActive(true);
        manoD.SetActive(true);
    }

    private void ActualizarPosicionesJugadores() //Actualizar la posición del jugador y el avión
    {
        //Poner al jugador donde esta el avion (en la punta)
        Vector3 pos = avion1.GetNamedChild("PicoPivot").transform.position; //Pillamos la posición del pico
        pos.y -= 1f; //Bajamos un poco el jugador
        pos.z -= 0.5f; //Hacemos retroceder al jugador un poco
        player.transform.position = pos; //le asignamos esta posición al jugador

        //Cambiar la rotacion del ala

        Vector3 rot = ad1p.transform.localRotation.eulerAngles; //Cogemos la rotación del pivote derecho
        float manoDZ = manoD.transform.rotation.eulerAngles.z; //Cogemos la rotación del mando derecho
        rot = new Vector3(rot.x, rot.y, -manoDZ); //Hacemos una mezcla pero solo usando la rotación en Z del mando
        ad1p.transform.localRotation = Quaternion.Euler(rot); //Se la asignamos al pivote


        Vector3 rot2 = ai1p.transform.localRotation.eulerAngles; //Cogemos la rotación del pivote izquierdo
        float manoIZ = manoI.transform.rotation.eulerAngles.z; //Cogemos la rotación del mando izquierdo
        rot = new Vector3(rot.x, rot.y, -manoIZ); //Hacemos una mezcla pero solo usando la rotación en Z del mando
        ai1p.transform.localRotation = Quaternion.Euler(rot); //Se la asignamos al pivote

    }

    private void ActualizarDistancias() //Actualizamos el texto de la distancia
    {
        distancia.text = ((int)player.transform.position.z).ToString();
    }

    private void CheckColision() //Función que se llama desde ColisionAvioneta.cs para indicar que el avión se ha chocado (Deprecado)
    {
        noChocado = false;
    }

    private void interludio() //Entramos en la fase 1.5
    {
        manoI = player.GetNamedChild("LeftHand Controller"); //Cogemos el mando izquierdo
        manoD = player.GetNamedChild("RightHand Controller"); //Cogemos el mando derecho


        manoI.SetActive(false);  //Desactivamos el mando izquierdo
        manoD.SetActive(false); //Desactivamos el mando derecho

        escena[0].gameObject.SetActive(true); //Activamos la mano con la animación (tutorial)
        escena[1].gameObject.SetActive(true); //Activamos el avión con la animación (tutorial)

    }

    private void TerminarFase2() //Terminamos la partida como tal
    {
        avion1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; //Detenemos el avión por completo, congelando las constraints del rigidbody
        avion1 .GetComponent<AircraftPhysics>().enabled = false; //Detenemos el script de las físicas

        //Cogemos la distancia recorrida
        int puntuacion1;
        puntuacion1 = int.Parse(distancia.text);

        if (!File.Exists(Application.dataPath + "/MaxScoreAvion.txt")) //Si no existe el txt lo crea
        {
            StreamWriter write = new StreamWriter(Application.dataPath + "/MaxScoreAvion.txt", false);
            write.WriteLine("0");
            write.Close();
        }
        //Leemos desde un txt la puntuación máxima mediante un StreamReader
        StreamReader read = new StreamReader(Application.dataPath + "/MaxScoreAvion.txt");
        int maxScore = int.Parse(read.ReadLine());
        read.Close();

        distancia.gameObject.SetActive(false); //Desactivamos el texto que muestra la distancia

        //Leemos la puntuación y la ponemos en un texto
        puntos.text = "Puntuación: " + puntuacion1.ToString();
        puntos.gameObject.SetActive(true);

        if(puntuacion1 > 0)//Si la puntuación es positiva
        {
            if (puntuacion1 > maxScore) // y además hemos hecho un nuevo record
            {
                audioSources[2].GetComponent<AudioSource>().Play(); //Activamos el sonido "Big Win"
                newRecord.gameObject.SetActive(true); //Activamos el texto que pone "nuevo record"

                //Sobreescribimos la puntuación en el txt mediante un StreamWriter
                StreamWriter write = new StreamWriter(Application.dataPath + "/MaxScoreAvion.txt", false); 
                write.WriteLine(puntuacion1);
                write.Close();
            }
            else //Si no hemos hecho record
            {
                audioSources[0].GetComponent<AudioSource>().Play(); //Activamos el sonido "Win"
            }
        }
        else //Si la distancia recorrida es negativa (hemos ido hacia detrás)
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
        Vector3 rot = avion1.GetNamedChild("PicoPivot").transform.localRotation.eulerAngles; //Recogemos la rotación del pico
        float prevRot = rot.x; //guardamos la rotación actual
        rot.x += f; //Modificamos la rotación
        if(prevRot <= 90f && rot.x > 90f) //Si la rotación anterior era menor a 90 y la nueva es mayor que 90
        {
            rot.x = 90f; //No le dejamos y la ponemos a 90, ya que si no el pico podría meterse dentro del avión y eso no es muy realista
        }
        avion1.GetNamedChild("PicoPivot").transform.localRotation = Quaternion.Euler(rot); //Aplicamos la rotación
    }

    private void giroPicoIzquierda(float f) //Giramos el pico con el gatillo izquierda
    {
        Vector3 rot = avion1.GetNamedChild("PicoPivot").transform.localRotation.eulerAngles;//Recogemos la rotación del pico
        float prevRot = rot.x; //guardamos la rotación actual
        rot.x -= f; //Modificamos la rotación
        if (prevRot >= 270f && rot.x < 270f) //Si la rotación anterior era mayor que 270 y la nueva es menor que 270
        {
            rot.x = 270f; //No le dejamos y la ponemos a 270, ya que si no el pico podría meterse dentro del avión y eso no es muy realista
        }
        avion1.GetNamedChild("PicoPivot").transform.localRotation = Quaternion.Euler(rot); //Aplicamos la rotación
    }

    public void CeroTiempo() //Poner el tiempo a 0 (se usa cuando agarramos el cubo)
    {
        restoTiempo = 0;
        cubo.gameObject.SetActive(false); //Desactivamos el cubo
    }

    private IEnumerator LoadYourAsyncScene() //Función que carga la escena del menú (sacada de la documentación de Unity)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
