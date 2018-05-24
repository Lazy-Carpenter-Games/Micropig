using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("The transform of the player gameobject")]
    [SerializeField] private Transform playerTransform;

    [Tooltip("How attentively the camera follows the player")]
    [Range(0.01f, 1.0f)]
    [SerializeField] private float smoothFactor = 0.5f;

    [Tooltip("How quickly the camera rotates around the player")]
    [SerializeField] private float rotationSpeed = 5.0f;
    
    public bool LookAtPlayer = true;
    public bool RotateAroundPlayer = true;

    private Vector3 cameraOffset;

	// Use this for initialization
	private void Start ()
    {
        cameraOffset = transform.position - playerTransform.position;
	}
	
	// LateUpdate is called after Update
	private void LateUpdate ()
    {
        if(RotateAroundPlayer)
        {
            Quaternion camTurnXAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
            Quaternion camTurnYAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * rotationSpeed, Vector3.right);

            cameraOffset = camTurnXAngle * camTurnYAngle * cameraOffset;
        }

        Vector3 newPos = playerTransform.position + cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

        if(LookAtPlayer)
        {
            transform.LookAt(playerTransform);
        }
	}
}
