using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClampPosition : MonoBehaviour
{
    private bool agarrado = false;
    private float posY;
    private float posX;

    private void Update()
    {
        if (agarrado)
        {
            XRGrabInteractable i = this.GetComponent<XRGrabInteractable>();
            Transform j = i.GetOldestInteractorSelecting().transform;

            Vector3 pos = j.localPosition;
            var posicionCalculada = pos.z - transform.localPosition.z;
            this.transform.Translate(new Vector3(0, 0, posicionCalculada), Space.Self);


        }
    }
    public void recargarRail()
    {
        posX = transform.localPosition.x;
        posY = transform.localPosition.y;
        agarrado = true;
    }
    public void soltarRail()
    {
        agarrado = false;
    }
}
