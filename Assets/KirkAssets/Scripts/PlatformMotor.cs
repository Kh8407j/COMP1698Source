// KHOGDEN 001115381
using audio;
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

        [Header("Collision Check Logic")]
        [SerializeField][Range(0.01f, 2f)] float checkRadius = 0.1f;
        [SerializeField][Range(2f, 10f)] float centerGap = 2f;
        [SerializeField] LayerMask groundLayers;
        private bool onGround, onRoof, onLeftWall, onRightWall;

        // KH - Received outputs/calculations from controller scripts.
        private float horOutput;
        private float verOutput;

        // KH - Movement and physics mechanics.
        private Vector2 moveDir;
        private float yVel;
        private bool jumpTrigger;
        private bool facingRight = true;
        private float gravity = 9.81f;

        // KH - Component references.
        private CapsuleCollider2D col;
        private Rigidbody2D rb;
        private SpriteRenderer sr;

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            col = GetComponent<CapsuleCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponentInChildren<SpriteRenderer>();
        }

        // KH - Called on a constant timeline.
        private void FixedUpdate()
        {
            Move();

            // KH - Make the motor jump if it's holding the correct input and is on ground.
            if (PressedJump() && onGround)
                Jump();
            else if (LiftedJump())
                jumpTrigger = false;

            // KH - Continously update collision detections booleans. Allows for seeing in debug mode within the inspector.
            onGround = OnGround();
            onRoof = OnRoof();
            onLeftWall = OnLeftWall();
            onRightWall = OnRightWall();

            // KH - Force gravity onto the motor when airborne. Stop once on ground again.
            if (!onGround)
                yVel -= gravity * Time.fixedDeltaTime;
            else if (yVel < 0f)
                HitGround();

            // KH - If the motor hits the roof while moving up, stop them moving upwards.
            if (onRoof && yVel > 0f)
                HitRoof();

            // KH - Make the motor's graphic sprite face towards the direction it's facing.
            if (facingRight)
                sr.flipX = false;
            else
                sr.flipX = true;
        }

        // KH - Surroundings collision debugging.
        #region
        // KH - Draw out visual gizmos in scene view. Suitable for debugging.
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                // KH - Calculate the collision check gizmos.
                Vector3 groundLeft = new Vector3(transform.position.x - (col.size.x / centerGap), transform.position.y - (col.size.y / 2f));
                Vector3 groundRight = new Vector3(transform.position.x + (col.size.x / centerGap), transform.position.y - (col.size.y / 2f));
                Vector3 roofLeft = new Vector3(transform.position.x - (col.size.x / centerGap), transform.position.y + (col.size.y / 2f));
                Vector3 roofRight = new Vector3(transform.position.x + (col.size.x / centerGap), transform.position.y + (col.size.y / 2f));

                Vector3 leftWallUp = new Vector3(transform.position.x - (col.size.x / 2f), transform.position.y + (col.size.y / centerGap));
                Vector3 leftWallDown = new Vector3(transform.position.x - (col.size.x / 2f), transform.position.y - (col.size.y / centerGap));
                Vector3 rightWallUp = new Vector3(transform.position.x + (col.size.x / 2f), transform.position.y + (col.size.y / centerGap));
                Vector3 rightWallDown = new Vector3(transform.position.x + (col.size.x / 2f), transform.position.y - (col.size.y / centerGap));

                // KH - Draw out the collision check gizmos to be seen in scene view.
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

        // KH - Make the motor move in the given output values.
        void Move()
        {
            // KH - Store the horizontal output for editing.
            float x = horOutput;

            // KH - Make the motor sprite face towards the direction it's about to move in.
            if (x < 0f)
                facingRight = false;
            else if (x > 0f)
                facingRight = true;

            // KH - Prevent the motor from moving the horizontal direction it's going if a wall is in front of it.
            if (!facingRight && onLeftWall || facingRight && onRightWall)
                x = 0f;

            // KH - Calculate where the motor is supposed to move.
            moveDir = new Vector2(x * moveSpeed, yVel);

            // KH - Make the motor move.
            transform.Translate(moveDir * Time.fixedDeltaTime);
        }

        // KH - Make the motor jump upwards.
        void Jump()
        {
            // KH - Play the jump sound.
            NextSoundAttributes soundAttributes = new NextSoundAttributes();
            AudioManager.control.PlayAudio("Jump", soundAttributes);

            // KH - Move the motor upwards, set 'jumpTrigger' to true to prevent repeatedly jumping while holding the input.
            jumpTrigger = true;
            yVel = jumpHeight;
        }

        // KH - Zero or reset all output values.
        public void ResetOutput()
        {
            horOutput = 0f;
            verOutput = 0f;
        }

        // KH - Called when the motor hits the ground.
        void HitGround()
        {
            // KH - Stop the motor from moving downwards.
            yVel = 0f;
        }

        // KH - Called when the motor hits the roof.
        void HitRoof()
        {
            // KH - Stop the motor from moving upwards.
            yVel = 0f;
        }

        // KH - Collision check logic methods.
        #region
        // KH - Check that the motor is on ground.
        bool OnGround()
        {
            // KH - Calculate the coordinates for where the motor's feet would be for detecting ground.
            Vector3 leftCoords = new Vector3(transform.position.x - (col.size.x / centerGap), transform.position.y - (col.size.y / 2f));
            Vector3 rightCoords = new Vector3(transform.position.x + (col.size.x / centerGap), transform.position.y - (col.size.y / 2f));

            // KH - Check that the calculated coordinates collide into colliders with the correct layers.
            bool leftGroundCheck = Physics2D.OverlapCircle(leftCoords, checkRadius, groundLayers);
            bool rightGroundCheck = Physics2D.OverlapCircle(rightCoords, checkRadius, groundLayers);

            // KH - Return the final calculations.
            return leftGroundCheck || rightGroundCheck;
        }

        // KH - Check that the motor is touching the roof.
        bool OnRoof()
        {
            // KH - Calculate the coordinates for where the motor's head would be for detecting roof.
            Vector3 leftCoords = new Vector3(transform.position.x - (col.size.x / centerGap), transform.position.y + (col.size.y / 2f));
            Vector3 rightCoords = new Vector3(transform.position.x + (col.size.x / centerGap), transform.position.y + (col.size.y / 2f));

            // KH - Check that the calculated coordinates collide into colliders with the correct layers.
            bool leftRoofCheck = Physics2D.OverlapCircle(leftCoords, checkRadius, groundLayers);
            bool rightRoofCheck = Physics2D.OverlapCircle(rightCoords, checkRadius, groundLayers);

            // KH - Return the final calculations.
            return leftRoofCheck || rightRoofCheck;
        }

        // KH - Check that the motor is touching a wall to it's left.
        bool OnLeftWall()
        {
            // KH - Calculate the coordinates for where the motor's left side would be for detecting walls on that side.
            Vector3 upCoords = new Vector3(transform.position.x - (col.size.x / 2f), transform.position.y + (col.size.y / centerGap));
            Vector3 downCoords = new Vector3(transform.position.x - (col.size.x / 2f), transform.position.y - (col.size.y / centerGap));

            // KH - Check that the calculated coordinates collide into colliders with the correct layers.
            bool upWallCheck = Physics2D.OverlapCircle(upCoords, checkRadius, groundLayers);
            bool downWallCheck = Physics2D.OverlapCircle(downCoords, checkRadius, groundLayers);

            // KH - Return the final calculations.
            return upWallCheck || downWallCheck;
        }

        // KH - Check that the motor is touching a wall to it's right.
        bool OnRightWall()
        {
            // KH - Calculate the coordinates for where the motor's right side would be for detecting walls on that side.
            Vector3 upCoords = new Vector3(transform.position.x + (col.size.x / 2f), transform.position.y + (col.size.y / centerGap));
            Vector3 downCoords = new Vector3(transform.position.x + (col.size.x / 2f), transform.position.y - (col.size.y / centerGap));

            // KH - Check that the calculated coordinates collide into colliders with the correct layers.
            bool upWallCheck = Physics2D.OverlapCircle(upCoords, checkRadius, groundLayers);
            bool downWallCheck = Physics2D.OverlapCircle(downCoords, checkRadius, groundLayers);

            // KH - Return the final calculations.
            return upWallCheck || downWallCheck;
        }

        // KH - Method to get the value of 'onGround'.
        public bool GetOnGround()
        {
            return onGround;
        }

        // KH - Method to get the value of 'onRoof'.
        public bool GetOnRoof()
        {
            return onRoof;
        }

        // KH - Method to get the value of 'onLeftWall'.
        public bool GetOnLeftWall()
        {
            return onLeftWall;
        }

        // KH - Method to get the value of 'onRightWall'.
        public bool GetOnRightWall()
        {
            return onRightWall;
        }
        #endregion

        // KH - Method for seeing if the motor is attempting to jump.
        bool PressedJump()
        {
            return verOutput > 0.5f && !jumpTrigger;
        }

        // KH - Method for seeing if the motor is no longer trying to jump.
        bool LiftedJump()
        {
            return verOutput < 0.5f;
        }

        // KH - Method to get the value of 'moveDir'.
        public Vector3 GetMoveDir()
        {
            return moveDir;
        }

        // KH - Method to get the value of 'facingRight'.
        public bool GetFacingRight()
        {
            return facingRight;
        }

        // KH - Method to set the value of 'gravity'.
        public void SetGravity(float newGravity)
        {
            gravity = newGravity;
        }

        // KH - Method to set the value of 'horOutput'.
        public void SetHorOutput(float output)
        {
            horOutput = output;
        }

        // KH - Method to set the value of 'verOutput'.
        public void SetVerOutput(float output)
        {
            verOutput = output;
        }
    }
}