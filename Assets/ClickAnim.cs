using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequiredComponent(typeof(Rigidbody))]


public class ClickAnim : MonoBehaviour
{

    private Animator animator;
    private Animation animation;
    int tap = 0;
    protected Rigidbody r;
    public float Speed;
    public float AngularSpeed;


    void Start()
    {
        Physics.gravity = new Vector3(0, -100.0F, 0);
        //r = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed primary button.");

            if (animator == null)
            {
                animator = FindObjectOfType<Animator>();
            }
            if (tap == 0)
            {
                animator.enabled = true;
            }

            if (tap < 5)
            {
                tap++;
                Debug.Log("tap++");
                animator.enabled = false;
                //Vector3 position = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
                //Speed = r.velocity.magnitude;
                //AngularSpeed = r.angularVelocity.magnitude;

                //r.Addforce(Vector3.forward);
                Physics.gravity = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
            }
            else
            {
                animator.Play("get u");
                animator.enabled = true;
                
                Debug.Log("reset");
                tap = 0;
            }
        }
    }
}
