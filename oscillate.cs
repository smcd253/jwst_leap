using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oscillate : MonoBehaviour {
    public float osc_speed;
    public float osc_delta;
    private Vector3 startPos;

    public bool x = true;
    public bool y = true;
    public bool z = true;

    private void Start()
    {
        startPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 osc = startPos;
        if (x)
        {
            osc.x += osc_delta * Mathf.Sin(Time.time * osc_speed);
        }
        if (y)
        {
            osc.y += osc_delta * Mathf.Sin(Time.time * osc_speed);
        }
        if (z)
        {
            osc.z += osc_delta * Mathf.Sin(Time.time * osc_speed);
        }
        transform.position = osc;
    }
}
