using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giro : MonoBehaviour
{

    // Random rnd = new Random();

    Vector3 rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // double a = rnd.NextDouble();
        
        // if (a < 0.5){
        //     rotSpeed = new Vector3(0.0f, 0.5f, 0.0f);
        // } else {
            rotSpeed = new Vector3(0.0f, -0.5f, 0.0f);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Transform>().rotation = GetComponent<Transform>().rotation + rotSpeed;
    }

    public void actualizaGiro()
    {
        rotSpeed = -rotSpeed;

        rotSpeed = rotSpeed * 1.5;
    }
}
