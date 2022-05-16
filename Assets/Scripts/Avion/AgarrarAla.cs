using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AgarrarAla : MonoBehaviour
{
    public MeshRenderer[] ms; //Un array de MeshRenderers, para cambiar el color

    private bool agarrado = false; //Comprobación de si el objeto está agarrado
    private Transform padre; //Una variable para guardar el transform del padre del objeto, ya que cuando agarras un objeto con XRGrabInteractable se sale de la jerarquía y hay que meterlo de nuevo
    public string eje = "Z"; //Un valor público (para que se pueda cambiar) que indica el eje en el que se realiza la rotación
    private void Start()
    {
        ms = this.GetComponentsInChildren<MeshRenderer>(); //Al principio recoge todo los meshRenderers de los hijos
        padre = this.transform.parent; //Guardamos el padre
    }
    public void SetRed() //El nombre del método es antiguo, ya que ahora cambia el color a azul
    {
        foreach(MeshRenderer r in ms)//El color de todos los hijos
        {
            r.material.color = Color.blue;
        }
    }
    public void SetBlue() //El nombre del método es antiguo, ya que ahora cambia el color a blanco
    {
        foreach (MeshRenderer r in ms)//El color de todos los hijos
        {
            r.material.color = Color.white;
        }
    }
    public void CambiarRotZ() //Cambia la rotación en el eje Z
    {
        XRGrabInteractable i = this.GetComponent<XRGrabInteractable>(); //Obtenemos el XRGrabInteractable
        Transform j = i.GetOldestInteractorSelecting().transform; //Coge el transform del objeto que esta interaccionando

        Vector3 angles = j.rotation.eulerAngles;
        this.transform.rotation = Quaternion.Euler(0, 0, angles.z); //Asigna al objeto la rotación en z del mando

    }
    public void CambiarRotX() //Cambia la rotación en el eje X
    {
        XRGrabInteractable i = this.GetComponent<XRGrabInteractable>(); //Obtenemos el XRGrabInteractable
        Transform j = i.GetOldestInteractorSelecting().transform; //Coge el transform del objeto que esta interaccionando

        Vector3 angles = j.rotation.eulerAngles;
        this.transform.rotation = Quaternion.Euler(angles.x, 0, 0); //Asigna al objeto la rotación en x del mando

    }
    public void Update()
    {
        if(agarrado == true) //Si el objeto esta agarrado
        {
            if (eje == "Z") //y tiene Z en el eje
            {
                CambiarRotZ(); //Cambia la rotación en Z
            }
            else if(eje == "X") //Si tiene el eje X
            {
                CambiarRotX(); //Cambia la rotación en X
            }
            
        }
    }

    public void cambiarAgarre() //Cambia el estado  del objeto
    {
        if(agarrado == false){ //Si no esta agarrado
            agarrado = true; //Agarralo
        }
        else
        {
            agarrado = false; //Si estaba agarrado lo suelta
            this.transform.parent = padre; //Le vuelve a asignar el transform del padre
        }
    }
}
