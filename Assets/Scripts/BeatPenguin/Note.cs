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
        float t = (float)(timeInstantiated - timeSinceInstantiated / (SongManager.Instance.noteTime* 2));
        if (t > 1)
        {
            gameObject.transform.position = new Vector3(0, 42.5f, 0);
        }
        else
        {
            Debug.Log("Matame");
        }
    }
}
