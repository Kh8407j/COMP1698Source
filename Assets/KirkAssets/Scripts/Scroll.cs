// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace platformer
{
    public class Scroll : MonoBehaviour
    {
        [Header("Scroll Settings")]
        [SerializeField] float scrollSpeedRate = 1f;
        [SerializeField] float resetXPosAt = -10f;

        // KH - Holds reference of the transform's position during awake.
        private Vector2 awakePos;

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            awakePos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            // KH-  Continuously make the level scroll to the left to give the illusion everything's moving.
            transform.Translate(new Vector3(-scrollSpeedRate * Time.deltaTime, 0f, 0f));

            // KH - To give the illusion the level is endless, reposition the transform when it scrolls too far out.
            if (transform.position.x < resetXPosAt)
                transform.position = awakePos;
        }
    }
}