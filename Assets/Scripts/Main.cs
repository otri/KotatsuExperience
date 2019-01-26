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

    }

    // Update the game state every frame.
    void Update() {
    }
}
