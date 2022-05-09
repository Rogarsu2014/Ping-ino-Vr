using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinCarril : MonoBehaviour
{
    [SerializeField] public UnityEvent _recargar;

    private void OnCollisionEnter(Collision collision)
    {
        print("ey");
        _recargar.Invoke();
    }

}
