// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace systems
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] bool paused; // KH - If true, game time will be frozen.
        private bool restrictPausing = true; // KH - If true then the player won't be able to toggle pausing the game.

        // Update is called once per frame
        void Update()
        {
            // KH - If the user is giving the right input, toggle whether the game is paused or not.
            if (Input.GetButtonDown("Pause"))
                TogglePause(!paused);
        }

        // KH - Toggle whether the game is paused or not. If 'restrictPausing' is true then this void give a error message in the console.
        public void TogglePause(bool pauseGame)
        {
            // KH - Check that pausing isn't restricting before toggling pause.
            if (!restrictPausing)
            {
                // KH - Check if the game was already paused before this void was called.
                if (paused)
                {
                    // KH - Unpause the game and resume time if the game was already paused before this void was called.
                    paused = false;
                    Time.timeScale = 1f;
                }
                else
                {
                    // KH - If the game was unpaused before this void was called, pause the game and freeze time.
                    paused = true;
                    Time.timeScale = 0f;
                }
            }
            else
                Debug.LogError("Pausing is restricted!");
        }

        // KH - Set whether pausing should be restricted or not.
        public void RestrictPausing(bool restrict)
        {
            restrictPausing = restrict;
        }

        // KH - Method to check if the game is paused or not.
        public bool IsPaused()
        {
            return paused;
        }
    }
}