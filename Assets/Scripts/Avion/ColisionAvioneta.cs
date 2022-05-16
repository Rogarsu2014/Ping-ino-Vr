using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionAvioneta : MonoBehaviour
{
    public GameObject gameManager; //Guardamos el game Manager de la escena

    private bool mensajeMandado = false; //Comprobación de que el mensaje no se mande mas de una vez

    private void OnCollisionEnter(Collision collision) //cuando el objeto (el avion) colisiona
    {
        if (!mensajeMandado)//Si no se ha mandado el mensaje
        {
            if (collision.gameObject.name == "Terrain" || collision.gameObject.name == "Iceberg") //Y ha chocado específicamente con el terreno o con el iceberg
            {
                gameManager.SendMessage("CheckColision"); //Llama al método CheckColision
            }
        }
    }
}
