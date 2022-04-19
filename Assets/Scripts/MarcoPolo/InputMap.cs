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


            clone.velocity = pistola.TransformDirection(Vector3.forward * 30);
        }
    }

    public void OnMove(InputValue input)
    {
        movement = input.Get<Vector2>();
    }
    
    void FixedUpdate()
    {
        var playerTransform = transform;
        playerTransform.Translate(playerTransform.right  * movement.x * speed, Space.World);
        playerTransform.Translate(playerTransform.forward * movement.y * speed,Space.World);
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
