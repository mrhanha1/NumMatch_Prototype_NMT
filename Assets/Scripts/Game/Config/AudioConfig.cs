using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Config/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    public AudioEntry[] entries;

    [Serializable]
    public struct AudioEntry
    {
        public string key;
        public AudioClip clip;
    }
}