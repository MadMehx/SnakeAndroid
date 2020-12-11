using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequiredComponent(typeof(Rigidbody))]


public class ClickAnim : MonoBehaviour
{

    private Animator animator;
    private Animation animation;
    int tap = 0;
    int lol;

    void Start()
    {
        Physics.gravity = new Vector3(0, -50, 0);
        GetComponent<Rigidbody>().inertiaTensor = GetComponent<Rigidbody>().inertiaTensor + new Vector3(0, 0, GetComponent<Rigidbody>().inertiaTensor.z * 100);
        //r = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var foundRigidBodies = FindObjectsOfType<Rigidbody>();

            Debug.Log("Pressed primary button.");

            if (animator == null)
            {
                animator = FindObjectOfType<Animator>();
            }
            if (tap == 0)
            {
                //Physics.gravity = new Vector3(0, 0, 0);
                animator.enabled = true;
                lol = 0;
                while (foundRigidBodies.Length > lol)
                {
                    foundRigidBodies[lol].isKinematic = true;
                    foundRigidBodies[lol].detectCollisions = false;
                    lol++;
                }
                
            }

            if (tap < 5)
            {
                tap++;
                Debug.Log("tap++");
                animator.enabled = false;
                lol = 0;
                while (foundRigidBodies.Length > lol)
                {
                    foundRigidBodies[lol].isKinematic = false;
                    foundRigidBodies[lol].detectCollisions = true;
                    lol++;
                }
                //Vector3 position = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
                //Speed = r.velocity.magnitude;
                //AngularSpeed = r.angularVelocity.magnitude;

                //r.Addforce(Vector3.forward);
                Debug.Log("grav init");

                
                foundRigidBodies[UnityEngine.Random.Range(0, lol)].AddForce(new Vector3(UnityEngine.Random.Range(-2000.0f, 2000.0f), UnityEngine.Random.Range(-1000.0f, 2000.0f), UnityEngine.Random.Range(-2000.0f, 2000.0f)), ForceMode.Impulse);
                //Physics.gravity = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-50.0f, 200.0f), Random.Range(-100.0f, 100.0f));
                
               
            }
            else
            {
                //Physics.gravity = new Vector3(0, 0, 0);
                lol = 0;
                while (foundRigidBodies.Length > lol)
                {
                    foundRigidBodies[lol].isKinematic = false;
                    foundRigidBodies[lol].detectCollisions = true;
                    Debug.Log("lmao");
                    lol++;
                }
<<<<<<< HEAD
<<<<<<< HEAD
                animator.enabled = true;
                animator.Play("get u");
                lol = 0;
                Debug.Log("collisions");
                while (foundRigidBodies.Length > lol)
                {
                    foundRigidBodies[lol].detectCollisions = false;
                    lol++;
                }
                StartCoroutine(hit());
               
=======
>>>>>>> parent of 4143f0e... Merge branch 'main' of https://github.com/MadMehx/SnakeAndroid into main
=======
                animator.enabled = true;
                animator.Play("get u");
                StartCoroutine(hit());
                
>>>>>>> parent of 0b7894b... Revert "Merge branch 'main' of https://github.com/MadMehx/SnakeAndroid into main"

                Debug.Log("reset");
                tap = 0;
            }
        }
    }

    IEnumerator hit()
    {
        var foundRigidBodies = FindObjectsOfType<Rigidbody>();
        yield return new WaitForSeconds(10);
        Debug.Log("registered hit");

        
        lol = 0;
        Debug.Log("change");
        while (foundRigidBodies.Length > lol)
        {
            foundRigidBodies[lol].isKinematic = true;
            foundRigidBodies[lol].detectCollisions = false;
            lol++;
        }
        //Physics.gravity = new Vector3(0, -100.0F, 0);
        Debug.Log("grav reset");
    }
}
