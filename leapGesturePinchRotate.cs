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
    public class leapGesturePinchRotate : MonoBehaviour
    {
        // player variables
        public Controller leapController;
        public InteractionController leftInteractionController;
        public InteractionController rightInteractionController;
        public Transform leftPalm;
        public Transform rightPalm;
        Frame currentFrame;
        Frame firstFrame;
        public Hand anyHand;
        Transform palm;
        public Transform cameraTransform;
        Vector3 palmStartPosition;
        Vector3 palmPreviousPosition;
        public float sensitivity = 1f;
        public float distRead;

        // target variables
        public Transform targetTransform;
        Quaternion targetHomeRotation;

        // other variables
        bool grabStartPinch = true;
        public bool isPinchRotating = false; // output bool for state machine

        void Start()
        {
            leapController = new Controller();
            targetHomeRotation = targetTransform.GetComponent<Rigidbody>().rotation;
        }
        // Update is called once per frame
        //void Update()
        //{
        //    if (/*target_in_sight() &&*/ !(leftInteractionController.isPrimaryHovering || rightInteractionController.isPrimaryHovering))
        //    {
        //        pinch_rotate();
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
        public void updateDistRead() // for state machine
        {
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
                    Vector3 dirCurrent = palm.position - targetTransform.position;
                    float distCurrentMag = dirCurrent.magnitude;
                    this.distRead = distCurrentMag;
                }
            }

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
                            // STATE MACHINE
                            isPinchRotating = true;

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
                            // STATE MACHINE
                            isPinchRotating = false;
                            grabStartPinch = true;
                        }
                        //}
                    }
                }
            }
        }
    }
}