// KHOGDEN 001115381
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer
{
    public class PlatformController : MonoBehaviour
    {
        // KH - Sub-class containing settings for input restrictions. Accessible in inspector.
        [Serializable]
        public class Restrictions
        {
            [SerializeField] bool hor;
            [SerializeField] bool ver;

            // KH - Method to get the value of 'Hor'.
            public bool Hor()
            {
                return hor;
            }

            // KH - Method to get the value of 'Ver'.
            public bool Ver()
            {
                return ver;
            }
        }
        public Restrictions restrictions = new Restrictions();

        // KH - Stores the value for the ultrasonic sensor reading on the Arduino kit.
        private float ultrasonicSensorDistance;

        // KH - Calculated player input values.
        private float horInput;
        private float verInput;
        private bool holdingFire1;

        // KH - The motor/attack scripts this controller will be collaborating with.
        private PlatformMotor motor;
        private PlatformAttack attack;

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            motor = GetComponent<PlatformMotor>();
            attack = GetComponent<PlatformAttack>();
        }

        // Update is called once per frame
        void Update()
        {
            CalculateInput();
            Output();
        }

        // KH - Check for any input the player is giving and apply it into the controller.
        void CalculateInput()
        {
            horInput = Input.GetAxisRaw("Horizontal");
            verInput = Input.GetAxisRaw("Vertical");
            holdingFire1 = Input.GetButton("Fire1");
        }

        // KH - Output the received input values to the output scripts
        void Output()
        {
            // KH - Check that horizontal inputs aren't being restricted before outputting to motor.
            if (!restrictions.Hor())
                motor.SetHorOutput(horInput);

            // KH - Check that vertical inputs aren't being restricted before outputting to motor.
            if (!restrictions.Ver())
                motor.SetVerOutput(verInput);

            // KH - Output to the attack script whether fire 1 is being held down.
            attack.SetFire1Output(holdingFire1);
        }
    }
}