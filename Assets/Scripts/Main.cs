﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

public enum PlayState {
    Undefined,
    InitializePlay,
    StartOfPlay,
    StopMamachan,
    HangLaundry,
    ReturnToPlay,
    InfluenzaGameOver
}

public class Main : MonoBehaviour
{
    public static Main instance = null;

    public CanvasGroup IntroPanel;
    public Button StartButton;

    public CanvasGroup HUD;
    public Thermometer _Thermometer;

    void Start() {
        instance = this;
        InitSoundManager();
        InitGamePlayer();
        SetPlayState(PlayState.InitializePlay);

        StartButton.OnClickAsObservable().Subscribe((click)=>{
            SetPlayState(PlayState.StartOfPlay);
        });
    }

    // Update the game state every frame.
    void Update() {
        UpdatePlayState();
    }

//----- Sound Management ------
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

//---- PlayState Management ----
    PlayState _playState = PlayState.Undefined;
    public PlayState playState {
        get { return _playState; }
        set { SetPlayState(value); }
    }

    public void SetPlayState( PlayState newState ) {
        switch(newState) {
        case PlayState.InitializePlay:
        break;

        case PlayState.StartOfPlay:
            _player.SetActive(true);
            break;

        default:
            Debug.Assert(false, "Unahndled state");
        break;
        }

        _playState = newState;
    }

    public void UpdatePlayState() {
        switch(_playState) {
        case PlayState.Undefined:
        break;

        case PlayState.InitializePlay:
            SetPlayState( PlayState.StartOfPlay );
        break;

        case PlayState.StartOfPlay:
        break;

        default:
            Debug.Assert(false, "Unahndled state");
        break;
        }
    }

    // --- Player Management ---
    GamePlayer _player;

    void InitGamePlayer() {
        _player = GetComponentInChildren<GamePlayer>();
        _player.SetActive(false);
        _Thermometer.SetHPObservable(_player.CurrentHpObservable());
    }

    static IObservable<long> playerHpObservable(){
        return instance._player.CurrentHpObservable();
    }
}
