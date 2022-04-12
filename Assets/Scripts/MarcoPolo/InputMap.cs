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
    public void OnActivate()
    {

        Rigidbody clone;
        clone = Instantiate(bala, pistola.position, pistola.rotation);

        // Give the cloned object an initial velocity along the current
        // object's Z axis

        clone.velocity = pistola.TransformDirection(Vector3.forward * 30);
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


}
