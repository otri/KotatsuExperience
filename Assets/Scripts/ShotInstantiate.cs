using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotInstantiate : MonoBehaviour
{
    public GameObject MiganInstiante;

    [SerializeField] float WaitForCLicked = 2f;
    [SerializeField] AudioClip throwClip;


    bool IfClicked = false;

    void Start()
    {
        IfClicked = false;
    }


  void Update()
    {

        
    }

    IEnumerator IFCLICKEDBYPLAYER()
    {

        IfClicked = true;
        yield return new WaitForSeconds(WaitForCLicked);
        IfClicked = false;
    }

}
