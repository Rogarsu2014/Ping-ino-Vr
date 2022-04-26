using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DianaScript : MonoBehaviour
{

    // Start is called before the first frame update
    public GameManager gm;
    public AudioSource hitAudio;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        gm.randomDianaPos(this.gameObject);
        gm.addPoints();
        hit();
        
    }

    public void hit()
    {
        hitAudio.Play();
    }

    private void OnDestroy()
    {

        print("adios");
    }

}
