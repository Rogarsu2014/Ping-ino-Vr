using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{

    double timeInstantiated;
    public float assignedTime;
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
            Destroy(gameObject);
            //Destroy(this);
        }
        else
        {
            Vector3 aux = new Vector3(Vector3.Lerp(Vector3.left*SongManager.Instance.noteDespawnY,Vector3.left*SongManager.Instance.noteSpawnY, t).x, transform.position.y, transform.position.z);
            transform.position = aux;
        }
    }
}
