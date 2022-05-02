using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float sensitivity = 200f;
    private float mouseX, mouseY;

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y + 3.5f, target.position.z + .3f), .5f);
    }
    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        target.Rotate(Vector3.up * mouseX);
        transform.Rotate(Vector3.right * -mouseY);
    }
}
