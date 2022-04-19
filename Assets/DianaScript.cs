using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DianaScript : MonoBehaviour
{

    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
        Destroy(collision.gameObject);
    }

    private void OnDestroy()
    {

        print("adios");
    }
}
