// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace entity
{
    public class Health : MonoBehaviour
    {
        // The amount of health the damageable entity has, and the maximum it can go up to.
        [SerializeField] float health = 100f;
        [SerializeField] float maxHealth = 100f;

        // Add or remove health from 'health' by the inputted value in 'amount'.
        public void ChangeHealth(float amount)
        {
            // Store the value of health before altering it.
            float prevHealth = health;

            // Add the inputted amount of health being given to 'health'.
            health += amount;

            // Check that health went down 
            if(prevHealth > health)
            {
                
            }
            else
            {

            }

            // Check that the health change has caused the health value to go over the maximum/minimum it's allowed up/down to.
            if (health > maxHealth)
                health = maxHealth;
            else if (health < 0f)
                health = 0f;
        }

        // Method to get the value of 'health'.
        public float GetHealth()
        {
            return health;
        }

        // Method to get the value of 'maxHealth'.
        public float GetMaxHealth()
        {
            return maxHealth;
        }
    }
}
