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
    public class multi_grasp_scale : MonoBehaviour
    {
        public InteractionController rightHand;
        public InteractionController leftHand;
        public Transform hand_left;
        public Transform hand_right;

        private void Awake()
        {
            if (leftHand.isLeft)
            {
                Debug.LogFormat("left hand InteractionControler instantiated", leftHand.gameObject);
            }
            else
            {
                Debug.LogError("left hand InteractionControler NOT instantiated", leftHand.gameObject);
            }
            if (rightHand.isRight)
            {
                Debug.LogFormat("right hand InteractionControler instantiated", leftHand.gameObject);
            }
            else
            {
                Debug.LogError("right hand InteractionControler NOT instantiated", leftHand.gameObject);
            }
        }

        // Use this for initialization
        void Start()
        {
            //float start_scale = this.transform.localScale.x;
        }

        bool grab_init_vectors = true;
        Vector3 start_scale;
        Vector3 start_hand_dist;
        Vector3 trans_dist_vect;
        float start_dist_val;
        float trans_dist_val;
        float scale_val;
        // Update is called once per frame
        void Update()
        {
            if (leftHand.isGraspingObject && rightHand.isGraspingObject)
            {
                if (leftHand.graspedObject.name == this.transform.gameObject.name || rightHand.graspedObject.name == this.transform.gameObject.name)
                {
                    if (leftHand.graspedObject.name == rightHand.graspedObject.name) // if grab object with same hand
                    {
                        if (grab_init_vectors)
                        {
                            start_scale = this.transform.localScale;
                            start_hand_dist = hand_left.position - hand_right.position;
                            // change to start_hand_dist.magnitute!!!
                            //start_dist_val = (float)Math.Sqrt(Math.Pow((float)start_hand_dist.x, 2f) + Math.Pow((float)start_hand_dist.y, 2f) + Math.Pow((float)start_hand_dist.z, 2f));
                            start_dist_val = start_hand_dist.magnitude;
                            grab_init_vectors = false;
                        }
                        else
                        {
                            trans_dist_vect = hand_left.position - hand_right.position;
                            //trans_dist_val = (float)Math.Sqrt(Math.Pow((float)trans_dist_vect.x, 2f) + Math.Pow((float)trans_dist_vect.y, 2f) + Math.Pow((float)trans_dist_vect.z, 2f));
                            trans_dist_val = trans_dist_vect.magnitude;
                            //string trans_vect_debug = trans_dist_vect.ToString();
                            //Debug.Log(trans_vect_debug);

                            // find scale by dividing trans/start distance vals
                            scale_val = trans_dist_val / start_dist_val;
                            Debug.LogFormat("scale_val = {0}", scale_val);

                            // transform the local scale by a factor of scale_val
                            this.transform.localScale = start_scale * scale_val;
                        }
                    }
                    else
                    {
                        grab_init_vectors = true;
                    }
                }
            }
        }
    }
}

