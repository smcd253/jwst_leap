using Leap.Unity.Attributes;
using Leap.Unity.Query;
using Leap.Unity.Interaction.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Leap.Unity.Space;
using InteractionEngineUtility;
using Leap.Unity.RuntimeGizmos;

namespace Leap.Unity.Interaction
{
    public class home_to_dest : MonoBehaviour
    {
        // home base variables
        protected Vector3 homePos;
        protected Quaternion homeRot;
        protected Vector3 homeScale;

        // public variables
        public Transform destParent;
        public Transform homeParent;
        public Transform destSibling;
        public float speed = 0.5f;
        public InteractionBehaviour interactionBehaviour;
        public int maxLoopCount = 300;

        // destination variables
        protected Vector3 destPos;
        protected Quaternion destRot;
        protected Vector3 destScale;

        void Start()
        {
            // grab original local position & store
            homePos = this.transform.localPosition;
            homeRot = this.transform.localRotation;
            homeScale = this.transform.localScale;

            // grab dest local stuff
            destPos = destSibling.localPosition;
            destRot = destSibling.localRotation;
            destScale = destSibling.localScale;
        }

        private void Update()
        {
            // if object is grabbed while at destination, turn off the parent rotator
            if (this.transform.parent.parent.parent.name == "start_menu" && interactionBehaviour.isGrasped)
            {
                this.transform.parent.gameObject.GetComponent<Rotator>().enabled = false;
            }
        }

        public void ret_home() // restore original position
        {
            //transform.localPosition = homePos;
            //transform.localRotation = homeRot;
            //transform.localScale = homeScale;

            // enable hologram when return home
            if (destSibling.gameObject.name == "smirror")
            {
                foreach (Transform child in destSibling)
                {
                    child.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            else
            {
                destSibling.gameObject.GetComponent<MeshRenderer>().enabled = true; // disable hologram while real object is present
            }
            //Debug.LogFormat(this.transform.gameObject, "home_to_dest.ret_home()");
            if (this.gameObject.active == true)
            {
                StopAllCoroutines();
                StartCoroutine(grad_ret_home());
            }
            //grad_ret_home();
        }

        // --------------- BUG IN HERE -----------------
        // ------------------------------ FIX: set bring_kids_home.cs to loop through start_menu to grab objects under menu parent folders ------------------------------------------
        IEnumerator grad_ret_home()
        {
            //Debug.Log("grad_ret_home() called");

            // -------------------- HERE IS BUG -------------------
            // reset parent to homeParent
            this.transform.SetParent(homeParent); // this does not reset the parent of our transform

            // set starting info to current local transform
            // this transform should be ways away from the homeParents AND SHOULD NOT
            Vector3 startPos = this.transform.localPosition;
            Quaternion startRot = this.transform.localRotation;
            Vector3 startScale = this.transform.localScale;

            float linear = 0f;
            float t;

            // --------------------------- HERE IS SYMPTOM --------------------------
           //if (this.transform.localPosition == homePos)
           //{
           //    Debug.Log("already home!"); //this is always true!
           //}

            while (
                this.transform.localPosition != homePos //||
                //this.transform.localRotation != homeRot ||
                //this.transform.localScale != homeScale
                )
            {
                linear += Time.deltaTime * speed;
                t = Mathf.SmoothStep(0f, 1f, linear);

                //transform.localPosition = Vector3.Lerp(transform.localPosition, homePos, Time.deltaTime * speed_home);
                this.transform.localPosition = Vector3.Lerp(startPos, homePos, t);

                //transform.localScale = Vector3.Lerp(transform.localScale, homeScale, Time.deltaTime * speed_home);
                transform.localScale = Vector3.Lerp(startScale, homeScale, t);

                //transform.localRotation = Quaternion.Lerp(transform.localRotation, homeRot, Time.deltaTime * speed_home);
                transform.localRotation = Quaternion.Lerp(startRot, homeRot, t);
                Debug.LogFormat(this.transform.gameObject, "going home...");

                yield return null;
            }
            Debug.LogFormat(this.transform.gameObject,"grad_ret_home() finished");
        }

        public void to_dest() // bring to position
        {
            //transform.localPosition = homePos;
            //transform.localRotation = homeRot;
            //transform.localScale = homeScale;

            StopAllCoroutines();
            StartCoroutine(grad_to_dest());
           // grad_to_dest();
            this.transform.parent.gameObject.GetComponent<Rotator>().enabled = true;
        }

        IEnumerator grad_to_dest()
        {
            this.transform.SetParent(destParent);

            Vector3 startPos = this.transform.localPosition;
            Quaternion startRot = this.transform.localRotation;
            Vector3 startScale = this.transform.localScale;
            float linear = 0f;
            float t;

            while (
                this.transform.localPosition != destPos //||
                //this.transform.localRotation != destRot ||
                //this.transform.localScale != destScale
                )
            {
                linear += Time.deltaTime * this.speed;
                t = Mathf.SmoothStep(0f, 1f, linear);

                //transform.localPosition = Vector3.Lerp(transform.localPosition, homePos, Time.deltaTime * speed_home);
                transform.localPosition = Vector3.Lerp(startPos, destPos, t);

                //transform.localScale = Vector3.Lerp(transform.localScale, homeScale, Time.deltaTime * speed_home);
                transform.localScale = Vector3.Lerp(startScale, destScale, t);

                //transform.localRotation = Quaternion.Lerp(transform.localRotation, homeRot, Time.deltaTime * speed_home);
                transform.localRotation = Quaternion.Lerp(startRot, destRot, t);

                if (transform.localPosition == destPos)
                {
                    if (destSibling.gameObject.name == "smirror")
                    {
                        foreach (Transform child in destSibling)
                        {
                            child.gameObject.GetComponent<MeshRenderer>().enabled = false;
                        }
                    }
                    else
                    {
                        destSibling.gameObject.GetComponent<MeshRenderer>().enabled = false; // disable hologram while real object is present
                    }
                }
                Debug.LogFormat(this.transform.gameObject, "going to dest...");
                
                yield return null;
            }
            Debug.LogFormat(this.transform.gameObject, "grad_to_dest() finished");
        }
    }
}