using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
    public class leapHandLaser : MonoBehaviour
    {
        //leap variables
        public Controller leapController;
        private Frame thisFrame;
        public Hand thisHand;
        public bool isLeapTeleporter;
        public bool isLeapGrabber;

        public Transform StartPoint;
        public Transform directionPoint;

        public Ray ray;
        public float distance;
        public bool showBeam;
        public bool showMarker;
        public bool laserActive = false;

        public float BeamDiameter = 0.01f;
        public float MarkerDiameter = 0.25f;

        private GameObject Beam;
        private Renderer BeamRenderer;
        public Material BeamMaterial;
        private GameObject Marker;
        private Renderer MarkerRenderer;
        public Material MarkerMaterial;
        public Vector3 localOriginOffset = Vector3.zero;

        [Tooltip("List of GameObject Colliders to Ignore")]
        [SerializeField]
        public List<GameObject> ignoreColliders = new List<GameObject>();
        private bool ignoreThisCollider = false;

        private readonly Quaternion x90 = Quaternion.Euler(90f, 0f, 0f);

        public bool Raycast(Ray ray, IEnumerable<Collider> colliders, out RaycastHit hit, float maxDistance = float.PositiveInfinity)
        {
            hit = new RaycastHit();

            // find collider
            float distance = float.PositiveInfinity;
            RaycastHit temp;
            foreach (var c in colliders)
            {
                foreach (var ignore in ignoreColliders)
                {
                    if (c.gameObject == ignore) ignoreThisCollider = true;
                }
                if (c.gameObject.name == "Contact Palm Bone" || c.gameObject.name == "Contact Finger Bone") ignoreThisCollider = true;
                //Debug.LogFormat("Collider: " + c.gameObject.name, c.gameObject);
                if (c.Raycast(ray, out temp, maxDistance) && !ignoreThisCollider)
                {
                    //Debug.LogFormat("Collider Hit: " + c.gameObject.name, c.gameObject);
                    if (temp.distance < distance)
                    {
                        distance = temp.distance;
                        hit = temp;
                        //Debug.Log("Distance (inside fn) = " + distance);
                        //Debug.Log("Distance raycase hit (inside fn) = " + temp.distance);
                    }
                }
                ignoreThisCollider = false;
            }

            return distance < float.PositiveInfinity;
        }
        void OnEnable()
        {
            if (this.Beam == null)
            {
                // create laser beam
                this.Beam = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                this.Beam.name = "beam";
                this.Beam.transform.SetParent(this.transform, false);
                this.Beam.layer = this.gameObject.layer;
                this.BeamRenderer = this.Beam.GetComponent<Renderer>();
                this.BeamRenderer.material = BeamMaterial;

            }
            if (this.Marker == null)
            {
                // create marker
                this.Marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                this.Marker.name = "marker";
                this.Marker.transform.SetParent(this.transform, false);
                this.Marker.layer = this.gameObject.layer;
                this.MarkerRenderer = this.Marker.GetComponent<Renderer>();
                this.MarkerRenderer.material = MarkerMaterial;

            }
            // grab leapController info for laser beam direction
            leapController = new Controller();

            // add new colliders in laser to ignore list
            ignoreColliders.Add(this.Marker);
            ignoreColliders.Add(this.Beam);

            this.Update();
        }
        float update_distance(Vector3 dir)
        {
            float newDistance;
            Ray laserRay = new Ray(this.StartPoint.position, dir);
            Collider[] colliders = FindObjectsOfType<Collider>();
            RaycastHit hit;
            Raycast(laserRay, colliders, out hit);
            newDistance = hit.distance;
            return newDistance;
        }
        // grab position of marker to teleport
        public Vector3 grabMarkerPos()
        {
            return this.Marker.transform.position;
        }
        public void Update()
        {
            if (laserActive)
            {
                showBeam = true;
                showMarker = true;
            }
            else
            {
                showBeam = false;
                showMarker = false;
            }
            // grab leapController info for laser beam direction
            if (leapController.IsConnected)
            {
                thisFrame = leapController.Frame();
                List<Hand> hands = thisFrame.Hands;
                if (hands.Count > 0)
                {
                    if (hands[0].IsRight) thisHand = hands[0];
                }
                if (this.StartPoint != null && this.directionPoint != null && thisHand.IsRight)
                {

                    // update distance with raycast --> port to new function later
                    Vector3 direction = this.directionPoint.position - this.StartPoint.position;
                    distance = update_distance(direction);

                    // general beam/marker vector info
                    Vector3 midpoint = this.StartPoint.position + (direction.normalized * distance)/2;
                    Vector3 endpoint = this.StartPoint.position + (direction.normalized * distance);
                    Quaternion rotation = Quaternion.LookRotation(direction) * x90;
                    
                    // update showbeam and showmarker variables with distance condition
                    if (distance < 0.1f)
                    {
                        showBeam = false;
                        showMarker = false;
                    }
                   
                    if (showBeam) // generate beam
                    {
                        this.Beam.gameObject.SetActive(true);
                        this.Beam.transform.position = midpoint;
                        this.Beam.transform.rotation = rotation;
                        Vector3 beamScale = new Vector3(this.BeamDiameter, this.distance / 2f, this.BeamDiameter);
                        if (this.Beam.transform.localScale != beamScale) this.Beam.transform.localScale = beamScale;
                    }
                    else
                    {
                        this.Beam.gameObject.SetActive(false);
                    }
                    if (showMarker) // generate marker
                    {
                        this.Marker.gameObject.SetActive(true);
                        this.Marker.transform.position = endpoint;
                        Vector3 markerScale = Vector3.one * this.MarkerDiameter;
                        if (this.Marker.transform.localScale != markerScale) this.Marker.transform.localScale = markerScale;
                    }
                    else
                    {
                        this.Marker.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}


