using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main instance = null;

    void Awake() {
        if( instance == null ) {
            instance = this;
        }
        else if( instance != this ) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        InitGame();
    }

    // Init all the working parts of the game.
    void InitGame() {
        InitSoundManager();
    }

    // Update the game state every frame.
    void Update() {
    }


//----- Sound Management
    public GameObject efxSourceRoot;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    static float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    static float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.
    AudioSource[] efxSources;
    int efxSourceIndex = 0;

    void InitSoundManager() {
        efxSources = efxSourceRoot.GetComponentsInChildren<AudioSource>(efxSourceRoot);

    }

    AudioSource NextEfxSource() {
        AudioSource source = efxSources[efxSourceIndex];
        efxSourceIndex = (efxSourceIndex+1)%efxSources.Length;
        return source;
    }

    public static void PlaySound( AudioClip clip, bool randomizePitch = false, float volume = 1.0f ) {
        var source = instance.NextEfxSource();

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        
        // Set the pitch of the audio source to the randomly chosen pitch.
        if( randomizePitch ) {
            source.pitch = randomPitch;
        } else {
            source.pitch = 1;
        }

        source.volume = volume;
        source.clip = clip;
        source.Play();
    }
}
