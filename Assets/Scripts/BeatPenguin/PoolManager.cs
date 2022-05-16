using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] objectPool;

    public GameObject SelectRandom()
    {
        int rand = Random.Range(0, objectPool.Length);
        return objectPool[rand];
    }
    

    // Start is called before the first frame update
    void Start()
    {
        PruebaMov();
        
    }

    void PruebaMov(){
        objectPool[0].transform.position = new Vector3(20.0f, 11.12f,0.39f);
        
    }

    // Update is called once per frame
    void Update()
    {

        objectPool[0].transform.position = new Vector3(objectPool[0].transform.position.x-Time.deltaTime, objectPool[0].transform.position.y, objectPool[0].transform.position.z);
        
    }
}

