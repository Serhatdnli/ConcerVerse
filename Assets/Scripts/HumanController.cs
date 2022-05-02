using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    private Rigidbody rb;
    private bool vertical, horizontal;
    private int vValue, hValue;
    private Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            vertical = true;
            vValue = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            vertical = true;
            vValue = -1;
        }
        else
        {
            vertical = false;
            vValue = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            horizontal = true;
            hValue = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontal = true;
            hValue = 1;
        }
        else
        {
            horizontal = false;
            hValue = 0;
        }
    }

    private void FixedUpdate()
    {
        if (vertical)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            rb.AddForce(transform.forward * vValue * 100f);
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        }


        if (horizontal)
        {
            rb.velocity = new Vector3(0, 0, rb.velocity.z);
            rb.AddForce(transform.right * hValue * 100f);

        }
        else
        {
            rb.velocity = new Vector3(0, 0, rb.velocity.z);
        }

        if (vertical || horizontal)
        {
            animator.SetInteger("State", 1);
            print("Girdi");
        }
        else
        {
            animator.SetInteger("State", 0);
            print("çıktı");
        }

    }


}
