using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCycle : MonoBehaviour
{
    float a = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Make the light rotate around the y axis.
        a += 0.1f;
        transform.rotation = Quaternion.Euler(new Vector3(60, a, 0));

    }
}
