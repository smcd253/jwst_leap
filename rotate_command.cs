using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate_command : MonoBehaviour {

    public Transform body;
    public string direction;
    public float speed;

    private void Start()
    {
        Debug.Log("rotate_command enabled");
    }
    public void set_direction (string input)
    {
        direction = input;
    }
    public void set_speed (float input)
    {
        speed = input;
    }
    void Update ()
    {
        float omega = Time.deltaTime * speed;

        if (direction == "cw")
        {
            body.Rotate(Vector3.up * omega);
            Debug.Log("rotate cw");
        }
        else if (direction == "ccw")
        {
            body.Rotate(Vector3.down * omega);

            Debug.Log("rotate ccw");
        }
        else
        {
            Debug.Log("no direction input");
        }
    }
}

