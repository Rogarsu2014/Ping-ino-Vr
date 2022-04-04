using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AgarrarAla : MonoBehaviour
{
    public MeshRenderer[] ms;
    public Transform mano;
    private void Start()
    {
        ms = this.GetComponentsInChildren<MeshRenderer>();
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
    public void CambiarRot()
    {
        XRGrabInteractable i = this.GetComponent<XRGrabInteractable>();
        Transform j = i.selectingInteractor.transform;
        this.transform.rotation = this.transform.rotation * j.rotation;
        //this.transform.rotation = mano.rotation;
    }
}
