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


    private CameraController cameraController;
    private Animator animator;
    private CharacterController cController;
    private Vector3 velocity;
    bool isGrounded, oneTimeLock = true;

    private Transform cameraTransform;
    private PlayerStates myStates = PlayerStates.Listening;

    public Animator Animator { get => animator; }

    void Start()
    {
        cameraController = CameraController.Instance;
        cameraController.Target = transform;
        cController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraTransform = cameraController.transform;
        variableJoystick = UIManager.Instance.VariableJoystick;
    }

    void FixedUpdate()
    {
        ClientSend.PlayerMovement(transform.position,transform.rotation,animator.GetInteger("State"));
        if (myStates == PlayerStates.Walking)
        {
            float heading = Mathf.Atan2(variableJoystick.Horizontal, variableJoystick.Vertical);
            transform.rotation = Quaternion.Euler(0f, cameraController.transform.eulerAngles.y + (heading * Mathf.Rad2Deg), 0f);
            cController.Move(transform.forward * speed * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        TouchListener();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }



    private void TouchListener()
    {

        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < Screen.width / 3 && touch.position.y < Screen.height / 2)
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

    }

    private void MouseDown()
    {
        animator.SetInteger("State", 1);
        //print("down");
        myStates = PlayerStates.Walking;
    }
    private void MouseHold()
    {
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
        ClientSend.DanceMusic(GetComponent<PlayerManager>().id);
    }
}
