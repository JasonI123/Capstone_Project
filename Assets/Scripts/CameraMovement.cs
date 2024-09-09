using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject mover;
    public GameObject rotator;
    public GameObject subMover;
    public float multiplier = 100;
    public float zoomMultiplier = 100;
    public float rotMult = 5;
    public float shiftRotMult = 2.5f;
    public float shiftMult = 10;

    [SerializeField]
    private float maxX;
    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxY;
    [SerializeField]
    private float minY;
    [SerializeField]
    private float maxZ;
    [SerializeField]
    private float minZ;

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Temporarily set camera x axis rotation to 0.
        float originalXrot = rotator.transform.rotation.x;
        Quaternion rot = rotator.transform.rotation;
        rot.x = 0;
        rotator.transform.rotation = rot;

        // Rotation
        float rotation = 0;
        if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
        {
            rotation = -1;
        }
        else if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q))
        {
            rotation = 1;
        }
        rotation *= Time.deltaTime * rotMult;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            rotation *= shiftRotMult;


        // Forward and back movement
        float forwardBackInput = Input.GetAxis("Horizontal");
        forwardBackInput *= Time.deltaTime * multiplier;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            forwardBackInput *= shiftMult;

        // Left and right movement
        float leftRightInput = Input.GetAxis("Vertical");
        leftRightInput *= Time.deltaTime * multiplier;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            leftRightInput *= shiftMult;

        // Up and Down movement
        float upDownInput = Input.GetAxis("UpDown");
        upDownInput *= Time.deltaTime * zoomMultiplier;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            upDownInput *= shiftMult;

        // Set camera position and rotation
        mover.transform.position = rotator.transform.position;
        mover.transform.rotation = rotator.transform.rotation;
        subMover.transform.localPosition = Vector3.zero;
        Vector3 mov = subMover.transform.localPosition;
        mov.x += forwardBackInput;
        mov.z += leftRightInput;
        mov.y += upDownInput;
        subMover.transform.localPosition = mov;
        rotator.transform.position = subMover.transform.position;

        // Keep camera in bounds
        Vector3 t = rotator.transform.position;
        if (t.x < minX)
            t.x = minX;
        else if (t.x > maxX)
            t.x = maxX;

        if (t.y < minY)
            t.y = minY;
        else if (t.y > maxY)
            t.y = maxY;

        if (t.z < minZ)
            t.z = minZ;
        else if (t.z > maxZ)
            t.z = maxZ;
        rotator.transform.position = t;

        // Set x rotation back to normal
        rot.x = originalXrot;
        rotator.transform.rotation = rot;
        rotator.transform.Rotate(0, rotation, 0);

    }
}
