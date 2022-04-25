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
        gm.randomDianaPos(this.gameObject);
        gm.addPoints();
        
    }



    private void Awake()
    {
        transform.rotation= new Quaternion(Random.Range(0,180), Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180)); 
    }

    private void OnDestroy()
    {

        print("adios");
    }
}
