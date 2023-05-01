// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer
{
    public class Projectile : MonoBehaviour
    {
        [Header("Projectile Statistics")]
        [SerializeField] float damage = 10f;
        [SerializeField] float lifeTime = 5f;
        [SerializeField] float moveSpeedX = 5f;
        [SerializeField] float moveSpeedY;
        [SerializeField] bool facingRight = true;
        private Vector2 moveDir;

        [Header("Misc..")]
        [SerializeField] bool flipSpriteTowardsDirection = true;

        // KH - Component references.
        private SpriteRenderer sr;

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            sr = GetComponentInChildren<SpriteRenderer>();
        }

        // KH - Called upon every frame.
        void Update()
        {
            // KH - Continously reduce the life timer. Upon reaching zero the projectile destroy's itself.
            if (lifeTime > 0f)
                lifeTime -= Time.deltaTime;
            else
                DestroyProjectile();
        }

        // KH - Called on a constant timeline.
        private void FixedUpdate()
        {
            // KH - Make the projectile move in the inputted direction.
            transform.Translate(moveDir * Time.fixedDeltaTime);
        }

        // KH - Called when a collider enter's this gameobject's trigger.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Health health = collision.GetComponentInParent<Health>();

            // KH - Check that the hit object is damageable.
            if(health != null)
            {
                // KH - Destroy the projectile since it's now hit a target.
                DestroyProjectile();
            }
        }

        // KH - Destroy the projectile out of the scene.
        public virtual void DestroyProjectile()
        {
            // KH - Destroy the projectile.
            Destroy(gameObject);
        }

        // KH - Method to change the move direction of the projectile.
        public void SetMoveDir(Vector2 output)
        {
            moveDir = output;

            // KH - Check if the projectile's sprite should be facing the direction it's moving towards.
            if (flipSpriteTowardsDirection)
            {
                // KH - Make the projectile face the direction it's moving in.
                if (moveDir.x > 0f)
                    sr.flipX = false;
                else if (moveDir.x < 0f)
                    sr.flipX = true;
            }
        }
    }
}