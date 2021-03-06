using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{

    double timeInstantiated;
    public float assignedTime;
    float spdRot = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));
        if (t > 1)
        {
            Miss();
            Destroy(gameObject);
            //Destroy(this);
        }
        else
        {
            Vector3 aux = new Vector3(Vector3.Lerp(Vector3.left*SongManager.Instance.noteDespawnY,Vector3.left*SongManager.Instance.noteSpawnY, t).x, transform.position.y, transform.position.z);
            transform.position = aux;
            transform.Rotate(spdRot*Time.deltaTime,spdRot*Time.deltaTime, spdRot*Time.deltaTime);
        }
    }

    public void Hit(){
        ScoreManager.Hit();
        Destroy(gameObject);

    }
    public void Miss(){
        ScoreManager.Miss();

    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "LeftHand Controller" || collision.gameObject.name == "RightHand Controller")
        {
            Hit();
        }

    }

}
