// KHOGDEN 001115381
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer
{
    public class PlatformController : MonoBehaviour
    {
        // Sub-class containing settings for input restrictions. Accessible in inspector.
        [Serializable]
        public class Restrictions
        {
            [SerializeField] bool hor;
            [SerializeField] bool ver;

            // Method to get the value of 'Hor'.
            public bool Hor()
            {
                return hor;
            }

            // Method to get the value of 'Ver'.
            public bool Ver()
            {
                return ver;
            }
        }
        public Restrictions restrictions = new Restrictions();

        // Calculated player input values.
        private float horInput;
        private float verInput;

        // The motor script this controller will be collaborating with.
        private PlatformMotor motor;

        // Called before 'void Start()'.
        private void Awake()
        {
            motor = GetComponent<PlatformMotor>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CalculateInput();
            OutputToMotor();
        }

        // Check for any input the player is giving and apply it into the controller.
        void CalculateInput()
        {
            horInput = Input.GetAxisRaw("Horizontal");
            verInput = Input.GetAxisRaw("Vertical");
        }

        // Output the received input values to the motor script.
        void OutputToMotor()
        {
            // Check that horizontal inputs aren't being restricted before outputting to motor.
            if(!restrictions.Hor())
                motor.SetHorOutput(horInput);

            // Check that vertical inputs aren't being restricted before outputting to motor.
            if (!restrictions.Ver())
                motor.SetVerOutput(verInput);
        }
    }
}