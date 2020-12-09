using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject gameObjectToInstantiate;
    public GameObject gameWorldBoundaries;
    private GameObject spawnedObject;
    private GameObject worldBoundaries;
    private Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    animator = FindObjectOfType<Animator>();

    public GameObject placementIndicator;
    private ARRaycastManager aRRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    // Start is called before the first frame update
    void Start()
    {

        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }
    private Animator animator;

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (!TryToGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }
        if (aRRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            // get all possible surfaces, select the closest one
            var hitPose = hits[0].pose;

            // adjust the height of the object by .2
            hitPose.position.y = hitPose.position.y + 0.2f;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                //spawnedObject.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                worldBoundaries = Instantiate(gameWorldBoundaries, hitPose.position, hitPose.rotation);
            }
            else
            {
                //spawnedObject.transform.position = hitPose.position;
                //worldBoundaries.transform.position = hitPose.position;
                Animator.enabled = false;
            }
        }
    }

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
}
