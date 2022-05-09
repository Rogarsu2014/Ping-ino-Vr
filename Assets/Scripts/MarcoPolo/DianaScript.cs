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
        gm.randomDianaPos(this.gameObject);
        collision.gameObject.transform.position = new Vector3(1000, 1000, 1000);
        gm.addPoints();
        hit();
        
    }

    public void hit()
    {
        hitAudio.Play();
    }

    private void OnDestroy()
    {
    }

}
