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

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(IFCLICKEDBYPLAYER());

            //プレイヤーが押さしと、二つ弾を逃げるようにする。
            //If The Player Click, To Avoid More than One Shot Been Appeared
            if (IfClicked)
            {
                Main.PlaySound( throwClip, randomizePitch:true );
                Instantiate(MiganInstiante, transform.position, Quaternion.identity);
            }
        }
        
    }

    IEnumerator IFCLICKEDBYPLAYER()
    {

        IfClicked = true;
        yield return new WaitForSeconds(WaitForCLicked);
        IfClicked = false;
    }

}
