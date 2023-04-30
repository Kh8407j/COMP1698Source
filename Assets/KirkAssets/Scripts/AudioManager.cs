// KHOGDEN 001115381
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager control; // KH - Allows access to the audio manager from other scripts.

        [SerializeField] GameObject soundObject; // KH - The sound gameobject that'll be instantiated whenever 'PlayAudio()' is called.
        private GameObject lastInstantiatedAudioObject; // KH - Reference for the last audio object to be instantiated from 'PlayAudio()'.

        [Serializable]
        public class Sound
        {
            // KH - Identifiable name and associated audio clips for the sound.
            [SerializeField] string name;
            [SerializeField] AudioClip[] audioClips;

            // KH - Method to return the value of 'name'.
            public string GetName()
            {
                return name;
            }

            // KH - Method to return a audio clip of a specific index in 'audioClips'.
            public AudioClip GetAudioClip(int index)
            {
                return audioClips[index];
            }

            // KH - Method to get the length value of 'audioClips'.
            public int GetAudioClipsLength()
            {
                return audioClips.Length;
            }
        }
        [SerializeField] Sound[] sounds = new Sound[0];

        // KH - Called before 'void Start()'.
        private void Awake()
        {
            control = this;
        }

        // KH - Pick out a sound from 'sounds' and play one of it's audio clips chosen at random.
        public void PlayAudio(string soundName, NextSoundAttributes attributes)
        {
            // KH - Instantiate the sound gameobject and setup component/class references.
            GameObject audioObject = Instantiate(soundObject, attributes.Position, Quaternion.identity, transform.parent);
            lastInstantiatedAudioObject = audioObject;
            AudioSource audio = audioObject.GetComponent<AudioSource>();
            Audio audioScript = audioObject.GetComponent<Audio>();
            Sound sound = GetSound(soundName);

            // KH - To prevent the scene hierarchy getting too messy, rename the audio gameobject.
            audioObject.name = soundName;

            // KH - Set the properties of the audio about to play.
            audio.volume = attributes.Volume;
            audio.pitch = attributes.Pitch;
            audio.spatialBlend = attributes.SpatialBlend;
            audio.minDistance = attributes.MinDistance;
            audio.maxDistance = attributes.MaxDistance;
            audio.loop = attributes.Loop;
            audioScript.FollowTransform(attributes.FollowTransform);

            // KH - Pick out a random audio clip associated with the sound name and play it.
            audio.clip = sound.GetAudioClip(UnityEngine.Random.Range(0, sound.GetAudioClipsLength()));
            audio.Play();
        }

        // KH - Find a sound type from the 'sounds' array and return it, by using the sound name.
        public Sound GetSound(string soundName)
        {
            // KH - Go over all the registered sounds in the 'sounds' array and see which one matches the name inputted in 'soundName'.
            foreach (Sound sound in sounds)
            {
                // KH - If this is the sound with the corresponding name then it'll be returned.
                if (sound.GetName() == soundName)
                    return sound;
            }

            // KH - If no sound types with the corresponding name could be found then return null.
            Debug.LogWarning("Couldn't find sound with the following name: " + soundName);
            return null;
        }

        // KH - Method to get the value of 'lastInstantiatedAudioObject'.
        public GameObject LastInstantiatedAudioObject()
        {
            return lastInstantiatedAudioObject;
        }
    }
}