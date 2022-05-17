using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* Esta clase se encuentra en el gameobject que determina si ha recargado la escopeta,
 * cuando entra en colisi�n con algo llama a un evento de unity que ejecuta la funci�n de recargar.*/
public class FinCarril : MonoBehaviour
{
    [SerializeField] public UnityEvent _recargar;

    private void OnCollisionEnter(Collision collision)
    {
        _recargar.Invoke();
    }

}
