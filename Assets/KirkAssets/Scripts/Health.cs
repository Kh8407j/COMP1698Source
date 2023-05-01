// KHOGDEN 001115381
using audio;
using platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer
{
    public class Health : MonoBehaviour
    {
        // KH - The amount of health the damageable entity has, and the maximum it can go up to.
        [SerializeField] float health = 100f;
        [SerializeField] float maxHealth = 100f;

        // KH - Add or remove health from 'health' by the inputted value in 'amount'.
        public void ChangeHealth(float amount)
        {
            // KH - Store the value of health before altering it.
            float prevHealth = health;

            // KH - Add the inputted amount of health being given to 'health'.
            health += amount;

            // KH - Check that health went down or up.
            if (prevHealth > health)
            {
                // KH - Play the hurt sound.
                NextSoundAttributes soundAttributes = new NextSoundAttributes();
                AudioManager.control.PlayAudio("Hurt", soundAttributes);
            }
            else
            {

            }

            // KH - Check that the health change has caused the health value to go over the maximum/minimum it's allowed up/down to.
            if (health > maxHealth)
                health = maxHealth;
            else if (health < 0f)
                health = 0f;

            // KH - Check that the entity died/got destroyed.
            if(health == 0f)
            {
                // References to scripts that need to be disabled to used.
                PlatformController controller = GetComponent<PlatformController>();
                PlatformMotor motor = GetComponent<PlatformMotor>();

                // Disable the controller script if this is the player.
                if(controller != null)
                    controller.enabled = false;

                // Undo output values the motor may have from any controller scripts.
                if (motor != null)
                    motor.ResetOutput();
            }
        }

        // KH - Method to get the value of 'health'.
        public float GetHealth()
        {
            return health;
        }

        // KH - Method to get the value of 'maxHealth'.
        public float GetMaxHealth()
        {
            return maxHealth;
        }
    }
}
