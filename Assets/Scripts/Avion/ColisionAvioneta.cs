using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionAvioneta : MonoBehaviour
{
    public GameObject gameManager;

    private bool mensajeMandado = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!mensajeMandado)
        {
            if (collision.gameObject.name == "Terrain" || collision.gameObject.name == "Iceberg")
            {
                gameManager.SendMessage("CheckColision"); 
            }
        }
    }
}
