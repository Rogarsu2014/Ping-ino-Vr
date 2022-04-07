using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HoverColor : MonoBehaviour
{
    Material material;

    Color c;


    private void Awake()
    {
      c = GetComponent<MeshRenderer>().material.color;
    }
    public void iniciaHover()
    {
       GetComponent<MeshRenderer>().material.color = Color.red;

    }

    public void finalizaHover()
    {
        GetComponent<MeshRenderer>().material.color = c;

    }
}
