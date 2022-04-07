using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMap : MonoBehaviour
{
    Vector2 movement;
    float rotSpeed = 5f;
    float speed = .2f;
    public void OnActivate()
    {
        print("Bang");
    }

    public void OnMove()
    {
        print("memuevo");
    }

    void FixedUpdate()
    {
        var playerTransform = transform;
        playerTransform.Rotate(Vector3.up * movement.x * rotSpeed);
        playerTransform.Translate(Vector3.forward * movement.y * speed);
    }


}
