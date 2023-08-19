using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] songs;

    [SerializeField] private AudioSource audioSource;
    private int currentSongIndex;
    public static SoundtrackManager soundtrackManagerInstance { get; private set; }

    private void Awake()
    {
        if (soundtrackManagerInstance != null && soundtrackManagerInstance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            soundtrackManagerInstance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentSongIndex = Random.Range(0, songs.Length);

        PlayGameSong(currentSongIndex);
    }


    private void Update()
    {
        // Check if the current song has finished playing
        if (!audioSource.isPlaying)
        {
            // Increment the song index
            currentSongIndex++;

            // If we have reached the end of the song list, start from the beginning
            if (currentSongIndex >= songs.Length)
            {
                currentSongIndex = 0;
            }

            // Play the next song
            PlayGameSong(currentSongIndex);
        }
    }

    private void PlayGameSong(int index)
    {
        audioSource.clip = songs[index];
        audioSource.Play();
    }
}
