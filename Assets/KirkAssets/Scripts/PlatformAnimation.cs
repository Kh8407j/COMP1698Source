// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer
{
    public class PlatformAnimation : MonoBehaviour
    {
        // KH - Component references.
        private Animator anim;
        private PlatformMotor motor;
        private Health health;

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            anim = GetComponent<Animator>();
            motor = GetComponentInParent<PlatformMotor>();
            health = GetComponentInParent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            // Continously update the runtime animator's parameters so it can play the correct animations.
            anim.SetFloat("Health", health.GetHealth());
            anim.SetFloat("Horizontal", Mathf.Abs(motor.GetMoveDir().x));
            anim.SetFloat("Vertical", motor.GetMoveDir().y);
            anim.SetBool("OnGround", motor.GetOnGround());
            anim.SetBool("OnRoof", motor.GetOnRoof());
        }
    }
}