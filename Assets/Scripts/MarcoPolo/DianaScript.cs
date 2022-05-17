using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

/* Esta clase se encuentra en el prefab de las dianas, y determinan su comportamiento */

public class DianaScript : MonoBehaviour
{

    public GameManager gm;
    private AudioSource hitAudio;
    public PathCreator pathing;
    float speed = 3f;
    float distanceTravelled;

    private void Start()
    {
        hitAudio= gameObject.GetComponent<AudioSource>();
    }

    /* Cuando se encuentra una colisión, la diana se desplaza a otro lugar aleatorio, 
     añade puntos y ejecuta el audio de la explosión.*/
    private void OnCollisionEnter(Collision collision)
    {   
        gm.randomDianaPos(pathing.gameObject);
        collision.gameObject.transform.position = new Vector3(1000, 1000, 1000);
        gm.addPoints();
        hit();
        
    }

    public void hit()
    {
        hitAudio.Play();
    }

    /* En Update se calcula el movimiento a través del asset PathCreator. */
    private void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathing.path.GetPointAtDistance(distanceTravelled);
    }

}
