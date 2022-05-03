using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampPosition : MonoBehaviour
{
    private void Update()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x,0f, 0f);
        pos.y= Mathf.Clamp(transform.position.x, 0f, 0f);

        transform.position = pos;
    }
}
