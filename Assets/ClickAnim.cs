using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ClickAnim : MonoBehaviour
{
    private Animator animator;
    private Animation animation;
    int tap = 0;
    int lol;

    // Start
    // Begin and call the following function once
    void Start()
    {
        Physics.gravity = new Vector3(0, -50, 0);
        GetComponent<Rigidbody>().inertiaTensor = GetComponent<Rigidbody>().inertiaTensor + new Vector3(0, 0, GetComponent<Rigidbody>().inertiaTensor.z * 100);

    }

    // Update
    // Update is called once per frame with the following functions
    void Update()
    {
        // Detect click from the user and print out a log.
        if (Input.GetMouseButtonDown(0))
        {
            var foundRigidBodies = FindObjectsOfType<Rigidbody>();

            // Print out log to confirm button is pressed
            Debug.Log("Pressed primary button.");

            if (animator == null)
            {
                animator = FindObjectOfType<Animator>();
            }

            // Default animation before clicking
            if (tap == 0)
            {
                // Turn on the animation
                animator.enabled = true;
                lol = 0;

                // Modify rigidbody to fix physic and collision boxs from glitching out
                while (foundRigidBodies.Length > lol)
                {
                    foundRigidBodies[lol].isKinematic = true;
                    foundRigidBodies[lol].detectCollisions = false;
                    lol++;
                }
                
            }

            // While the user didnt click the screen 5 times
            if (tap < 5)
            {
                tap++;

                //print out log for each tap on the screen
                Debug.Log("tap++");

                //Turn off the animation
                animator.enabled = false;
                lol = 0;

                // Modify rigidbody to fix model physic and collision boxs from glitching out
                while (foundRigidBodies.Length > lol)
                {
                    foundRigidBodies[lol].isKinematic = false;
                    foundRigidBodies[lol].detectCollisions = true;
                    lol++;
                }
                
                // Print out log to notify the user
                Debug.Log("grav init");

                // Set the force acting on the model and sent him flying in a random direction
                foundRigidBodies[UnityEngine.Random.Range(0, lol)].AddForce(new Vector3(UnityEngine.Random.Range(-2000.0f, 2000.0f), UnityEngine.Random.Range(-1000.0f, 2000.0f), UnityEngine.Random.Range(-2000.0f, 2000.0f)), ForceMode.Impulse);
                
            }

            // If the user clicks the screen 5 times reset the model position
            // The model plays the next animation.
            else
            {
                lol = 0;

                // Modify rigidbody to fix model physic and collision boxs from glitching out
                while (foundRigidBodies.Length > lol)
                {
                    foundRigidBodies[lol].isKinematic = false;
                    foundRigidBodies[lol].detectCollisions = true;
                    Debug.Log("lmao");
                    lol++;
                }

                // Turn on the model animation
                animator.enabled = true;

                // Play the get up animation
                animator.Play("get u");
                lol = 0;

                // Print out the log for collisions
                Debug.Log("collisions");

                // Modify rigidbody to fix model physic and collision boxs from glitching out
                while (foundRigidBodies.Length > lol)
                {
                    foundRigidBodies[lol].detectCollisions = false;
                    lol++;
                }

                // Set the function play time of the animation so it stop
                StartCoroutine(hit());

                // Turn on the animation
                animator.enabled = true;

                // Play the get up animation
                animator.Play("get u");
                StartCoroutine(hit());
                
                // Print out log for resetting
                Debug.Log("reset");
                tap = 0;
            }
        }
    }

    // hit
    // set wait time for action applied to the model
    IEnumerator hit()
    {
        // Identify the rigidbody and set a wait time for the calling action
        var foundRigidBodies = FindObjectsOfType<Rigidbody>();
        yield return new WaitForSeconds(10);

        // Print out log to notify the user
        Debug.Log("registered hit");
        lol = 0;
        Debug.Log("change");

        // Modify rigidbody to fix model physic and collision boxs from glitching out
        while (foundRigidBodies.Length > lol)
        {
            foundRigidBodies[lol].isKinematic = true;
            foundRigidBodies[lol].detectCollisions = false;
            lol++;
        }

        // Print out log for resetting the gravity
        Debug.Log("grav reset");
    }
}
