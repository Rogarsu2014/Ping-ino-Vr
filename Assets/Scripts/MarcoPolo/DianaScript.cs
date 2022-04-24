using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DianaScript : MonoBehaviour
{

    // Start is called before the first frame update
    public GameManager gm;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        Destroy(this.gameObject);
        gm.addPoints();
        
    }

    private void OnDestroy()
    {

        print("adios");
    }
}
