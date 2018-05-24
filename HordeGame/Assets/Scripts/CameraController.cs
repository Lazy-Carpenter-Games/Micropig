using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("The transform of the player gameobject")]
    [SerializeField] private Transform playerTransform;

    [Tooltip("How quickly the camera rotates around the player")]
    [SerializeField] private float rotationSpeed = 10f;

    [Tooltip("How quickly the camera rotates around the player using the joystick")]
    [SerializeField] private float joystickRotationSpeed = 150f;

    [Tooltip("How slowly the camera follows the player's input")]
    [Range(0.001f, 1f)]
    [SerializeField] private float cameraLag = 0.25f;
    
    private Vector3 cameraOffset;
    private float rotYAxis;
    private float rotXAxis;
    private float distance;

	// Use this for initialization
	private void Start ()
    {
        cameraOffset = transform.position - playerTransform.position;

        rotYAxis = transform.eulerAngles.y;
        rotXAxis = transform.eulerAngles.x;

        distance = Vector3.Magnitude(cameraOffset);
	}
	
	// LateUpdate is called after Update
	private void LateUpdate ()
    {
        Orbit();
	}

    private void Orbit()
    {
        if(Input.GetAxis("Mouse X") > 0f || Input.GetAxis("Mouse X") < 0f || 
            Input.GetAxis("Mouse Y") > 0f || Input.GetAxis("Mouse Y") < 0f)
        {
            rotYAxis += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            rotXAxis += Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        }
        else if(Input.GetAxis("Joystick X") > 0f || Input.GetAxis("Joystick X") < 0f || 
            Input.GetAxis("Joystick Y") > 0f || Input.GetAxis("Joystick Y") < 0f)
        {
            rotYAxis += Input.GetAxis("Joystick X") * joystickRotationSpeed * Time.deltaTime;
            rotXAxis += Input.GetAxis("Joystick Y") * joystickRotationSpeed * Time.deltaTime;
        }

        rotXAxis = Mathf.Clamp(rotXAxis, -90f, 90f);

        Quaternion toRotation = Quaternion.Euler(rotXAxis, rotYAxis, 0f);
        Quaternion rotation = toRotation;

        Vector3 negDistance = new Vector3(0f, 0f, -distance);
        Vector3 position = rotation * negDistance + playerTransform.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, cameraLag);
        transform.position = Vector3.Slerp(transform.position, position, cameraLag);
    }

}
