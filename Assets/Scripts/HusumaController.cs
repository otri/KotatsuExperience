using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HusumaController : MonoBehaviour
{
    private Animator animator = null;

    public bool Open = false;
 
    // Start is called before the first frame update
    void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if( animator.GetBool("DoorsOpen") != Open ) {
            animator.SetBool("DoorsOpen", Open);
        }
    }
}
