using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("The camera of the player")]
    [SerializeField] private GameObject playerCameraObject;

    [Tooltip("Tyhe animator of the player")]
    [SerializeField] private Animator playerAnimator;

    [Tooltip("How fast or slow the player can look around")]
    [SerializeField] private float lookSpeed = 10f;

    [Tooltip("The walk speed of the player")]
    [SerializeField] private float walkSpeed = 300f;

    private Rigidbody playerRigidbody;
    private RaycastHit groundHit;
    private float maxVelocityChange = 10.0f;
    private float currentVelocitySpeed = 0f;
    private float inputH;
    private float inputV;
    private bool isCrouched = false;

    // Use this for initialization
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        PlayerLook();
        PlayerMovement();
    }

    /// <summary>
    /// Controls the player's ability to look around
    /// </summary>
    private void PlayerLook()
    {
        //Horizontal
        if (Input.GetAxisRaw("Mouse X") > 0 || Input.GetAxisRaw("Mouse X") < 0)
        {
            Vector3 playerHorizontalRotation = new Vector3(0f, Input.GetAxisRaw("Mouse X") * lookSpeed * Time.deltaTime, 0f);

            this.gameObject.transform.Rotate(playerHorizontalRotation);
        }

        //Vertical
        if (Input.GetAxisRaw("Mouse Y") > 0 || Input.GetAxisRaw("Mouse Y") < 0)
        {
            Vector3 playerVerticalRotation = new Vector3(Input.GetAxisRaw("Mouse Y") * lookSpeed * Time.deltaTime, 0f, 0f);

            playerCameraObject.transform.Rotate(playerVerticalRotation);
        }
    }

    /// <summary>
    /// Handles the player's movement
    /// </summary>
    private void PlayerMovement()
    {
        //Gets input from axis
        inputH = Input.GetAxisRaw("Horizontal Movement");
        inputV = Input.GetAxisRaw("Vertical Movement");

        //Calculate how fast we should be moving
        Vector3 targetVelocity = new Vector3(inputH, 0f, inputV);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= walkSpeed;

        //Apply a force that attempts to reach our target velocity
        Vector3 velocity = playerRigidbody.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0f;
        playerRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        
        Crouch();

        //Handle locomotive animations
        playerAnimator.SetFloat("InputH", inputH);
        playerAnimator.SetFloat("InputV", inputV);
    }

    private void Crouch()
    {
        if(!isCrouched && Input.GetButtonDown("Crouch"))
        {
            playerAnimator.SetBool("isCrouched", true);
            isCrouched = true;
            walkSpeed /= 2f;
            GetComponent<CapsuleCollider>().height /= 1.5f;
            float colliderCenterY = GetComponent<CapsuleCollider>().center.y;
            Vector3 newCenter = new Vector3(0f, colliderCenterY / 1.5f, 0f);
            GetComponent<CapsuleCollider>().center = newCenter;
        }
        else if(isCrouched && Input.GetButtonDown("Crouch"))
        {
            playerAnimator.SetBool("isCrouched", false);
            isCrouched = false;
            walkSpeed *= 2f;
            GetComponent<CapsuleCollider>().height *= 1.5f;
            float colliderCenterY = GetComponent<CapsuleCollider>().center.y;
            Vector3 newCenter = new Vector3(0f, colliderCenterY * 1.5f, 0f);
            GetComponent<CapsuleCollider>().center = newCenter;
        }
    }
}
