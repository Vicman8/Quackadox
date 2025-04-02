using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /*
     * If you want to use this AudioManager, put FindObjectOfType<AudioManager>().Play("SoundNameHere"); to add stuff.
     * I don't want to mess with your guys' code if I don't have to, so if you follow this instruction you can put music/Sound effects in practically anywhere.
     * I've added the finished sounds to the list on the AudioManager if you need a quick index.
     * If you *really* want me to add the sounds myself, then it will have to be on scripts I know no one is working on, so tell me.
     */

    public Sound[] sounds;

    public static AudioManager instance;

    // Used for initialization
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Debug.Log("Playing Audio.");
        Play("TitleScreen");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found. Did you name the file correctly?");
            return;
        }
        s.source.Play();
        
    }
}
