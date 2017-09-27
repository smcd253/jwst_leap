using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oscillate_y : MonoBehaviour {
    public float osc_speed;
    public float osc_delta;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 osc = startPos;
        osc.y += osc_delta * Mathf.Sin(Time.time * osc_speed);
        transform.position = osc;
    }
}
