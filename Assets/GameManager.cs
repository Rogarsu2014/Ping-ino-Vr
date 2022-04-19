using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> dianas;
    public 

    // Start is called before the first frame update
    void Start()
    {

        foreach (var diana in dianas)
        {
            Vector3 pos = new Vector3(Random.Range(-30f, 30f), 4, Random.Range(-30f, 30f));
            diana.transform.position = pos;
        }
        
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        
    }
}
