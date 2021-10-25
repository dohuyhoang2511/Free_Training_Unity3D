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
    private float newTranformX;
    private float newTranformY;
    private float colHeight;
    private float colCenterY;
    private float startPositionZ;
    private bool gameOver;

#if UNITY_ANDROID
    //Used to store locaiton of screen touch origin for mobile controls.
    private Vector2 touchOrigin = -Vector2.one;
    int horizontal = 0;
    int vertical = 0;
#endif

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        colHeight = characterController.height;
        colCenterY = characterController.center.y;

        transform.position = new Vector3(0, 1, 1);

        startPositionZ = transform.position.z;
        runDistance = transform.position.z - startPositionZ;

#if UNITY_ANDROID
        horizontal = 0;
        vertical = 0;
#endif
    }
#if !(UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL)
    internal float slideCounter;
#endif
    // Update is called once per frame
    void Update()
    {
        // https://learn.unity.com/tutorial/architecture-and-polish?projectId=5c514a00edbc2a0020694718#5c7f8528edbc2a002053b6ef

#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Running Slide"))
        {
            MoveLeft();
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Running Slide"))
        {
            MoveRight();
        }

#else

        //Check if Input has registered more than zero touches
        if (Input.touchCount > 0)
        {
            //Store the first touch detected.
            Touch myTouch = Input.touches[0];

            //Check if the phase of that touch equals Began
            if (myTouch.phase == TouchPhase.Began)
            {
                //If so, set touchOrigin to the position of that touch
                touchOrigin = myTouch.position;
            }

            //If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
            else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
                //Set touchEnd to equal the position of this touch
                Vector2 touchEnd = myTouch.position;

                //Calculate the difference between the beginning and end of the touch on the x axis.
                float x = touchEnd.x - touchOrigin.x;

                //Calculate the difference between the beginning and end of the touch on the y axis.
                float y = touchEnd.y - touchOrigin.y;

                //Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
                touchOrigin.x = -1;

                //Check if the difference along the x axis is greater than the difference along the y axis.
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    //If x is greater than zero, set horizontal to 1, otherwise set it to -1
                    horizontal = x > 0 ? 1 : -1;
                }
                else
                {
                    //If y is greater than zero, set horizontal to 1, otherwise set it to -1
                    vertical = y > 0 ? 1 : -1;
                }

                //Check Move Left Right
                if(horizontal == 1)
                {
                    MoveRight();
                }
                else if(horizontal == -1)
                {
                    MoveLeft();
                }

                //Check Jump
                if (characterController.isGrounded)
                {
                    if (vertical == 1)
                    {
                        newTranformY = jumpForce;

                        anim.CrossFadeInFixedTime("Jump", 0.1f);
                    }
                }
                else
                {
                    newTranformY -= jumpForce * 3 * Time.deltaTime;
                }

                //Check Slide
                

                slideCounter -= Time.deltaTime;

                if (slideCounter <= 0f)
                {
                    slideCounter = 0f;

                    characterController.center = new Vector3(0, colCenterY, 0);
                    characterController.height = colHeight;

                    inSlide = false;
                }

                if (vertical == -1)
                {
                    slideCounter = 1.2f;

                    characterController.center = new Vector3(0, (colCenterY / 2f) - 0.2f, 0);
                    characterController.height = colHeight / 2f;

                    anim.CrossFadeInFixedTime("Running Slide", 0.1f);
                    inSlide = true;
                }
            }

            horizontal = 0;
            vertical = 0;
        }

#endif
        newTranformX = Mathf.Lerp(newTranformX, newXPos, speedDodge * Time.deltaTime);

        Vector3 moveVector = new Vector3(newTranformX - transform.position.x, newTranformY * Time.deltaTime, moveSpeed * Time.deltaTime);

        characterController.Move(moveVector);

#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL

        Jump();
        Slide();

#endif

        RunDistance();

        if (!gameOver)
        {
            moveSpeed += 0.005f;
        }
    }

    void MoveLeft()
    {
        if (side == SIDE.MID)
        {
            newXPos = -xValue + 0.15f;
            side = SIDE.LEFT;
            anim.Play("Left Strafe");
        }
        else if (side == SIDE.RIGHT)
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

    void MoveRight()
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


#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL
    void Jump()
    {
        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                newTranformY = jumpForce;

                anim.CrossFadeInFixedTime("Jump", 0.1f);
            }
        }
        else
        {
            newTranformY -= jumpForce * 3 * Time.deltaTime;
        }
    }

    internal float slideCounter;
    void Slide()
    {
        slideCounter -= Time.deltaTime;

        if(slideCounter <= 0f)
        {
            slideCounter = 0f;

            characterController.center = new Vector3(0, colCenterY, 0);
            characterController.height = colHeight;

            inSlide = false;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            slideCounter = 1.2f;

            characterController.center = new Vector3(0, (colCenterY / 2f) - 0.2f, 0);
            characterController.height = colHeight / 2f;

            anim.CrossFadeInFixedTime("Running Slide", 0.1f);
            inSlide = true;
        }
    }
#endif


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "StumbleBackwards")
        {
            hit.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 100);

            gameOver = true;

            StopRun();

            anim.Play("Stumble Backwards");

            Invoke("GameOver", 0.7f);
        }
        if (hit.gameObject.tag == "HitToSideOfBody")
        {
            hit.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 50);

            gameOver = true;

            StopRun();

            anim.Play("Hit To Side Of Body");

            Invoke("GameOver", 1.7f);
        }
    }

    public float RunDistance()
    {
        runDistance = transform.position.z - startPositionZ;
        return runDistance;
    }

    void GameOver()
    {
        GameManager.Instance.GameOver();
    }

    void StopRun()
    {
        moveSpeed = 0;
    }

    public void PlayAnimationIdle()
    {
        anim.Play("Idle");
    }

    public void PlayAnimationRun()
    {
        anim.Play("Standard Run");
    }
}
