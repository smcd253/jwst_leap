using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outline_width_mod : MonoBehaviour {
    public float osc_speed;
    public float osc_delta;
    private Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 osc = startScale;
        osc.x += osc_delta * Mathf.Sin(Time.time * osc_speed);
        osc.z += osc_delta * Mathf.Sin(Time.time * osc_speed);
        transform.localScale = osc;
    }
}
