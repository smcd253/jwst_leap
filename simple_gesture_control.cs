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
    public class simple_gesture_control : MonoBehaviour
    {
        // player variables
        public Controller leapController;
        public InteractionController leftInteractionController;
        public InteractionController rightInteractionController;
        public Transform leftPalm;
        public Transform rightPalm;
        Frame currentFrame;
        Frame firstFrame;
        Hand anyHand;
        Transform palm;
        public Transform cameraTransform;
        Vector3 palmStartPosition;
        Vector3 palmPreviousPosition;
        public float sensitivity = 3f;
        public float distRead;

        // target variables
        public Transform targetTransform;
        Quaternion targetHomeRotation;

        // other variables
        bool grabStartPinch = true;

        void Start()
        {
            leapController = new Controller();
            targetHomeRotation = targetTransform.GetComponent<Rigidbody>().rotation;
        }
        // Update is called once per frame
        void Update()
        {
            if (/*target_in_sight() &&*/ !(leftInteractionController.isGraspingObject || rightInteractionController.isGraspingObject))
            {
                pinch_rotate();
                two_hand_explode();
                two_hand_collapse();
            }
        }//end update
        bool target_in_sight()
        {
            bool targetFound = false;
            Ray gaze = new Ray(cameraTransform.position, cameraTransform.forward);
            RaycastHit hitTarget;
            targetFound = targetTransform.GetComponent<Collider>().Raycast(gaze, out hitTarget, 100f); // ------------------- NOTE: COLLIDER MUST BE ENABLED------------------
            return targetFound;
        }
        public void pinch_rotate()
        {
            //Vector3 handDirection = anyHand.Direction.ToVector3();
            if (leapController.IsConnected)
            {
                currentFrame = leapController.Frame(); // construct controller object
                if (currentFrame.Hands.Count > 0)
                {
                    // build hand list
                    List<Hand> hands = currentFrame.Hands;
                    //for (int i = 0; i < hands.Count; i++)
                    //{
                        anyHand = hands[0];
                    if (anyHand.IsLeft)
                    {
                        palm = leftPalm;
                    }
                    else
                    {
                        palm = rightPalm;
                    }
                        //palm.position = anyHand.PalmPosition.ToVector3();
                        if (anyHand.TimeVisible > 0.5f) // if hand has been on screen for more than 0.5 seconds
                        {
                            // ignore raycast for debugging purposes
                            if (anyHand.IsPinching() && grabStartPinch)
                            {
                                palmStartPosition = palm.position;
                                palmPreviousPosition = palm.position;
                                grabStartPinch = false;
                            }
                            else if (anyHand.IsPinching())
                            {
                                Vector3 deltaPalmPosition = palm.position - palmStartPosition;
                                Quaternion rotateFactor = Quaternion.Euler(0, -deltaPalmPosition.x, 0);
                                //Vector3 rotateFactorVect = new Vector3(0, deltaPalmPosition.x, 0);
                                //distance of hand to target for rotation scaling
                                Vector3 dirPrevious = palmPreviousPosition - targetTransform.position;
                                Vector3 dirCurrent = palm.position - targetTransform.position;
                                float distCurrentMag = dirCurrent.magnitude;
                                this.distRead = distCurrentMag;
                                dirPrevious.y = 0f;
                                dirPrevious.Normalize();
                                dirCurrent.y = 0f;
                                dirCurrent.Normalize();

                                float angle = Vector3.Angle(dirPrevious, dirCurrent);
                                float sign = Mathf.Sign(Vector3.Cross(dirPrevious, dirCurrent).y);

                                targetTransform.Rotate(0f, sensitivity * distCurrentMag * sign * angle, 0f);
                                //Debug.Log("Pinching... " + targetTransform.name + " should be rotating");
                                palmPreviousPosition = palm.position;
                                //targetTransform.rotation *= Quaternion.Euler(0f, sensitivity * distCurrentMag * sign * angle, 0f); // DON'T DO THIS!
                                //targetTransform.rotation = Quaternion.RotateTowards(targetTransform.rotation, targetTransform.rotation * rotateFactor, 360f);
                                //targetTransform.rotation = Quaternion.FromToRotation(targetTransform.up, targetTransform.up + rotateFactorVect);
                            }
                            else
                            {
                                grabStartPinch = true;
                            }
                        //}
                    }
                }
            }
        }
        bool grabFirstFrame = true;
        
        int contractedFingerCount = 0;
        public void hand_expand_explode()
        {
            //Vector3 handDirection = anyHand.Direction.ToVector3();
            if (leapController.IsConnected)
            {
                currentFrame = leapController.Frame(); // construct controller object
                if (currentFrame.Hands.Count > 0)
                {
                    // build hand list
                    List<Hand> hands = currentFrame.Hands;
                    for (int i = 0; i < hands.Count; i++)
                    {
                        anyHand = hands[i];
                        if (anyHand.TimeVisible > 0.5f) // if hand has been on screen for more than 0.5 seconds
                        {
                            //grab first frame if all fingers contracted
                            for (int f = 0; f < 5; f++) // loop through fingers
                            {
                                if (!anyHand.Finger(f).IsExtended)
                                {
                                    contractedFingerCount++;
                                }
                            }
                            if (contractedFingerCount == 5 && grabFirstFrame) // fist made (fingers contracted) start counting frames
                            {
                                firstFrame = leapController.Frame();
                                grabFirstFrame = false;
                            }
                        }
                    }
                }
            }
        } // not using for now

#region TWO_HAND_GESTURES
        Hand hand1, hand2;
        Frame startFrame;
        public float handDist = 0f;
        public int gestureFrameLimit = 90;
        public float distanceHandsTogether = 40f;
        public float distanceHandsApart = 400f;

        bool readExplodeGesture = false;
        int frameCountExplodeGesture = 0;
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
        bool readCollapseGesture = false;
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

// JASON QUATERNION NOTES
//Vector3 up = Vector3.up, fwd = Vector3.forward;
//
//Quaternion q1 = Quaternion.identity;
//up = q1 * up;
//fwd = q1 * fwd;
//
//Quaternion q2 = Quaternion.identity;
//up = q2 * up;
//fwd = q2 * fwd;
//
////...
//
//Quaternion result1 = q1 * Quaternion.Inverse(q2);
//Quaternion result2 = Quaternion.LookRotation(fwd, up);
//
////result1 != result2 (usually)
