// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer
{
    public class PlatformAttack : MonoBehaviour
    {
        [Header("Projectile")]
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] Transform firepoint;

        // KH - The velocity that each fired projectile will start off with.
        [SerializeField] float moveSpeedX = 5f;
        [SerializeField] float moveSpeedY;

        [Header("Misc..")]
        [SerializeField][Range(0.01f, 2f)] float attackCooldownTime = 0.1f;
        private float attackCooldownTimer;
        private bool attackTrigger;

        // KH - Output values from controller scripts.
        private bool fire1Output;

        // KH - Collaborating scripts.
        private PlatformMotor motor;

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            motor = GetComponent<PlatformMotor>();
        }

        // Update is called once per frame
        void Update()
        {
            // KH - If the fire input is held, fire a projectile.
            if (PressedAttack() && ReadyToAttack())
                Fire();
            else if(LiftedAttack())
                attackTrigger = false;

            // KH - Continously tick down the cooldown timer till it's at zero. Prevent going over negative.
            if(attackCooldownTimer > 0f)
                attackCooldownTimer -= Time.deltaTime;
            else if(attackCooldownTimer < 0f)
                attackCooldownTimer = 0f;
        }

        // KH - Fire a projectile.
        void Fire()
        {
            // KH - Reset the attack cooldown timer to prevent this void being so much in a short time span.
            attackCooldownTimer = attackCooldownTime;

            // KH - Instantiate the projectile into the scene and setup a reference for it's mechanic script.
            GameObject projectileObj = Instantiate(projectilePrefab, firepoint.position, Quaternion.identity);
            Projectile projectileScript = projectileObj.GetComponent<Projectile>();

            // KH - Make the projectile face and move towards the same direction the motor is facing.
            projectileScript.SetMoveDir(new Vector2(moveSpeedX, moveSpeedY));
            projectileScript.SetFacingRight(motor.GetFacingRight());

            // KH - Set 'attackTrigger' to true to make sure the output cannot be held to repeatedly fire.
            attackTrigger = true;
        }

        // KH - Method to see if attacking is ready.
        bool ReadyToAttack()
        {
            return attackCooldownTimer == 0f;
        }

        // KH - Method to set the value of 'fire1Output'.
        public void SetFire1Output(bool output)
        {
            fire1Output = output;
        }

        // KH - Method to see if the attack trigger has been pressed.
        bool PressedAttack()
        {
            return fire1Output && !attackTrigger;
        }

        // KH - Method to see if the attack trigger has been let go.
        bool LiftedAttack()
        {
            return !fire1Output;
        }
    }
}