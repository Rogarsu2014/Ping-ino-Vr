using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giro : MonoBehaviour
{
    float rotSpeed = 35.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }

    // Cambia el sentido del giro
    public void actualizaGiro()
    {
        if (rotSpeed < 30.0)
        {
            rotSpeed = -rotSpeed * (float)1.05;
        }
        else
        {
            rotSpeed = -rotSpeed;
        }
    }
}
