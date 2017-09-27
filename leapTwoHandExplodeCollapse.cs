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
    public class leapTwoHandExplodeCollapse : MonoBehaviour
    {
        // player variables
        public Controller leapController;
        public InteractionController leftInteractionController;
        public InteractionController rightInteractionController;
        public Transform leftPalm;
        public Transform rightPalm;
        Frame currentFrame;
        public Transform cameraTransform;

        // target variables
        public Transform targetTransform;

        void Start()
        {
            leapController = new Controller();
        }
        // Update is called once per frame
        //void Update()
        //{
        //    if (/*target_in_sight() &&*/ !(leftInteractionController.isPrimaryHovering || rightInteractionController.isPrimaryHovering))
        //    {
        //        two_hand_explode();
        //        two_hand_collapse();
        //        //Debug.Log("No Hovering, two hand functions called");
        //    }
        //}//end update
        bool target_in_sight()
        {
            bool targetFound = false;
            Ray gaze = new Ray(cameraTransform.position, cameraTransform.forward);
            RaycastHit hitTarget;
            targetFound = targetTransform.GetComponent<Collider>().Raycast(gaze, out hitTarget, 100f); // ------------------- NOTE: COLLIDER MUST BE ENABLED------------------
            return targetFound;
        }

        #region TWO_HAND_GESTURES
        public Hand hand1, hand2;
        Frame startFrame;
        public float handDist = 0f;
        public int gestureFrameLimit = 90;
        public float distanceHandsTogether = 40f;
        public float distanceHandsApart = 400f;

        public bool readExplodeGesture = false; // output to state machine
        int frameCountExplodeGesture = 0;

        public void updateHandDist() // for state machine
        {
            if (leapController.IsConnected)
            {
                currentFrame = leapController.Frame(); // construct controller object
                if (currentFrame.Hands.Count > 1)
                {
                    // build hand list
                    List<Hand> hands = currentFrame.Hands;
                    hand1 = hands[0];
                    hand2 = hands[1];
                    if (hand1.TimeVisible > 0.25f && hand2.TimeVisible > 0.25f)
                    {
                        Vector3 hand1Pos = hand1.PalmPosition.ToVector3();
                        Vector3 hand2Pos = hand2.PalmPosition.ToVector3();
                        handDist = Vector3.Distance(hand2Pos, hand1Pos);
                    }
                }
            }
        }
        public void two_hand_explode()
        {
            if (readExplodeGesture) // count frame duration 
            {
                frameCountExplodeGesture++;
                if (frameCountExplodeGesture >= gestureFrameLimit) // disable gesture read if waited too long
                {
                    readExplodeGesture = false;
                    frameCountExplodeGesture = 0;
                }
            }
            //Vector3 handDirection = anyHand.Direction.ToVector3();
            if (leapController.IsConnected)
            {
                currentFrame = leapController.Frame(); // construct controller object
                if (currentFrame.Hands.Count > 1)
                {
                    // build hand list
                    List<Hand> hands = currentFrame.Hands;
                    hand1 = hands[0];
                    hand2 = hands[1];
                    if (hand1.TimeVisible > 0.25f && hand2.TimeVisible > 0.25f)
                    {
                        Vector3 hand1Pos = hand1.PalmPosition.ToVector3();
                        Vector3 hand2Pos = hand2.PalmPosition.ToVector3();
                        handDist = Vector3.Distance(hand2Pos, hand1Pos);
                        if (handDist < distanceHandsTogether && !readExplodeGesture) // if hands together, capture first frame, start reading gesture
                        {
                            // Debug.Log("Begin Reading Explode");
                            startFrame = leapController.Frame();
                            readExplodeGesture = true;
                        }
                        else if (handDist >= distanceHandsApart && readExplodeGesture && frameCountExplodeGesture < gestureFrameLimit) // gesture completed successfully!
                        {
                            //Debug.Log("Explode Gesture Conditions Met");
                            targetTransform.GetComponent<bring_kids_home>().all_children_home();
                            targetTransform.GetComponent<Animator>().ResetTrigger("collapse");
                            targetTransform.GetComponent<Animator>().SetTrigger("explode");
                        }
                    }
                }
            }
        }
        int frameCountCollapseGesture = 0;
        public bool readCollapseGesture = false;
        public void two_hand_collapse()
        {
            if (readCollapseGesture) // count frame duration 
            {
                frameCountCollapseGesture++;
                if (frameCountCollapseGesture >= gestureFrameLimit) // disable gesture read if waited too long
                {
                    readCollapseGesture = false;
                    frameCountCollapseGesture = 0;
                }
            }
            //Vector3 handDirection = anyHand.Direction.ToVector3();
            if (leapController.IsConnected)
            {
                currentFrame = leapController.Frame(); // construct controller object
                if (currentFrame.Hands.Count > 1)
                {
                    // build hand list
                    List<Hand> hands = currentFrame.Hands;
                    hand1 = hands[0];
                    hand2 = hands[1];
                    if (hand1.TimeVisible > 0.5f && hand2.TimeVisible > 0.5f)
                    {
                        Vector3 hand1Pos = hand1.PalmPosition.ToVector3();
                        Vector3 hand2Pos = hand2.PalmPosition.ToVector3();
                        handDist = Vector3.Distance(hand2Pos, hand1Pos);
                        if (handDist > distanceHandsApart && !readCollapseGesture) // if hands together, capture first frame, start reading gesture
                        {
                            //Debug.Log("Begin Reading Collapse");
                            startFrame = leapController.Frame();
                            readCollapseGesture = true;
                        }
                        else if (handDist <= distanceHandsTogether && readCollapseGesture && frameCountExplodeGesture < gestureFrameLimit) // gesture completed successfully!
                        {
                            //Debug.Log("Collapse Gesture Conditions Met");
                            targetTransform.GetComponent<bring_kids_home>().all_children_home();
                            targetTransform.GetComponent<Animator>().ResetTrigger("explode");
                            targetTransform.GetComponent<Animator>().SetTrigger("collapse");
                        }
                    }
                }
            }
        }
        #endregion
    }//end class(script)
}//end namespace
