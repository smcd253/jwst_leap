using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity.Interaction
{
    public class leapHandTeleport : MonoBehaviour
    {
        public leapHandLaser handLaser;
        public Controller leapController;
        public InteractionController interactLaserHand;
        public Transform playerCamera;
        public float teleportSpeed = 0.5f;
        public bool isTeleporting = false;
        private Frame thisFrame;
        public Hand laserHand;
        public Hand otherHand;
        private Vector3 markerPosOnTeleport;

        public bool laserHandInstantiated = false;

        bool beginActionCapture = false;
        int captureFrameCount = 0;

        bool bufferActionCapture = false;
        int bufferFrameCount = 0;

        // Use this for initialization
        void Start()
        {
            leapController = new Controller();
            isTeleporting = false; // force public boolean
            
        }
        IEnumerator leapTeleport()
        {
            float linear = 0f;
            float t;
            while (playerCamera.transform.position != handLaser.grabMarkerPos())
            {
                isTeleporting = true;

                linear += Time.deltaTime * this.teleportSpeed;
                t = Mathf.SmoothStep(0f, 1f, linear);

                playerCamera.transform.position = Vector3.Lerp(markerPosOnTeleport, playerCamera.transform.position, t);

                yield return null;
            }
            isTeleporting = false;
        }
        public void updateLaserHandPinch()
        {
            if (leapController.IsConnected)
            {
                thisFrame = leapController.Frame();
                List<Hand> hands = thisFrame.Hands;

                if (hands.Count > 1)
                {
                    if (hands[0].IsRight)
                    {
                        laserHand = hands[0];
                        laserHandInstantiated = true;
                        otherHand = hands[1];
                    }
                    else if (hands[1].IsRight)
                    {
                        laserHand = hands[1];
                        laserHandInstantiated = true;
                        otherHand = hands[0];
                    }
                }
            }
        }
        // teleport function called on gestureStateMachine
        public void teleport()
        {
            //Debug.Log("Begin Action Capture = " + beginActionCapture);
            //Debug.Log("Capture Frame Count = " + captureFrameCount);
            if (beginActionCapture)
            {
                captureFrameCount++;
                if (captureFrameCount > 45)
                {
                    beginActionCapture = false; // capture period is 0.5 seconds
                    captureFrameCount = 0;
                    bufferActionCapture = true;
                }
            }
            if (bufferActionCapture) // prevents repeated teleport calls
            {
                bufferFrameCount++;
                if (bufferFrameCount > 45)
                {
                    bufferActionCapture = false; // buffer period is 0.5 seconds
                    bufferFrameCount = 0;
                    isTeleporting = false;
                }
            }
            if (leapController.IsConnected)
            {
                thisFrame = leapController.Frame();
                List<Hand> hands = thisFrame.Hands;

                if (hands.Count > 1)
                {
                    if (hands[0].IsRight)
                    {
                        laserHand = hands[0];
                        laserHandInstantiated = true;
                        otherHand = hands[1];
                    }
                    else if (hands[1].IsRight)
                    {
                        laserHand = hands[1];
                        laserHandInstantiated = true;
                        otherHand = hands[0];
                    }
                    if (otherHand.IsPinching() && !beginActionCapture && !bufferActionCapture) // start pinch (after buffer period is over --> begin waiting for release pinch
                    {
                        beginActionCapture = true;
                    }
                    if (beginActionCapture && !otherHand.IsPinching()) // if waiting for release pinch and release pinch occurs
                    {
                        markerPosOnTeleport = handLaser.grabMarkerPos();
                        beginActionCapture = false;
                        StopAllCoroutines();
                        StartCoroutine(leapTeleport());
                        bufferActionCapture = true;
                    }
                }
                else if (hands.Count > 0)
                {
                    if (hands[0].IsRight)
                    {
                        laserHand = hands[0];
                        laserHandInstantiated = true;
                        otherHand = null;
                    }
                }
                else laserHandInstantiated = false;
                // activate/deactivate laser conditions
                if (laserHandInstantiated)
                {
                    if (interactLaserHand.isGraspingObject) handLaser.laserActive = false;
                    else handLaser.laserActive = true;
                    if (laserHand.IsPinching()) handLaser.laserActive = false;
                    else handLaser.laserActive = true;
                }     
            }
        }
    }
}