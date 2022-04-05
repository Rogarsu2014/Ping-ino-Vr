using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AgarrarAla : MonoBehaviour
{
    public MeshRenderer[] ms;
    //public Transform mano;
    private bool agarrado = false;
    private Transform padre;
    public string eje = "Z";
    private void Start()
    {
        ms = this.GetComponentsInChildren<MeshRenderer>();
        padre = this.transform.parent;
    }
    public void SetRed()
    {
        foreach(MeshRenderer r in ms)
        {
            r.material.color = Color.red;
        }
    }
    public void SetBlue()
    {
        foreach (MeshRenderer r in ms)
        {
            r.material.color = Color.blue;
        }
    }
    public void CambiarRotZ()
    {
        XRGrabInteractable i = this.GetComponent<XRGrabInteractable>();
        Transform j = i.GetOldestInteractorSelecting().transform;
        //usar euler angles
        Vector3 angles = j.rotation.eulerAngles;
        this.transform.rotation = Quaternion.Euler(0, 0, angles.z);
        //this.transform.rotation = mano.rotation;
    }
    public void CambiarRotX()
    {
        XRGrabInteractable i = this.GetComponent<XRGrabInteractable>();
        Transform j = i.GetOldestInteractorSelecting().transform;
        //usar euler angles
        Vector3 angles = j.rotation.eulerAngles;
        this.transform.rotation = Quaternion.Euler(angles.x, 0, 0);
        //this.transform.rotation = mano.rotation;
    }
    public void Update()
    {
        if(agarrado == true)
        {
            if (eje == "Z")
            {
                CambiarRotZ();
            }else if(eje == "X")
            {
                CambiarRotX();
            }
            
        }
    }

    public void cambiarAgarre()
    {
        if(agarrado == false){
            agarrado = true;
        }
        else
        {
            agarrado = false;
            this.transform.parent = padre;
        }
    }
}
