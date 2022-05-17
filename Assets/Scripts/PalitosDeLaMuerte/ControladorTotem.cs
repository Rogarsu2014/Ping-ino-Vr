using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorTotem : MonoBehaviour
{
    int[] velocidades = new int[] {50, 64, 78, 92, 106, 120};

    int rotSpeed;

    public AudioSource sonidoGolpe;

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
        activaSonido();

        int nuevaVelocidad = velocidades[(int)Random.Range(0.0f, 5.0f)];

        if(rotSpeed > 0) {

            nuevaVelocidad = -nuevaVelocidad;
            rotSpeed = nuevaVelocidad;

        } else {

            rotSpeed = nuevaVelocidad;

        }
    }

    void activaSonido()
    {
        sonidoGolpe.Play();
    }
}