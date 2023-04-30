// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace audio
{
    public class Audio : MonoBehaviour
    {
        private Transform followTransform; // KH - If not null, the sound will constantly follow this transform.
        private AudioSource audio; // KH - Reference to the audio source component.

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            audio = GetComponent<AudioSource>();
        }

        // KH - Called on a constant timeline.
        private void FixedUpdate()
        {
            // KH - If 'followTransform' isn't null, have this audio source continuously follow it.
            if (followTransform != null)
                transform.position = followTransform.position;

            // KH - Once the audio for this game object stops playing, it'll be destroyed. Don't play sounds with no pitch.
            if (!audio.isPlaying || audio.pitch == 0f)
                Destroy(gameObject);
        }

        // KH - Method to set the value of 'followTransform'.
        public void FollowTransform(Transform transform)
        {
            followTransform = transform;
        }
    }
}