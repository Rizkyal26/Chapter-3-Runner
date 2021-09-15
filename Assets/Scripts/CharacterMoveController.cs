using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    private Rigidbody2D rig;
    private Animator anim;
    private CharacterSoundController sound;
    private float lastPositionX;

    [Header("Jump")]
    public float jumpAccel;

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    [Header("Scoring")]
    public ScoreController Score;
    public float scoringRatio;

    private bool isJumping;
    private bool isOnGround;

    private void start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<CharacterSoundController>();

        lastPositionX = transform.position.x; 
    }
    // Start is called before the first frame update
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<CharacterSoundController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isOnGround)
            {
                isJumping = true;

                sound.PlayJump();
            }
        }

        //Change animation
        anim.SetBool("isOnGround", isOnGround);

        int distancepassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreincrement = Mathf.FloorToInt(distancepassed / scoringRatio);

        if (scoreincrement > 0)
        {
            Score.IncreaseCurrentScore(scoreincrement);
            lastPositionX += distancepassed; 
        }

    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if (hit)
        {
            if(!isOnGround && rig.velocity.y <=0)
            {
                isOnGround = true;
            }
        }
        else
        {
            isOnGround = false;
        }

        Vector2 velocityVector = rig.velocity;
        if (isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }
        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);

        rig.velocity = velocityVector;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }
}
