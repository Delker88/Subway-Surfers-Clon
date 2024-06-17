using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { Left = -2, Middle = 0, Right = 2}
public class PlayerController : MonoBehaviour
{
    private Animator myAnimator;
    private Transform myTransform;
    private PlayerCollision playerCollision;
    private CharacterController _myCharacterController;
    public CharacterController MyCharacterController { get => _myCharacterController; set => _myCharacterController = value; }


    private Side position;
    private Vector3 motionVector;
    [Header ("Player Controller")]
    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _dogdeSpeed;
    public float ForwardSpeed { get => _forwardSpeed; set => _forwardSpeed = value; }
    public float JumpPower { get => _jumpPower; set => _jumpPower = value; }
    public float DogdeSpeed { get => _dogdeSpeed; set => _dogdeSpeed = value; }

    private float newXPosition;
    private float xPosition;
    private float yPosition;
    private float rollTimer;
    private int IdDogdeLeft = Animator.StringToHash("DodgeLeft");
    private int IdDogdeRight = Animator.StringToHash("DodgeRight");
    private int IdJump = Animator.StringToHash("Jump");
    private int IdFall = Animator.StringToHash("Fall");
    private int IdLanding = Animator.StringToHash("Landing");
    private int IdRoll = Animator.StringToHash("Roll");
    private int _IdStumbleLow = Animator.StringToHash("StumbleLow");
    public int IdStumbleLow { get => _IdStumbleLow; set => _IdStumbleLow = value; }

    private int _IdStumbleCornerLeft = Animator.StringToHash("StumbleCornerLeft");
    public int IdStumbleCornerLeft { get => _IdStumbleCornerLeft; set => _IdStumbleCornerLeft = value; }
    private int _IdStumbleCornerRight = Animator.StringToHash("StumbleCornerRight");
    public int IdStumbleCornerRight { get => _IdStumbleCornerRight; set => _IdStumbleCornerRight = value; }
    private int IdStumbleFall = Animator.StringToHash("StumbleFall");
    private int IdStumbleOffLeft = Animator.StringToHash("StumbleOffLeft");
    private int IdStumbleOffRight = Animator.StringToHash("StumbleOffRight");
    private int _IdStumbleSideLeft = Animator.StringToHash("StumbleSideLeft");
    public int IdStumbleSideLeft { get => _IdStumbleSideLeft; set => _IdStumbleSideLeft = value; }
    private int _IdStumbleSideRight = Animator.StringToHash("StumbleSideRight");
    public int IdStumbleSideRight { get => _IdStumbleSideRight; set => _IdStumbleSideRight = value; }
    private int _IdDeathBounce = Animator.StringToHash("DeathBounce");
    public int IdDeathBounce { get => _IdDeathBounce; set => _IdDeathBounce = value; }
    private int _IdDeathLower = Animator.StringToHash("DeathLower");
    public int IdDeathLower { get => _IdDeathLower; set => _IdDeathLower = value; }
    private int _IdDeathMovingTrain = Animator.StringToHash("DeathMovingTrain");
    public int IdDeathMovingTrain { get => _IdDeathMovingTrain; set => _IdDeathMovingTrain = value; }
    private int _IdDeathUpper = Animator.StringToHash("DeathUpper");
    public int IdDeathUpper { get => _IdDeathUpper; set => _IdDeathUpper = value; }

    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    [Header ("Player States")]
    [SerializeField] private bool _isRolling;
    public bool IsRolling { get => _isRolling; set => _isRolling = value; }
    public Side Position { get => position; set => position = value; }

    [SerializeField] private bool isJumping;
    [SerializeField] private bool isGrounded;

    private GameManager gameManager;
    private SceneController sceneController;
    private bool gameOn;
    private bool gameOf;
   

    void Start()
    {
        Position = Side.Middle;
        myTransform = GetComponent<Transform>();
        myAnimator = GetComponent<Animator>();
        _myCharacterController = GetComponent<CharacterController>();
        yPosition = -7;
        playerCollision = GetComponent<PlayerCollision>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
    }


    void Update()
    {
        gameOn = gameManager.GameOn;
        gameOf = gameManager.GameOff;
        myAnimator.SetBool("gameOn", gameOn);
        GetSwipe();
        SetPlayerPosition();
        MovePlayer();
        Jump();
        Roll();
        isGrounded = _myCharacterController.isGrounded;
    }
    private void GetSwipe()
    {
        if (!gameOf)
        {
            swipeLeft = Input.GetKeyDown(KeyCode.LeftArrow);
            swipeRight = Input.GetKeyDown(KeyCode.RightArrow);
        }
        if (gameOn)
        {
            swipeUp = Input.GetKeyDown(KeyCode.UpArrow);
            swipeDown = Input.GetKeyDown(KeyCode.DownArrow);
        }
    }

    public void SetPlayerCollisionPOsition()
    {
        if (Position == Side.Right)
        {
            UpdatePlayerPositionX(Side.Right);
        }    
        if (Position == Side.Middle)
        {
            UpdatePlayerPositionX(Side.Middle);
        }    
        if (Position == Side.Left)
        {
            UpdatePlayerPositionX(Side.Left);
        }
    }
    private void SetPlayerPosition()
    {
            if (swipeLeft && !_isRolling)
            {
                if (Position == Side.Middle)
                {
                    UpdatePlayerPositionX(Side.Left);
                    SetPlayerAnimator(IdDogdeLeft, false);
                }
                else if (Position == Side.Right)
                {
                    UpdatePlayerPositionX(Side.Middle);
                    SetPlayerAnimator(IdDogdeLeft, false);
                }
            }
            else if (swipeRight && !_isRolling)
            {
                if (Position == Side.Middle)
                {
                    UpdatePlayerPositionX(Side.Right);
                    SetPlayerAnimator(IdDogdeRight, false);
                }
                else if (Position == Side.Left)
                {
                    UpdatePlayerPositionX(Side.Middle);
                    SetPlayerAnimator(IdDogdeRight, false);
                }
            }
    }

    private void UpdatePlayerPositionX(Side plPosition)
    {
        newXPosition = (int)plPosition;
        Position = plPosition;
    }
    public void SetPlayerAnimator(int id, bool crossFade, float fadeTime = 0.1f)
    {
        myAnimator.SetLayerWeight(0, 1);
        if (crossFade)
        {
            myAnimator.CrossFadeInFixedTime(id, fadeTime);
        }
        else
        {
        myAnimator.Play(id);
        }
        ResetCollision();
    }

    public void SetPlayerAnimatorWithLayer(int id)
    {
        myAnimator.SetLayerWeight(1,1);
        myAnimator.Play(id);
        ResetCollision();
    }

    private void ResetCollision()
    {
        Debug.Log(playerCollision.CollisionX.ToString() + " " + playerCollision.CollisionY.ToString() + playerCollision.CollisionZ.ToString());
        playerCollision.CollisionX = CollisionX.None;
        playerCollision.CollisionY = CollisionY.None;
        playerCollision.CollisionZ = CollisionZ.None;
    }

    private void MovePlayer()
    {
            motionVector = new Vector3(xPosition - myTransform.position.x, yPosition * Time.deltaTime, _forwardSpeed * Time.deltaTime);
            xPosition = Mathf.Lerp(xPosition, newXPosition, Time.deltaTime * _dogdeSpeed);
            _myCharacterController.Move(motionVector); //Funcionalidad implementada en el character controller solo funciona con un vector 3 
    }

    private void Jump()
    {
        if (_myCharacterController.isGrounded)
        {
            isJumping = false;
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
               SetPlayerAnimator(IdLanding, false);
            if(swipeUp && !_isRolling)
            {
                isJumping = true;
                yPosition = _jumpPower;
                SetPlayerAnimator(IdJump, true);
            }
        }
        else
        {
            if (gameOn)
            {
                yPosition -= _jumpPower * 2 * Time.deltaTime;
                if (_myCharacterController.velocity.y <= 0)
                    SetPlayerAnimator(IdFall, false);
            }
           
        }
    }

    private void Roll()
    {
        rollTimer -= Time.deltaTime;
        if (rollTimer <= 0)
        {
            _isRolling = false;
            rollTimer = 0;
            _myCharacterController.center = new Vector3 (0, 0.45f, 0);
            _myCharacterController.height = 0.9f;
        }

        if (swipeDown && !isJumping)
        {
            _isRolling = true;
            rollTimer = 0.5f;
            _myCharacterController.center = new Vector3(0, 0.2f, 0);
            _myCharacterController.height = 0.4f;
            SetPlayerAnimator(IdRoll, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Spawn") && _myCharacterController.transform.position.z >= 1900)
        {
            print(myTransform.position.z);
            float spawnZ = GameObject.Find("spawn_start").transform.position.z;
            float playerX = _myCharacterController.transform.position.x;
            float playerY = _myCharacterController.transform.position.y;
            print("triguer funcionando");
            _myCharacterController.enabled = false;
            myTransform.position = new Vector3(playerX,playerY,spawnZ);
            _myCharacterController.enabled = true;
        }
    }
}
