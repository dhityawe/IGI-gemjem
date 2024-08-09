using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume;
        [Range(-3f, 3f)]
        public float pitch;

        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }

    public Sound[] sounds;

    private Dictionary<string, AudioSource> soundDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        soundDictionary = new Dictionary<string, AudioSource>();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            soundDictionary[s.name] = s.source; // Add to dictionary
        }
    }


    public void PlaySound(string name)
    {
        if (soundDictionary.TryGetValue(name, out AudioSource source))
        {
            source.Play();
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }


    public void StopSound(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            soundDictionary[name].Stop();
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }

    public void SetVolume(string name, float volume)
    {
        if (soundDictionary.ContainsKey(name))
        {
            soundDictionary[name].volume = volume;
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }

    public void SetPitch(string name, float pitch)
    {
        if (soundDictionary.ContainsKey(name))
        {
            soundDictionary[name].pitch = pitch;
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }
}
