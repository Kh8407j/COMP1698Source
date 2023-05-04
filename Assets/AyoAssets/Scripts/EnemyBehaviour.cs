using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer // KH
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public Instantiator instantiator;
        private Animator anim;
        bool shouldMove = true;

        // KH
        [SerializeField] float damage = 20f;
        private Health health;
        private bool triggeredDeath;

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            health = GetComponent<Health>();
            damage = ManagerVar.Instance.enemyDamage;
        }

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponentInChildren<Animator>();
            if(anim == null)
            {
                Debug.Log("No anim");
            }
            else
            {
                Debug.Log("Anim");
            }
        }

        // KH - Called on a constant timeline.
        void FixedUpdate()
        {
            // Ayo
            if(shouldMove == true)
            {
                transform.Translate(Vector2.left * instantiator.activeRate * Time.deltaTime);
            }

            // KH
            if (health.GetHealth() == 0f && !triggeredDeath)
            {
                triggeredDeath = true;
                anim.SetTrigger("OnEnemyDeath");
                Destroy(gameObject, .667f);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("ScreenX"))
            {
                Destroy(this.gameObject);
                //Debug.Log("Collided with x");
            }
            if (collision.gameObject.CompareTag("ScreenY"))
            {
                instantiator.DelayedInstantiation();
                //Debug.Log("Collided with y");
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                // KH - Check that the enemy isn't dead before dealing damage.
                if (health.GetHealth() > 0f)
                {
                    // KH - Damage the player and instantly kill the enemy.
                    Health playerHealth = collision.gameObject.GetComponent<Health>();
                    playerHealth.ChangeHealth(-damage);

                    // KH - Set the enemy's score worth to zero before instantly killing it so the player doesn't gain.
                    health.SetIncreaseScore(0);
                    health.Kill();

                    // Ayo
                    Debug.Log("Collided with player");
                    //anim.SetTrigger("OnEnemyDeath"); // KH - Moved this line somewhere in FixedUpdate(). :)
                    shouldMove = false;
                    //Destroy(this.gameObject, .667f); // KH - Moved this line too!
                }
            }
        }
    }
}