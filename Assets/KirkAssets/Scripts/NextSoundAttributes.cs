// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace audio
{
    public class NextSoundAttributes : ScriptableObject
    {
        [SerializeField] Vector3 pos; // KH - The position that the next played sound will be set.
        [SerializeField] float volume = 1f; // KH - The volume that the next played sound will be set to.
        [SerializeField] float pitch = 1f; // KH - The pitch that the next played sound will be set to.
        [SerializeField] float spatialBlend = 1f; // KH - The spatial blend value the next played sound will have.
        [SerializeField] float minDistance = 5f; // KH - The minimum distance the next played sound will have.
        [SerializeField] float maxDistance = 500f; // KH - The maximum distance the next played sound will have.
        [SerializeField] bool loop = false; // KH - Determines whether the next played sound should loop or not.
        [SerializeField] Transform followTransform = null; // KH - The transform that the next played sound will follow.

        public Vector3 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public float Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        public float Pitch
        {
            get { return pitch; }
            set { pitch = value; }
        }

        public float SpatialBlend
        {
            get { return spatialBlend; }
            set { spatialBlend = value; }
        }

        public float MinDistance
        {
            get { return minDistance; }
            set { minDistance = value; }
        }

        public float MaxDistance
        {
            get { return maxDistance; }
            set { maxDistance = value; }
        }

        public bool Loop
        {
            get { return loop; }
            set { loop = value; }
        }

        public Transform FollowTransform
        {
            get { return followTransform; }
            set { followTransform = value; }
        }
    }
}