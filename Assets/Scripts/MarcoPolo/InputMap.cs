using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMap : MonoBehaviour
{
    Vector2 movement;

    float speed = .05f;

    public List <Rigidbody> bala;
    private int disparos = 0;
    public List <Rigidbody> cartucho;
    private Transform pistola;
    public Transform camara;
    private Transform canonPosition;
    bool grabbed;
    bool readyToShoot=true;
    public void OnActivate()
    {

        if (grabbed)
        {
            //DISPARA  
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

    public void OnMove(InputValue input)
    {
        movement = input.Get<Vector2>();
    }
    
    void Update()
    {
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

        if(pistola.name == "Shotgun" && readyToShoot)
        {
            pistola.GetComponentInParent<AudioSource>().Play();
            float maxSpread = 0.1f;
            foreach (Rigidbody clone in cartucho)
            {
                Vector3 dir = transform.forward + new Vector3(Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread));
                //clone = Instantiate(cartucho, canonPosition.position, pistola.rotation
                clone.MovePosition(canonPosition.position);
                clone.rotation = pistola.rotation;
                clone.velocity = pistola.TransformDirection(dir* 30);

                //clone.GetComponent<Rigidbody>().AddForce(dir * 500);
                readyToShoot=false;
            }
            

        }
        else if (pistola.name == "Sniper")
        {
            pistola.GetComponentInParent<AudioSource>().Play();
            Rigidbody clone = bala[disparos];
            clone.MovePosition(canonPosition.position);
            clone.rotation = pistola.rotation;
            clone.velocity = pistola.TransformDirection(Vector3.forward * 300);
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
