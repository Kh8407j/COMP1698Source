// KHOGDEN 001115381
using audio;
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

        [Header("Animation")]
        [SerializeField] string attackAnimation;
        [SerializeField] string resetAnimation;

        // KH - The velocity that each fired projectile will start off with.
        [SerializeField] float moveSpeedX = 5f;
        [SerializeField] float moveSpeedY;

        [Header("Misc..")]
        [SerializeField][Range(0.01f, 2f)] float attackCooldownTime = 0.1f;
        private float attackCooldownTimer;
        private bool attackTrigger;

        // KH - Output values from controller scripts.
        private int fire1Output;

        // KH - Collaborating scripts and component references.
        private PlatformMotor motor;
        private Animator anim;

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            motor = GetComponent<PlatformMotor>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            // KH - If the fire input is held, perform attack animation. Animation is expected to have an event function.
            if (PressedAttack() && ReadyToAttack())
                anim.Play(attackAnimation);
            else if (LiftedAttack())
                attackTrigger = false;

            // KH - Continously tick down the cooldown timer till it's at zero. Prevent going over negative.
            if(attackCooldownTimer > 0f)
                attackCooldownTimer -= Time.deltaTime;
            else if(attackCooldownTimer < 0f)
                attackCooldownTimer = 0f;
        }

        // KH - Fire a projectile.
        public void Fire()
        {
            // KH - Reset the attack cooldown timer to prevent this void being so much in a short time span.
            attackCooldownTimer = attackCooldownTime;

            // KH - Instantiate the projectile into the scene and setup a reference for it's mechanic script.
            GameObject projectileObj = Instantiate(projectilePrefab, firepoint.position, Quaternion.identity);
            Projectile projectileScript = projectileObj.GetComponent<Projectile>();

            // KH - Make the projectile face and move towards the same direction the motor is facing.
            projectileScript.SetFacingRight(motor.GetFacingRight());
            projectileScript.SetMoveDir(new Vector2(moveSpeedX, moveSpeedY));

            // KH - Play the shoot sound.
            NextSoundAttributes soundAttributes = new NextSoundAttributes();
            AudioManager.control.PlayAudio("Shoot", soundAttributes);

            // KH - Set 'attackTrigger' to true to make sure the output cannot be held to repeatedly fire.
            attackTrigger = true;
        }

        // KH - Reset the animator to play the inputted animation in 'resetAnimation'.
        public void ResetAnimation()
        {
            anim.Play(resetAnimation);
        }

        // KH - Method to see if attacking is ready.
        bool ReadyToAttack()
        {
            return attackCooldownTimer == 0f;
        }

        // KH - Method to set the value of 'fire1Output'.
        public void SetFire1Output(int output)
        {
            fire1Output = output;
        }

        // KH - Method to see if the attack trigger has been pressed.
        bool PressedAttack()
        {
            return fire1Output == 1 && !attackTrigger;
        }

        // KH - Method to see if the attack trigger has been let go.
        bool LiftedAttack()
        {
            return fire1Output == 0;
        }
    }
}