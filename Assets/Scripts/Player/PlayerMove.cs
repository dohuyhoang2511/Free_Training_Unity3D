using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SIDE { LEFTMOST, LEFT, MID, RIGHT, RIGHTMOST }
public class PlayerMove : MonoBehaviour
{
    public SIDE side = SIDE.MID;
    public float xValue;
    public float speedDodge;
    public float jumpForce = 7f;
    public float moveSpeed;
    [HideInInspector] public bool inSlide;
    public float runDistance;

    private float newXPos = 0f;
    private CharacterController characterController;
    private Animator anim;
    private float x;
    private float y;
    private float colHeight;
    private float colCenterY;
    private float startPositionZ;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        colHeight = characterController.height;
        colCenterY = characterController.center.y;

        anim = GetComponentInChildren<Animator>();

        transform.position = new Vector3(0, 1, 1);

        startPositionZ = transform.position.z;
        runDistance = transform.position.z - startPositionZ;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Running Slide"))
        {
            if(side == SIDE.MID)
            {
                newXPos = -xValue + 0.15f;
                side = SIDE.LEFT;
                anim.Play("Left Strafe");
            }
            else if(side == SIDE.RIGHT)
            {
                newXPos = 0f;
                side = SIDE.MID;
                anim.Play("Left Strafe");
            }
            else if (side == SIDE.LEFT)
            {
                newXPos = (-2 * xValue) + 0.2f;
                side = SIDE.LEFTMOST;
                anim.Play("Left Strafe");
            }
            else if (side == SIDE.RIGHTMOST)
            {
                newXPos = xValue;
                side = SIDE.RIGHT;
                anim.Play("Left Strafe");
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Running Slide"))
        {
            if (side == SIDE.MID)
            {
                newXPos = xValue;
                side = SIDE.RIGHT;
                anim.Play("Right Strafe");
            }
            else if (side == SIDE.LEFT)
            {
                newXPos = 0f;
                side = SIDE.MID;
                anim.Play("Right Strafe");
            }
            else if (side == SIDE.RIGHT)
            {
                newXPos = 2 * xValue;
                side = SIDE.RIGHTMOST;
                anim.Play("Right Strafe");
            }
            else if (side == SIDE.LEFTMOST)
            {
                newXPos = -xValue + 0.15f;
                side = SIDE.LEFT;
                anim.Play("Right Strafe");
            }
        }

        x = Mathf.Lerp(x, newXPos, speedDodge * Time.deltaTime);

        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, moveSpeed * Time.deltaTime);

        characterController.Move(moveVector);

        Jump();
        Slide();

        RunDistance();
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                y = jumpForce;

                anim.CrossFadeInFixedTime("Jump", 0.1f);
            }
        }
        else
        {
            y -= jumpForce * 3 * Time.deltaTime;
        }
    }

    internal float slideCounter;
    public void Slide()
    {
        slideCounter -= Time.deltaTime;

        if(slideCounter <= 0f)
        {
            slideCounter = 0f;

            characterController.center = new Vector3(0, colCenterY, 0);
            characterController.height = colHeight;

            inSlide = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            slideCounter = 1.2f;

            characterController.center = new Vector3(0, (colCenterY / 2f) - 0.2f, 0);
            characterController.height = colHeight / 2f;

            anim.CrossFadeInFixedTime("Running Slide", 0.1f);
            inSlide = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "StumbleBackwards")
        {
            Invoke("Stop", 0.5f);
            anim.Play("Stumble Backwards");
            hit.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * moveSpeed / 1.5f);
        }
        if (hit.gameObject.tag == "HitToSideOfBody")
        {
            Invoke("Stop", 0.5f);
            anim.Play("Hit To Side Of Body");
            hit.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * moveSpeed);
        }
    }

    void Stop()
    {
        moveSpeed = 0f;
    }

    public float RunDistance()
    {
        runDistance = transform.position.z - startPositionZ;
        return runDistance;
    }
}
