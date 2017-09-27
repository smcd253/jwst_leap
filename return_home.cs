using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class return_home : MonoBehaviour
{
    protected Vector3 homePos;
    protected Quaternion homeRot;
    protected Vector3 homeScale;
    public float speed_home = 0.5f;

    void Start()
    {
        // grab original position & store
        homePos = transform.localPosition;
        homeRot = transform.localRotation;
        homeScale = transform.localScale;

    }

    public void ret_home() // restore original position
    {
        //transform.localPosition = homePos;
        //transform.localRotation = homeRot;
        //transform.localScale = homeScale;
        //Debug.Log("ret_home()");
        if (this.gameObject.active == true)
        {
            StopAllCoroutines();
            StartCoroutine(grad_ret_home());
        }
    }

    IEnumerator grad_ret_home()
    {
        Vector3 startPos = this.transform.localPosition;
        Quaternion startRot = this.transform.localRotation;
        float linear = 0f;
        float t;

        while (
            transform.localPosition != homePos ||
            transform.localRotation != homeRot ||
            transform.localScale != homeScale
            )
        {
            linear += Time.deltaTime * this.speed_home;
            t = Mathf.SmoothStep(0f, 1f, linear);

            //transform.localPosition = Vector3.Lerp(transform.localPosition, homePos, Time.deltaTime * speed_home);
            transform.localPosition = Vector3.Lerp(startPos, this.homePos, t);

            transform.localScale = Vector3.Lerp(transform.localScale, homeScale, t);

            //transform.localRotation = Quaternion.Lerp(transform.localRotation, homeRot, Time.deltaTime * speed_home);
            transform.localRotation = Quaternion.Lerp(startRot, this.homeRot, t);

            yield return null;
        }
    }
}
