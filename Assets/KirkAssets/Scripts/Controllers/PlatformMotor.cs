// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer
{
    public class PlatformMotor : MonoBehaviour
    {
        [Header("Motor Statistics")]
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float jumpHeight = 7f;
        [SerializeField] float gravity = 9.81f;

        // Received outputs/calculations from controller scripts.
        private float horOutput;
        private float verOutput;

        // Movement and physics mechanics.
        private Vector2 moveDir;
        private float yVel;

        // Component references.
        private CapsuleCollider2D col;
        private Rigidbody2D rb;
        private SpriteRenderer sr;

        // Called before 'void Start()'.
        private void Awake()
        {
            col = GetComponentInChildren<CapsuleCollider2D>();
            rb = GetComponentInChildren<Rigidbody2D>();
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
        }

        // Make the motor move in the given output values.
        void Move()
        {
            // Calculate where the motor is supposed to move.
            moveDir = new Vector2(horOutput * moveSpeed, yVel);

            // Make the motor sprite face towards the direction it's about to move in.
            if (moveDir.x < 0f)
                sr.flipX = true;
            else if(moveDir.x > 0f)
                sr.flipX = false;

            // Make the motor move.
            transform.Translate(moveDir * Time.fixedDeltaTime);
        }

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