using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        int direction = 0;
        if (InputManager.GetKey("rotate_left"))
            direction = 1;
        if (InputManager.GetKey("rotate_right"))
            direction = -1;
       
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.y += direction * Time.deltaTime * rotationSpeed;
        transform.eulerAngles = currentRotation;
    }
}
