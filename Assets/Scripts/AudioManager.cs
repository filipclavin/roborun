using System;
using UnityEngine;

//Script Made By Daniel Alvarado
public class AudioManager : MonoBehaviour
{
    [SerializeField] private bool mainMenu;
    public Sound[] sounds;
    public static AudioManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            {
                Destroy(gameObject);
                return;
            }
        }
        
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
       
        s.source.Play();
    }

    private void Start()
    {
        Play("Theme");
    }
    public void StopPizzaTheme()
    {
        sounds[10].source.Stop();
    }

    private void Update()
    {
        if (mainMenu || UIManager.Instance.isPaused)
        {
            for (int i = 1; i < sounds.Length; i++)
            {
                sounds[i].source.Pause();
            }
        }
        else
        {
            for (int i = 1; i < sounds.Length; i++)
            {
                sounds[i].source.UnPause();
            }
        }
    }

    public void NoMainMenu()
    {
        mainMenu = false;
    }

}
