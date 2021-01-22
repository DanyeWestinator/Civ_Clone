using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public float moveSpeed = 1f;
    private Vector3 startPos;
    private Vector3 startRotation;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRotation = transform.GetChild(0).rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        MoveCamera();
        if (InputManager.GetKeyDown("home") || Input.GetKeyDown(KeyCode.Return))
            Reset();
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

    void MoveCamera()
    {
        int zMove = 0;
        int xMove = 0;
        if (InputManager.GetKey("up"))
            zMove = 1;
        if (InputManager.GetKey("down"))
            zMove = -1;
        if (InputManager.GetKey("left"))
            xMove = -1;
        if (InputManager.GetKey("right"))
            xMove = 1;
        Vector3 toMove = new Vector3(xMove, 0f, zMove);
        toMove *= Time.deltaTime * moveSpeed;
        transform.position = transform.position += toMove;
    }

    public void Reset()
    {
        transform.position = startPos;
        transform.GetChild(0).eulerAngles = startRotation;
        transform.eulerAngles = Vector3.zero;
    }
}
