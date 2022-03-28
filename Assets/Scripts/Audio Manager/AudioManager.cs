using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public Sound[] sounds;

	void Awake()
	{
		// Avoiding Duplicating of Audio Manager instance in different scenes.
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			
		}

		// Setting every element in array Audio Source values to the Sound class variables.
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();

			s.source.pitch = s.pitch;
			s.source.volume = s.volume;
			s.source.clip = s.clip;
			s.source.loop = s.loop;

		}
	}

    private void Start()
    {
        Play("Theme");
    }

    // Function To play Sound 
    public void Play(string sound)
	{
		//Finding sound in array to play 
		Sound s = Array.Find(sounds, item => item.name == sound);

		//Avoiding Null Refrence Exception if Sound is not find in array
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Play();
	}

	public void StopSound(string sound)
	{
		//Finding sound in array to play 
		Sound s = Array.Find(sounds, item => item.name == sound);

		//Avoiding Null Refrence Exception if Sound is not find in array
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Stop();
	}

}
