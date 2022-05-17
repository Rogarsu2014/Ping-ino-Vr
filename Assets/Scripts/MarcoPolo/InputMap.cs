using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/*
  * Esta clase se implementa dentro del XROrigin de Marco Polo y controla el movimiento del jugador,
  * los disparos y el arma que sujeta. 
 */
public class InputMap : MonoBehaviour
{
    //Se define la variable donde se guarda el movimienot y la velocidad 
    Vector2 movement;
    float speed = .05f;
    //Un conteo de los disparos, ayuda en el pull
    private int disparos = 0;

    //Listas de balas para realizar un pull
    public List <Rigidbody> bala;
    public List <Rigidbody> cartucho;

    //Transform de los GameObjects relativos al arma, la cámara y al cañon(diferente en cada arma)
    private Transform pistola;
    public Transform camara;
    private Transform canonPosition;


    bool grabbed;
    bool readyToShoot=true;

    public void OnActivate()
    {

        if (grabbed)
        {
            //Dispara si está agarrado
            disparo();
            
        }
    }

    public void setPistola(Transform t)
    {
        pistola = t;
    }

    public void setCanon(Transform t)
    {
        canonPosition = t;
    }
    
    //Movimiento con el Input Manager de unity
    public void OnMove(InputValue input)
    {
        movement = input.Get<Vector2>();
    }
    
    void Update()
    {
        //Se calcula el movimiento del jugador en funcion de la camara, limitando el eje y.
        var playerTransform = transform;
        playerTransform.Translate(Vector3.Scale(camara.right,new Vector3(1,0,1)) * movement.x * speed,Space.World);
        playerTransform.Translate(Vector3.Scale(camara.forward, new Vector3(1, 0, 1)) * movement.y * speed,Space.World);
    }
    
    public void onGrab()
    {
        grabbed = true;
    }

    public void onDeGrab()
    {
        grabbed = false;
    }

    public void ReadyToShoot()
    {
        readyToShoot = true;
    }

    public void disparo()
    {
        /*
         * Se comprueba el nombre del arma que se tiene agarrado para saber el comportamiento del disparo, 
         * en el caso de la escopeta, se comprueba si ha recargado antes de disparar.
         */
        if(pistola.name == "Shotgun" && readyToShoot)
        {

            pistola.GetComponentInParent<AudioSource>().Play();
            float maxSpread = 0.1f;
            /* Se calcula una dirección aleatoria dentro de un rango para cada bala
             * para simular la dispersión de la escopeta.
             */
            foreach (Rigidbody clone in cartucho)
            {
               
                Vector3 dir = transform.forward + new Vector3(Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread));
                
                clone.MovePosition(canonPosition.position);
                clone.rotation = pistola.rotation;
                clone.velocity = pistola.TransformDirection(dir* 30);

                readyToShoot=false;
            }
            

        }
        else if (pistola.name == "Sniper")
        {
            //En este caso solo se calcula una trayectoria y se lanza a gran velocidad
            pistola.GetComponentInParent<AudioSource>().Play();
            
            //Se realiza un pull de las balas
            Rigidbody clone = bala[disparos];
            clone.MovePosition(canonPosition.position);
            clone.rotation = pistola.rotation;
            clone.velocity = pistola.TransformDirection(Vector3.forward * 300);

            //Se realiza un conteo de los disparos para cuando se supere un número máximo vuelve a tomar la primera bala
            disparos++;
            if (disparos >= bala.Count)
            {
                disparos = 0;
            }
        }
        else if(pistola.name == "Revolver")
        {
            pistola.GetComponentInParent<AudioSource>().Play();
            Rigidbody clone = bala[disparos];
            clone.MovePosition(canonPosition.position);
            clone.rotation = pistola.rotation;
            clone.velocity = pistola.TransformDirection(Vector3.forward * 30);
            disparos++;
            if(disparos >= bala.Count)
            {
                disparos = 0;
            }
        }
    }
}
