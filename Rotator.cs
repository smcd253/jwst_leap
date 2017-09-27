using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    public float speed = 1;
    public Vector3 axis;
    public bool rotateLocal = true;
    public bool orbit = false;
    public float orbitSpeed = 1f;
    public Transform orbitPoint;
    public Vector3 orbitAxis;

    void Update()
    {
        if (rotateLocal) transform.Rotate(new Vector3(axis.x, axis.y, axis.z) * Time.deltaTime * speed);
        if (orbit)
        {
            transform.RotateAround(orbitPoint.position, orbitAxis, Time.deltaTime * orbitSpeed);
        }
    }
}