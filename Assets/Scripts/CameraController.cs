using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Transform target;
    [SerializeField] private float sensitivity = 200f;
    private float mouseX, mouseY;
    private Vector3 offSet = new Vector3(0, 3, -3);
    private Vector3 offSet2 = new Vector3(0, 3, -3);

    public Transform Target
    {
        get => target;
        set
        {
            target = value;
            transform.position = Vector3.Lerp(transform.position, target.position + offSet + offSet2, .2f);
        }
    }

    private void Update()
    {
        TouchListener();
    }

    private void TouchListener()
    {
        if (Target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offSet + offSet2, .2f);
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x > (Screen.width / 3) * 2)
                {
                    mouseX = touch.deltaPosition.x * sensitivity * Time.deltaTime;
                    mouseY = touch.deltaPosition.y * sensitivity * Time.deltaTime;


                    offSet = Quaternion.AngleAxis(mouseX, Vector3.up) * offSet2;
                    offSet2 = Quaternion.AngleAxis(-mouseY, Vector3.right) * offSet;

                    transform.LookAt(target.position + new Vector3(0,2,0));

                }

            }
        }




    }



}
