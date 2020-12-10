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
    private GameObject spawnedObject;
    private GameObject worldBoundaries;
    private Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Animator animator;
    private Animation animation;

    public GameObject placementIndicator;
    private ARRaycastManager aRRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private Vector3 defaultObjectPosition;

    // Start is called before the first frame update
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        Physics.gravity = new Vector3(0, -10.0F, 0);
        resetButton.onClick.AddListener(ResetObject);
    }

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
                spawnedObject.transform.Rotate(0f, 180f, 0f, Space.Self);
                defaultObjectPosition = spawnedObject.transform.localPosition;
                if (animator == null)
                {
                    animator = FindObjectOfType<Animator>();
                }
                animator.enabled = true;
            }
            //regular tappin
            else
            {
                Debug.Log("Tapped.");
                animator.enabled = false;
                Debug.Log("grav init");
                Physics.gravity = new Vector3(UnityEngine.Random.Range(-100.0f, 100.0f), UnityEngine.Random.Range(-50.0f, 200.0f), UnityEngine.Random.Range(-100.0f, 100.0f));
                StartCoroutine(hit());
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

    //Timer for hit minimum 1 second :/
    IEnumerator hit()
    {

        yield return new WaitForSeconds(1);
        Debug.Log("registered hit");

        Physics.gravity = new Vector3(0, -100.0F, 0);
        Debug.Log("grav reset");
    }

    //reset: missing positional reset right now
    void ResetObject()
    {
        spawnedObject.transform.position = defaultObjectPosition;
        animator.Play("get u");
        animator.enabled = true;

        Debug.Log("reset");
    }
}