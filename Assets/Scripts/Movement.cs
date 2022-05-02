using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 12f, gravity = -9.81f, jumpHeight = 3f, groundDistance = .4f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Animator animator;
    private CharacterController cController;
    private Vector3 velocity;
    bool isGrounded, oneTimeLock = true;

    // Start is called before the first frame update
    void Start()
    {
        cController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics
            .CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal") * 2;
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (move.magnitude > 1)
            move /= move.magnitude;
        //burada optimizasyon
        cController.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;

        cController.Move(velocity * Time.deltaTime);

        if (Input.anyKey)
        {
            oneTimeLock = true;
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                animator.SetFloat("State", 4.1f, .1f, Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                animator.SetFloat("State", 5.9f, .1f, Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                animator.SetFloat("State", 5f, .1f, Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                animator.SetFloat("State", 0, .1f, Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                animator.SetFloat("State", 2f, .1f, Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetFloat("State", 1f, .1f, Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.Alpha1))
            {
                animator.SetFloat("State", 7f, .1f, Time.deltaTime);
            }
            else if(Input.GetKey(KeyCode.A))
            {
                animator.SetFloat("State", 4f, .1f, Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                animator.SetFloat("State", 6f, .1f, Time.deltaTime);
            }
        }
        else
        {
            animator.SetFloat("State", 3f, .1f, Time.deltaTime);

        }


    }
}
