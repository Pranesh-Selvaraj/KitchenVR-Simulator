using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] songs;
    public float volume;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            ChangeSong(Random.Range(0, songs.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = volume;

        if (!audioSource.isPlaying)
        {
            ChangeSong(Random.Range(0, songs.Length));
        }
        
    }
    public void ChangeSong(int songIndex)
    {
        audioSource.clip = songs[songIndex];
        audioSource.Play();
    }
}
