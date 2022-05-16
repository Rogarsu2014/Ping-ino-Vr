using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;



public class DianaScript : MonoBehaviour
{

    // Start is called before the first frame update
    public GameManager gm;
    private AudioSource hitAudio;
    public PathCreator path;
    float speed = 3f;
    float distanceTravelled;

    private void Start()
    {
        hitAudio= gameObject.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {   
        gm.randomDianaPos(path.gameObject);
        collision.gameObject.transform.position = new Vector3(1000, 1000, 1000);
        gm.addPoints();
        hit();
        
    }

    public void hit()
    {
        hitAudio.Play();
    }

    private void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = path.path.GetPointAtDistance(distanceTravelled);
    }

}
