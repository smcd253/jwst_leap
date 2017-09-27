using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity.Interaction
{
    public class gestureStateMachine : MonoBehaviour
    {
        public leapGesturePinchRotate pinchRot;
        public leapTwoHandExplodeCollapse twoHandExpCol;
        public leapHandTeleport leapTeleport;
        public scene_state_machine stateManager;
        private leapHandLaser laserRenderer;


        // hand objects
        public InteractionController leftInteractionController;
        public InteractionController rightInteractionController;
        public Transform leftNormalStart, leftNormalDir, rightNormalStart, rightNormalDir;

        // transform objects
        public Transform cameraTransform;
        public Transform targetTransform;

        // key boolean definitions
        public bool oneHV, twoHV; // 1 hand, 2 hands visible
        public bool LaT; // looking at target
        public bool HFE; // hands facing each other
        public bool HC, HFA; // hands close together, far apart
        public bool LHC; // left hand closer to leap controller (or camera) than right hand
        public bool LH, RH; // left, right hand holding object
        public bool TC; // camera transform too close to target transform
        public bool RHP; // right hand pinching

        // adjustable parameters
        public float tooClose = 5f;
        public float tooCloseRead;
        // runtime read only (debug variables)
        
        // state machine enumerator
        public enum gesture
        {
            none,
            teleport,
            rotate,
            exp_col,
        };
        public gesture currentGesture;

        // Use this for initialization
        void Start()
        {
            laserRenderer = leapTeleport.handLaser;
            currentGesture = gesture.none;
            oneHV = false;
            twoHV = false;
            LaT = false;
            HFE = false;
            LHC = false;
            LH = false;
            RH = false;
            TC = false;
            RHP = false;
        }

        bool target_in_sight()
        {
            bool targetFound = false;
            Ray gaze = new Ray(cameraTransform.position, cameraTransform.forward);
            RaycastHit hitTarget;
            targetFound = targetTransform.GetComponent<Collider>().Raycast(gaze, out hitTarget, 100f); // ------------------- NOTE: COLLIDER MUST BE ENABLED------------------
            return targetFound;
        }

        public float HFEAngle = 120f;
        public float handAngle;

        bool hands_facing_eachother()
        {
            Vector3 leftBeam = leftNormalDir.position - leftNormalStart.position;
            Vector3 rightBeam = rightNormalDir.position - rightNormalStart.position;

            handAngle = Vector3.Angle(leftBeam, rightBeam);
            if (handAngle >= HFEAngle) return true;
            else return false;
        } 
        
        bool left_hand_closer()
        {
            float leftToEye = Vector3.Magnitude(twoHandExpCol.leftPalm.position - cameraTransform.position);
            float rightToEye = Vector3.Magnitude(twoHandExpCol.rightPalm.position - cameraTransform.position);
            if (leftToEye < rightToEye) return true;
            else return false;
        }
        // Update is called once per frame
        void Update()
        {
            // update variables coming from gesture scripts
            twoHandExpCol.updateHandDist(); // for HC, HFA, LHC
            //pinchRot.updateDistRead(); // for TC
            leapTeleport.updateLaserHandPinch(); // for RHP
            tooCloseRead = Vector3.Distance(targetTransform.position, cameraTransform.position);

            // give input booleans value
            oneHV = leftInteractionController.isActiveAndEnabled || rightInteractionController.isActiveAndEnabled;
            twoHV = leftInteractionController.isActiveAndEnabled && rightInteractionController.isActiveAndEnabled;
            //LaT = target_in_sight();
            if (twoHV)
            {
                LHC = left_hand_closer();
                HFE = hands_facing_eachother();
            }
            else
            {
                LHC = false;
                HFE = false;
            }
            LH = leftInteractionController.isGraspingObject;
            RH = rightInteractionController.isGraspingObject;
            TC = tooCloseRead <= tooClose;
            if (leapTeleport.laserHandInstantiated) RHP = leapTeleport.laserHand.IsPinching();

            if (!TC)
            {
                pinchRot.pinch_rotate();
            }
            twoHandExpCol.two_hand_collapse();
            twoHandExpCol.two_hand_explode();

            if (!RH && !LH && LHC && !RHP)
            {
                laserRenderer.laserActive = true;
                leapTeleport.teleport();
            }
            else laserRenderer.laserActive = false;

            if (targetTransform.GetComponent<Rotator>())
            {
                if (pinchRot.isPinchRotating) targetTransform.GetComponent<Rotator>().rotateLocal = false;
                else targetTransform.GetComponent<Rotator>().rotateLocal = true;
            }

            //if (oneHV || twoHV)
            //{
            //    if (!LH && !RH && twoHV)
            //    {
            //        if (HFE) currentGesture = gesture.exp_col;
            //        else if (LHC && !RHP) currentGesture = gesture.teleport;
            //    }
            //    else if (!TC && (!((LH && pinchRot.anyHand.IsLeft) || (RH && pinchRot.anyHand.IsRight))))
            //    {
            //        currentGesture = gesture.rotate;
            //    }
            //}
            //else currentGesture = gesture.none;
            //
            //if (currentGesture != gesture.teleport) laserRenderer.laserActive = false;
            //
            //switch (currentGesture)
            //{
            //    case gesture.none:
            //        break;
            //    case gesture.teleport:
            //        // replace leapHandTeleport Update() with teleport() function
            //        leapTeleport.teleport();
            //        break;
            //    case gesture.rotate:
            //        // replace leapGesturePinchRotate Update() w/ rotate() function
            //        pinchRot.pinch_rotate();
            //        break;
            //    case gesture.exp_col:
            //        // call leapTwoHandExplodeCollapse explode() function
            //        twoHandExpCol.two_hand_explode();
            //        twoHandExpCol.two_hand_collapse();
            //        break;
            //}
        }
    }
}

