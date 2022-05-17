using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
 
/* Esta clase se encarga de mantener el rail de la escopeta en el rango
 * en el que se deber�a mover en la realidad y a la vez calcula si recarga
 */
public class ClampPosition : MonoBehaviour
{
    private bool agarrado = false;
    private Vector3 posInicial;
    private Quaternion rotInicial;
    public float maximoZ;
    public float minimoZ;
    private float posX;
    private float posY;
    private bool topeMax=false;
    private bool topeMin=false;

    private void Awake()
    {
        posInicial = transform.localPosition;
        rotInicial = transform.localRotation;
    }

    private void Update()
    {
        if (agarrado)
        {
            //Si esta agarrado lo transofrma en funci�n del movimiento de la mano que agarra
            XRGrabInteractable i = this.GetComponent<XRGrabInteractable>();
            Transform j = i.GetOldestInteractorSelecting().transform;

            Vector3 pos = j.position;
            float posicion = pos.z - transform.localPosition.z;

            //Si est� en un tope no se transforma
            if ((topeMax&&posicion>0)||(topeMin&&posicion<0))
            {
                transform.Translate(new Vector3(0, 0, 0), Space.Self);
            }
            //Sino transforma �nicamente la posici�n en Z local, hacia delante y detras.
            else
            {
                transform.Translate(new Vector3(0, 0, posicion), Space.Self);

            }

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
        transform.localPosition = posInicial;
        transform.localRotation = rotInicial;
        agarrado = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tope") print("topemax"); topeMax = true;
        if (collision.gameObject.name == "TopeMin") print("topeMin"); topeMin = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        topeMax = false;
        topeMin = false;
    }
}
