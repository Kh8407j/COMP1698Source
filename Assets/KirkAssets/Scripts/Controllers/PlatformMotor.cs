// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace platformer
{
    public class PlatformMotor : MonoBehaviour
    {
        [Header("Motor Statistics")]
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float jumpHeight = 7f;
        [SerializeField] float gravity = 9.81f;

        [Header("Collision Check Logic")]
        [SerializeField][Range(0.01f, 2f)] float checkRadius = 0.1f;
        [SerializeField][Range(2f, 10f)] float centerGap = 2f;
        [SerializeField] LayerMask groundLayers;
        private bool onGround, onRoof, onLeftWall, onRightWall; // KH - Allows seeing in debug whether the motor is on ground, or touching roof.

        // Received outputs/calculations from controller scripts.
        private float horOutput;
        private float verOutput;

        // Movement and physics mechanics.
        private Vector2 moveDir;
        private float yVel;
        private bool jumpTrigger;
        private bool facingRight = true;

        // Component references.
        private CapsuleCollider2D col;
        private Rigidbody2D rb;
        private SpriteRenderer sr;

        // Called before 'void Start()'.
        private void Awake()
        {
            col = GetComponent<CapsuleCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponentInChildren<SpriteRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // Called on a constant timeline.
        private void FixedUpdate()
        {
            Move();
            
            // Continously update collision detections booleans. Allows for seeing in debug mode within the inspector.
            onGround = OnGround();
            onRoof = OnRoof();
            onLeftWall = OnLeftWall();
            onRightWall = OnRightWall();

            if(verOutput > 0.5f && onGround)
                Jump();

            // Force gravity onto the motor when airborne. Stop once on ground again.
            if (!onGround)
                yVel -= gravity * Time.fixedDeltaTime;
            else if (yVel < 0f)
                HitGround();

            // If the motor hits the roof while moving up, stop them moving upwards.
            if (onRoof && yVel > 0f)
                HitRoof();

            // Make the motor's graphic sprite face towards the direction it's facing.
            if(facingRight)
                sr.flipX = false;
            else
                sr.flipX = true;
        }

        // Surroundings collision debugging.
        #region
        // Draw out visual gizmos in scene view. Suitable for debugging.
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                // Calculate the ground check gizmos.
                Vector3 groundLeft = new Vector3(transform.position.x - (col.size.x / centerGap), transform.position.y - (col.size.y / 2f));
                Vector3 groundRight = new Vector3(transform.position.x + (col.size.x / centerGap), transform.position.y - (col.size.y / 2f));
                Vector3 roofLeft = new Vector3(transform.position.x - (col.size.x / centerGap), transform.position.y + (col.size.y / 2f));
                Vector3 roofRight = new Vector3(transform.position.x + (col.size.x / centerGap), transform.position.y + (col.size.y / 2f));

                Vector3 leftWallUp = new Vector3(transform.position.x - (col.size.x / 2f), transform.position.y + (col.size.y / centerGap));
                Vector3 leftWallDown = new Vector3(transform.position.x - (col.size.x / 2f), transform.position.y - (col.size.y / centerGap));
                Vector3 rightWallUp = new Vector3(transform.position.x + (col.size.x / 2f), transform.position.y + (col.size.y / centerGap));
                Vector3 rightWallDown = new Vector3(transform.position.x + (col.size.x / 2f), transform.position.y - (col.size.y / centerGap));

                // Draw out the ground check gizmos to be seen in scene view.
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(groundLeft, checkRadius);
                Gizmos.DrawSphere(groundRight, checkRadius);

                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(roofLeft, checkRadius);
                Gizmos.DrawSphere(roofRight, checkRadius);

                Gizmos.color = Color.green;
                Gizmos.DrawSphere(leftWallUp, checkRadius);
                Gizmos.DrawSphere(leftWallDown, checkRadius);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(rightWallUp, checkRadius);
                Gizmos.DrawSphere(rightWallDown, checkRadius);
            }
        }
        #endregion

        // Make the motor move in the given output values.
        void Move()
        {
            // Store the horizontal output for editing.
            float x = horOutput;

            // Make the motor sprite face towards the direction it's about to move in.
            if (x < 0f)
                facingRight = false;
            else if (x > 0f)
                facingRight = true;

            // Prevent the motor from moving the horizontal direction it's going if a wall is in front of it.
            if(!facingRight && onLeftWall || facingRight && onRightWall)
                x = 0f;

            // Calculate where the motor is supposed to move.
            moveDir = new Vector2(x * moveSpeed, yVel);

            // Make the motor move.
            transform.Translate(moveDir * Time.fixedDeltaTime);
        }

        // Make the motor jump upwards.
        void Jump()
        {
            // Make motor jump upwards. Set 'jumpTrigger' to true to prevent the motor from holding the jump input to repeatedly move up.
            jumpTrigger = true;
            yVel = jumpHeight;
        }

        // Called when the motor hits the ground.
        void HitGround()
        {
            // Stop the motor from moving downwards.
            yVel = 0f;
        }

        // Called when the motor hits the roof.
        void HitRoof()
        {
            // Stop the motor from moving upwards.
            yVel = 0f;
        }

        // Collision check logic methods.
        #region
        // Check that the motor is on ground.
        bool OnGround()
        {
            // Calculate the coordinates for where the motor's feet would be for detecting ground.
            Vector3 leftCoords = new Vector3(transform.position.x - (col.size.x / centerGap), transform.position.y - (col.size.y / 2f));
            Vector3 rightCoords = new Vector3(transform.position.x + (col.size.x / centerGap), transform.position.y - (col.size.y / 2f));

            // Check that the calculated coordinates collide into colliders with the correct layers.
            bool leftGroundCheck = Physics2D.OverlapCircle(leftCoords, checkRadius, groundLayers);
            bool rightGroundCheck = Physics2D.OverlapCircle(rightCoords, checkRadius, groundLayers);

            // Return the final calculations.
            return leftGroundCheck || rightGroundCheck;
        }

        // Check that the motor is touching the roof.
        bool OnRoof()
        {
            // Calculate the coordinates for where the motor's head would be for detecting roof.
            Vector3 leftCoords = new Vector3(transform.position.x - (col.size.x / centerGap), transform.position.y + (col.size.y / 2f));
            Vector3 rightCoords = new Vector3(transform.position.x + (col.size.x / centerGap), transform.position.y + (col.size.y / 2f));

            // Check that the calculated coordinates collide into colliders with the correct layers.
            bool leftRoofCheck = Physics2D.OverlapCircle(leftCoords, checkRadius, groundLayers);
            bool rightRoofCheck = Physics2D.OverlapCircle(rightCoords, checkRadius, groundLayers);

            // Return the final calculations.
            return leftRoofCheck || rightRoofCheck;
        }

        // Check that the motor is touching a wall to it's left.
        bool OnLeftWall()
        {
            // Calculate the coordinates for where the motor's left side would be for detecting walls on that side.
            Vector3 upCoords = new Vector3(transform.position.x - (col.size.x / 2f), transform.position.y + (col.size.y / centerGap));
            Vector3 downCoords = new Vector3(transform.position.x - (col.size.x / 2f), transform.position.y - (col.size.y / centerGap));

            // Check that the calculated coordinates collide into colliders with the correct layers.
            bool upWallCheck = Physics2D.OverlapCircle(upCoords, checkRadius, groundLayers);
            bool downWallCheck = Physics2D.OverlapCircle(downCoords, checkRadius, groundLayers);

            // Return the final calculations.
            return upWallCheck || downWallCheck;
        }

        // Check that the motor is touching a wall to it's right.
        bool OnRightWall()
        {
            // Calculate the coordinates for where the motor's right side would be for detecting walls on that side.
            Vector3 upCoords = new Vector3(transform.position.x + (col.size.x / 2f), transform.position.y + (col.size.y / centerGap));
            Vector3 downCoords = new Vector3(transform.position.x + (col.size.x / 2f), transform.position.y - (col.size.y / centerGap));

            // Check that the calculated coordinates collide into colliders with the correct layers.
            bool upWallCheck = Physics2D.OverlapCircle(upCoords, checkRadius, groundLayers);
            bool downWallCheck = Physics2D.OverlapCircle(downCoords, checkRadius, groundLayers);

            // Return the final calculations.
            return upWallCheck || downWallCheck;
        }
        #endregion

        // Method to set the value of 'horOutput'.
        public void SetHorOutput(float output)
        {
            horOutput = output;
        }

        // Method to set the value of 'verOutput'.
        public void SetVerOutput(float output)
        {
            verOutput = output;
        }
    }
}