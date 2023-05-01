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

        [Header("Misc..")]
        [SerializeField][Range(0.01f, 2f)] float attackCooldownTime = 0.1f;
        private float attackCooldownTimer;

        // KH - Output values from controller scripts.
        private bool fire1Output;

        // KH - Collaborating scripts.
        private PlatformMotor motor;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // KH - Continously tick down the cooldown timer till it's at zero. Prevent going over negative.
            if(attackCooldownTimer > 0f)
                attackCooldownTimer -= Time.deltaTime;
            else if(attackCooldownTimer < 0f)
                attackCooldownTimer = 0f;
        }

        // KH - Fire a projectile.
        void Fire()
        {
            // Instantiate the projectile into the scene and setup a reference for it's mechanic script.
            GameObject projectileObj = Instantiate(projectilePrefab, firepoint.position, Quaternion.identity);
            Projectile projectileScript = projectileObj.GetComponent<Projectile>();

            //if(motor.GetFacingRight())

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
    }
}