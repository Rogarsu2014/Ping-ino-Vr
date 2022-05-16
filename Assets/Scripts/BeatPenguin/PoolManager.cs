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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

