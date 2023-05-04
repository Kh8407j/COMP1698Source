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
        [SerializeField] bool facingRight = true;
        private float xScaleAbs;
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
        void FixedUpdate()
        {
            Move();
        }

        // KH - Called when a collider enter's this gameobject's trigger.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // AO - Checks if collider is NOT player type
            if (collision.gameObject.CompareTag("Player"))
            {
                //DO NOTHING
            }
            else
            {
                Health health = collision.GetComponentInParent<Health>();

                // KH - Check that the hit object is damageable.
                if (health != null)
                {
                    // KH - Damage the damagable object. Destroy the projectile since it's now hit a target.
                    health.ChangeHealth(-damage);
                    DestroyProjectile();
                }
            }
        }

        // KH - Make the projectile move.
        public virtual void Move()
        {
            // KH - Calculate where the projectile is going to move towards.
            Vector2 calcMoveDir = moveDir;

            // KH - Make sure the projectile is moving towards the direction it's facing.
            if (!facingRight)
                calcMoveDir = new Vector2(-calcMoveDir.x, calcMoveDir.y);

            // KH - Make the projectile move in the inputted direction.
            transform.Translate(calcMoveDir * Time.fixedDeltaTime);
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

            // KH - Make sure the projectile graphics are faced towards the direction it's moving it.
            FaceDirection();
        }

        // KH - Make the projectile's graphics face the direction it's facing towards.
        void FaceDirection()
        {
            // KH - Calculate what the X scale of the transform should be.
            xScaleAbs = Mathf.Abs(transform.localScale.x);

            if (!facingRight)
                xScaleAbs = -xScaleAbs;

            // KH - Apply the final calculations onto the transform.
            transform.localScale = new Vector2(xScaleAbs, transform.localScale.y);
        }

        // KH - Method to set the value of 'facingRight'.
        public void SetFacingRight(bool output)
        {
            facingRight = output;
        }

        // AO - Method to scale projectile and translate from fire point to avoid self-collision and correct position
        public void SetScale(float scale)
        {
            //transform.position = new Vector(0, 4, 0);
            //transform.position += Vector3.up * 3f;
            transform.position = new Vector3(transform.position.x + 0.175f, transform.position.y + 0.175f, transform.position.z);
            transform.localScale *= scale;
        }
    }
}