// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace systems
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager control; // KH - Allows other scripts to access the game manager.
        private Pause pause; // KH - Access to the pause system.
        private int score; // KH - The player's current score.

        static GameUI gameUI; // AO - Sets GameUI object
        // AO - pause method
        public static bool gamePaused;
        public static float health; // AO - health for UI

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            // KH - Destroy any duplicate game manager gameobjects created from transferring scenes.
            if (control == null)
            {
                control = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (control != this)
                Destroy(gameObject);

            // AO - UI set and update
            gameUI = FindObjectOfType<GameUI>();
            health = 100;//FindObjectOfType<Player>().playerHealth;

            gameUI.UpdateHealth();
        }

        void Update()
        {
            score = ManagerVar.Instance.score;
        }

        // KH - Reset the statistics of the session.
        public void ResetSessionStatistics()
        {
            score = 0;
        }

        // KH - Increase the player's score by a inputted amount.
        public void IncreaseScore(int increase)
        {
            score += increase;
            gameUI.UpdateScore();
        }

        // KH - Method to get the value of 'score'.
        public int GetScore()
        {
            return score;
        }

        public void SetScore(int output)
        {
            score = output;
        }

        // KH - Method to access the pause system.
        public Pause PauseSystem()
        {
            return pause;
        }

        public static void CalculateHealth(float healthValue)
        {
            health += healthValue;
            health = Mathf.Clamp(health, 0, 100);
            if (health <= 0)
            {
                gameUI.CheckGameState(GameUI.GameState.GameOver);
                //FindObjectOfType<AudioManager>().AudioTrigger(AudioManager.SoundFXCat.PlayerDeath, Vector3.zero, 1.5f);
            }
            else
            {
                gameUI.UpdateHealth();
            }
        }
    }
}