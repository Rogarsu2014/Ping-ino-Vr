using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giro : MonoBehaviour
{
    int[] velocidades = new int[] {30, 44, 58, 72, 86, 100};

    int rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = velocidades[(int)Random.Range(0.0f, 5.0f)];
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }

    // Cambia el sentido del giro
    public void actualizaGiro()
    {
        int nuevaVelocidad = velocidades[(int)Random.Range(0.0f, 5.0f)];

        if(rotSpeed > 0) {

            nuevaVelocidad = -nuevaVelocidad;
            rotSpeed = nuevaVelocidad;

        } else {

            rotSpeed = nuevaVelocidad;

        }
    }
}


// public void actualizaGiro()
//     {
//         if (Mathf.Abs(rotSpeed) < 100.0)
//         {
//             rotSpeed = -rotSpeed * (float)1.2;
//             //Debug.Log(rotSpeed);
//         }
//         else
//         {
//             rotSpeed = -rotSpeed;
//             //Debug.Log(rotSpeed);
//         }
//     }
