using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Transform target;
    [SerializeField] private float sensitivity = 200f;
    private float mouseX, mouseY;
    private Vector3 offSet = new Vector3(0, 4, 4);
    private Vector3 offSet2 = new Vector3(0, 4, -4);

    public Transform Target
    {
        get => target;
        set
        {
            target = value;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        if (Target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offSet + offSet2, .2f);
            if (Input.mousePosition.x > (Screen.width / 3)*2)
            {
                offSet = Quaternion.AngleAxis(mouseX, Vector3.up) * offSet2;
                offSet2 = Quaternion.AngleAxis(-mouseY, Vector3.right) * offSet;
                transform.LookAt(target);
            }
            //Vector3 angles = (Vector3.right * -mouseY) + (Vector3.up * mouseX);
            //Target.Rotate(Vector3.up * mouseX);
        }
        //if (Target != null)
        //{
        //    Vector3 angles = (Vector3.right * -mouseY) + (Vector3.up * mouseX);
        //    offSet = Quaternion.AngleAxis(mouseX, Vector3.up) * offSet2;
        //    offSet2 = Quaternion.AngleAxis(-mouseY, Vector3.right) * offSet;
        //    //transform.Rotate(angles);
        //    //transform.RotateAround(target.up,Vector3.up, mouseX);
        //    //transform.RotateAround(target.right,Vector3.right, -mouseY);
        //    transform.position = Vector3.Lerp(transform.position, target.position + offSet + offSet2, .5f);
        //    transform.LookAt(target);
        //    //Target.Rotate(Vector3.up * mouseX);
        //}
    }



}
