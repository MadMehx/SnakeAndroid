using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject gameObjectToInstantiate;
    public Button resetButton;
    public GameObject placementIndicator;

    private GameObject spawnedObject;
    private GameObject worldBoundaries;
    private Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Animator animator;
    private Animation animation;
    private ARRaycastManager aRRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private Vector3 defaultObjectPosition;
    public Pose spawnPose;
    int lol;
    bool tapped;
    Vector3 punch;

    // Start is called before the first frame update
    void Start()
    {
        // Find AR raycast manager
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        // add listener to reset button
        resetButton.onClick.AddListener(ResetObject);
        // initialize gravity to 0
        Physics.gravity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // find all rigidbodies in scene
        var foundRigidBodies = FindObjectsOfType<Rigidbody>();
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        // check if screen is tapped, if not then return
        if (!TryToGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }
        // check for planes
        if (aRRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            tapped = true;
            // get all possible surfaces, select the closest one
            var hitPose = hits[0].pose;

            // adjust the height of the object by .2
            hitPose.position.y = hitPose.position.y + 0.2f;
            spawnPose = hitPose;

            if (spawnedObject == null)
            {
                // if no object is spawned, then instantiate object
                spawnedObject = Instantiate(gameObjectToInstantiate, spawnPose.position, spawnPose.rotation);
                // rotate to face screen
                spawnedObject.transform.Rotate(0f, 180f, 0f, Space.Self);
                // find animator
                if (animator == null)
                {
                    animator = FindObjectOfType<Animator>();
                }
                animator.enabled = false;
                defaultObjectPosition = spawnedObject.transform.localPosition;
                lol = 0;
                StartCoroutine(hit());
            }
            else
            {
                // object has already spawned, user has tapped
                // set gravity
                Physics.gravity = new Vector3(0, -1, 0);
                animator.enabled = false;
                lol = 0;
                // edit all rigidbodies for ragdoll
                while (foundRigidBodies.Length > lol)
                {
                    foundRigidBodies[lol].isKinematic = false;
                    foundRigidBodies[lol].detectCollisions = true;
                    lol++;
                }
                // apply random force to a random rigidbody
                punch = new Vector3(UnityEngine.Random.Range(-20.0f, 20.0f), UnityEngine.Random.Range(-10.0f, 40.0f), UnityEngine.Random.Range(-20.0f, 20.0f));
                foundRigidBodies[UnityEngine.Random.Range(0, lol)].AddForce(punch, ForceMode.Impulse);
            }
        }
    }

    // display placement indicator on screen, update its location
    // if no plane is visible in center of screen, then placement indicator will not be displayed
    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    // get a touch position on screen
    bool TryToGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void UpdatePlacementPose()
    {
        // find center of screen
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f,0.5f));
        // create list of raycast hits
        var hits = new List<ARRaycastHit>();

        // send a ray from center of screen forward to find any planes
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        // placement pose is valid if a plane is hit
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            // set placement pose to the first plane in list
            placementPose = hits[0].pose;
        }
    }

    // defines how a long the physics hit will last on the rigidbody
    IEnumerator hit()
    {

        yield return new WaitForSeconds(1);
        ResetObject();
    }

    // delay timer and logic for resetting object
    IEnumerator phyDelay()
    {
        // find all rigidbodies
        var foundRigidBodies = FindObjectsOfType<Rigidbody>();
        // set object to default position
        spawnedObject.transform.position = defaultObjectPosition;
        // reset gravity
        Physics.gravity = new Vector3(0, 0, 0);
        // enable animator for getup animation
        animator.enabled = true;
        tapped = false;
        yield return new WaitForSeconds(10);
        // if screen is not tapped, animation will play.
        if (tapped == false)
        {
            lol = 0;
            while (foundRigidBodies.Length > lol)
            {
                foundRigidBodies[lol].isKinematic = true;
                foundRigidBodies[lol].detectCollisions = false;
                lol++;
            }
        }   
    }

    // reset the object to its original position
    void ResetObject()
    {
        // find all rigidbodies
        var foundRigidBodies = FindObjectsOfType<Rigidbody>();
        // set object to default position
        spawnedObject.transform.position = defaultObjectPosition;
        // remove gravity
        Physics.gravity = new Vector3(0, 0, 0);
        // set all rigidbodies to avoid collision
        lol = 0;
        while (foundRigidBodies.Length > lol)
        {
            foundRigidBodies[lol].detectCollisions = false;
            lol++;
        }
        // play get up animation
        animator.enabled = true;
        animator.Play("get u");
        StartCoroutine(phyDelay());
    }
}