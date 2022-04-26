using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMap : MonoBehaviour
{
    Vector2 movement;

    float speed = .1f;

    public Rigidbody bala;
    public Transform pistola;
    public Transform camara;
    public Transform canonPosition;
    bool grabbed;
    public void OnActivate()
    {

        if (grabbed)
        {
            //DISPARA  

            Rigidbody clone;
            clone = Instantiate(bala, canonPosition.position, pistola.rotation);


            clone.velocity = pistola.TransformDirection(Vector3.forward * 50);
        }
    }

    public void OnMove(InputValue input)
    {
        movement = input.Get<Vector2>();
    }
    
    void FixedUpdate()
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

}
