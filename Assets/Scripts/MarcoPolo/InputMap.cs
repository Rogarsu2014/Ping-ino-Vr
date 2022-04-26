using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMap : MonoBehaviour
{
    Vector2 movement;

    float speed = .05f;

    public Rigidbody bala;
    public Rigidbody cartucho;
    private Transform pistola;
    public Transform camara;
    private Transform canonPosition;
    bool grabbed;
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

    public void disparo()
    {
        if(pistola.name == "Shotgun")
        {
            Rigidbody clone;
            float maxSpread = 0.1f;
            for (int i = 0; i < 7; i++)
            {
                Vector3 dir = transform.forward + new Vector3(Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread));
                clone = Instantiate(cartucho, canonPosition.position, pistola.rotation);
                clone.velocity = pistola.TransformDirection(dir* 30);

                //clone.GetComponent<Rigidbody>().AddForce(dir * 500);
            }
            

        }
        else if(pistola.name == "Revolver")
        {
            Rigidbody clone;
            clone = Instantiate(bala, canonPosition.position, pistola.rotation);
            clone.velocity = pistola.TransformDirection(Vector3.forward * 50);
        }
    }
}
