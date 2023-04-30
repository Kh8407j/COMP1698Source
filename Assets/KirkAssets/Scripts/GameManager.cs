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
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // KH - Method to access the pause system.
        public Pause PauseSystem()
        {
            return pause;
        }
    }
}