using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;
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

    public CanvasGroup Watermark;

    public CanvasGroup HUD;
    public Thermometer _Thermometer;
    public GameObject _GameStage;

    public GameObject QuestionPanel;

    public PlayableDirector _GameTimelineDirector;
    public TimelineAsset MamaEnter;  
    public TimelineAsset MamaAttack;

    public MamaController _Mama;
    public AudioClip MikanThrowClip;
    public Transform _MikanInBowl;
    public GameObject MikanPrefab;

    public CanvasGroup WorkScreen;
    public CanvasGroup GameOverScreen;

    public Camera _MainCamera;
    public Camera _MamaAttackCamera;

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

    public AudioClip StartOfPlayMusic;
    public AudioClip StopMamachanMusic;
    public AudioClip WorkMusic;
    public AudioClip GameOverMusic;

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

    public void PlayMusic( AudioClip music ) {
        musicSource.loop = true;
        musicSource.clip = music;
        musicSource.Play();
    }

//---- PlayState Management ----
    PlayState _playState = PlayState.Undefined;
    public PlayState playState {
        get { return _playState; }
        set { SetPlayState(value); }
    }

    public void SetPlayState( PlayState newState ) {
        PlayState oldState = _playState;
        _playState = newState;

        switch(newState) {
        case PlayState.InitializePlay:
        break;

        case PlayState.StartOfPlay:
            PlayMusic(StartOfPlayMusic);
            _GameTimelineDirector.Play(MamaEnter);
            break;

        case PlayState.StopMamachan:
            _Mama.can_walk = true;
            PlayMusic(StopMamachanMusic);
            QuestionPanel.SetActive(false);
            _GameTimelineDirector.Play(MamaAttack);
        break;

        case PlayState.HangLaundry:
            _Mama.can_walk = false;
            _player.IsWarm.Value = false;
            PlayMusic(WorkMusic);
            _player.DoWork();
        break;

        case PlayState.ReturnToPlay:
            _GameTimelineDirector.Stop();
            QuestionPanel.SetActive(false);
            _Mama.can_walk = false;
            break;

        case PlayState.InfluenzaGameOver:
            PlayMusic(GameOverMusic);
        break;

        default:
            Debug.Assert(false, "Unahndled state");
        break;
        }

        bool introScreen = newState == PlayState.InitializePlay;
        IntroPanel.gameObject.SetActive(introScreen);
        Watermark.alpha = introScreen ? 0 : 1;

        bool gameScreen = newState == PlayState.StartOfPlay
                     || newState == PlayState.StopMamachan
                     || newState == PlayState.ReturnToPlay;
        _GameStage.SetActive(gameScreen);

        bool showHUD = newState == PlayState.StartOfPlay
                     || newState == PlayState.StopMamachan
                     || newState == PlayState.HangLaundry
                     || newState == PlayState.ReturnToPlay;
        HUD.gameObject.SetActive(showHUD);

        bool mamaAttack = newState == PlayState.StopMamachan;
        _MainCamera.gameObject.SetActive(!mamaAttack);
        _MamaAttackCamera.gameObject.SetActive(mamaAttack);

        bool doWork = newState == PlayState.HangLaundry;
        WorkScreen.gameObject.SetActive(doWork);

        bool gameOver = newState == PlayState.InfluenzaGameOver;
        GameOverScreen.gameObject.SetActive(gameOver);
    }

    public void UpdatePlayState() {
        switch(_playState) {
        case PlayState.Undefined:
        break;

        case PlayState.InitializePlay:
        break;

        case PlayState.StartOfPlay:
        break;

        case PlayState.StopMamachan:
            _player.WarmpthUpdate();

            if( _GameTimelineDirector.time >= _GameTimelineDirector.duration ) {
                SetPlayState(PlayState.HangLaundry);
            }

            if(Input.GetMouseButtonDown(0))
            {
                Main.PlaySound( MikanThrowClip, randomizePitch:true );
                GameObject thrownMikan = Instantiate(MikanPrefab, _MikanInBowl.position, _MikanInBowl.rotation);
                Vector3 force = new Vector3(0, 5, 10);
                thrownMikan.GetComponent<Rigidbody>().AddForce( force, ForceMode.Impulse );
                thrownMikan.GetComponent<Rigidbody>().useGravity = true;
                thrownMikan.GetComponent<Mikan>().enabled = true;

                _MikanInBowl.gameObject.SetActive(false);
            }
        break;

        case PlayState.HangLaundry:
            _player.WarmpthUpdate();
            if(_player.Working.Value == false) {
                SetPlayState(PlayState.ReturnToPlay);
            }
        break;

        case PlayState.ReturnToPlay:
            // TODO: Refill Mikan
            SetPlayState(PlayState.StartOfPlay);
        break;

        case PlayState.InfluenzaGameOver:
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
        _player.Init();
        _Thermometer.SetHPObservable(_player.CurrentHpObservable());
        _player.CurrentHpObservable().Subscribe((hp)=> {
            if(hp <= 0) {
                SetPlayState(PlayState.InfluenzaGameOver);
            }
        });
    }

    static IObservable<long> playerHpObservable(){
        return instance._player.CurrentHpObservable();
    }

    public void PlayerHitNo() {
        SetPlayState(PlayState.StopMamachan);
    }

    public void PlayerHitYes() {
        // TODO: _DidWorkGetMikan = true;
        SetPlayState(PlayState.HangLaundry);
    }

    public static void MikanHitMama() {

    }
}
