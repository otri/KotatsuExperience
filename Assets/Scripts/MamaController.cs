using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaController : MonoBehaviour
{
    private Animator animator = null;
    public Transform mamaTransform;
    public bool can_walk = true;
    private float pos_speed_x = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // transformを取得
        Transform myTransform = mamaTransform;

        // 座標を取得
        Vector3 pos = myTransform.position;

        if (can_walk == true)
        {
            if (pos.x >= 0.5f) this.pos_speed_x = -0.01f;
            else if (pos.x <= -0.5f) this.pos_speed_x = 0.01f;

            pos.x += this.pos_speed_x;    // z座標へ0.01加算

            myTransform.position = pos;  // 座標を設定
        }

        if( animator.GetBool("Walk") != can_walk ) {
            animator.SetBool("Walk", can_walk);
        }
    }


}
