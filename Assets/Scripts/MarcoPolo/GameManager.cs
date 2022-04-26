using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
 
public class GameManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> dianas;
    private int points;
    public TextMeshProUGUI textMesh;
    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        textMesh.text = "0";
        foreach (var diana in dianas)
        {
            randomDianaPos(diana);
        }
        
    }

    public void randomDianaPos(GameObject d)
    {
        Vector3 pos = new Vector3(Random.Range(-30f, 30f), 4, Random.Range(-30f, 30f));
        d.transform.position = pos;
    }


    public void addPoints()
    {
        points += 1;
        print(points);
        print(points.ToString());
        textMesh.text = points.ToString();
    }

    private void FixedUpdate()
    {
        foreach(var d in dianas)
        {
            lookAtPlayer(d, player.transform);
        }
    }

    private void lookAtPlayer(GameObject diana, Transform t)
    {
        Quaternion _lookRotation = Quaternion.LookRotation((t.position - diana.transform.position).normalized);
        diana.transform.rotation = _lookRotation;
    }
}
