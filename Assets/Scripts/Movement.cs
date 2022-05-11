using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;

public class Movement : Singleton<Movement>
{
    [SerializeField] private float speed = 12f, gravity = -9.81f, jumpHeight = 3f, groundDistance = .4f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private VariableJoystick variableJoystick;

    private Animator animator;
    private CharacterController cController;
    private Vector3 velocity;
    bool isGrounded, oneTimeLock = true;

    private Transform cameraTransform;
    private PlayerStates myStates = PlayerStates.Listening;

    public Animator Animator { get => animator; }

    void Start()
    {
        CameraController.Instance.Target = transform;
        cController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraTransform = CameraController.Instance.transform;
        variableJoystick = UIManager.Instance.VariableJoystick;
    }

    void FixedUpdate()
    {

        if (myStates == PlayerStates.Walking)
        {
            float heading = Mathf.Atan2(variableJoystick.Horizontal, variableJoystick.Vertical);
            transform.rotation = Quaternion.Euler(0f, heading * Mathf.Rad2Deg, 0f);
            cController.Move(transform.forward * speed * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    private void LateUpdate()
    {
        TouchListener();
    }



    private void TouchListener()
    {
        if (Input.mousePosition.x < Screen.width / 3 && Input.mousePosition.y < Screen.height / 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseDown();
            }
            if (Input.GetMouseButton(0))
            {
                MouseHold();
            }
            if (Input.GetMouseButtonUp(0))
            {
                MouseUp();
            }
        }
    }

    //serverden gelen animasyon bizim anlık animasyonumuzu bozuyo ondan dolayı düzgün çalışmıyor. fixle
    private void MouseDown()
    {
        animator.SetInteger("State", 1);
        //print("down");
        myStates = PlayerStates.Walking;
    }
    private void MouseHold()
    {
        //print("Mouse position : " + Input.mousePosition + "  Width : " + Screen.width + " Height : " + Screen.height);
    }
    private void MouseUp()
    {
        animator.SetInteger("State", 0);
        //print("up");
        myStates = PlayerStates.Listening;
    }

    public void Dance()
    {
        animator.SetInteger("State", 2);
    }
}
