using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgarrarAla : MonoBehaviour
{
    public MeshRenderer[] ms;
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

}
