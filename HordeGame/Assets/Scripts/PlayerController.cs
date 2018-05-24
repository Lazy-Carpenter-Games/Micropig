using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("The camera of the player")]
    [SerializeField] private GameObject playerCameraObject;

    [Tooltip("Tyhe animator of the player")]
    [SerializeField] private Animator playerAnimator;

    [Tooltip("The walk speed of the player")]
    [SerializeField] private float walkSpeed = 300f;

    [Tooltip("How quickly the player turns around")]
    [Range(0.001f, 1f)]
    [SerializeField] private float turnSpeed = 0.25f;

    private Rigidbody playerRigidbody;
    private float inputH;
    private float inputV;

    // Use this for initialization
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    /// <summary>
    /// Handles the player's movement
    /// </summary>
    private void PlayerMovement()
    {
        //Gets input from axis
        if(Input.GetAxis("Horizontal Movement") != 0f || Input.GetAxis("Vertical Movement") != 0f)
        {
            inputH = Input.GetAxisRaw("Horizontal Movement");
            inputV = Input.GetAxisRaw("Vertical Movement");
        }
        else
        {
            inputH = Input.GetAxis("Horizontal Movement Joystick");
            inputV = Input.GetAxis("Vertical Movement Joystick");
        }

        Vector3 targetDirection = new Vector3(inputH, 0f, inputV);
        targetDirection = playerCameraObject.transform.TransformDirection(targetDirection);
        targetDirection.y = 0f;

        if(inputH != 0f || inputV != 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed);
        }

        playerRigidbody.velocity = targetDirection * walkSpeed;
    }
}
