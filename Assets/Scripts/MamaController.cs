using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamaController : MonoBehaviour
{
    private Animator animator = null;
    public Transform mamaTransform;
    public bool can_walk = true;
    private float pos_speed_x;
    private const float DodgeSpeed = 2.0f;
    private const float DodgeMaxX = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponentInChildren<Animator>();
        pos_speed_x = DodgeSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // transformを取得
        Transform myTransform = mamaTransform;

        // 座標を取得
        Vector3 pos = myTransform.localPosition;

        if (can_walk == true)
        {
            if (pos.x >= DodgeMaxX) this.pos_speed_x = -DodgeSpeed;
            else if (pos.x <= -DodgeMaxX) this.pos_speed_x = DodgeSpeed;

            pos.x += this.pos_speed_x * Time.deltaTime;    // z座標へ0.01加算
        } else {
            pos.x -= pos.x * 0.5f * Time.deltaTime;    // z座標へ0.01加算
        }

        myTransform.localPosition = pos;  // 座標を設定

        if( animator.GetBool("Walk") != can_walk ) {
            animator.SetBool("Walk", can_walk);
        }
    }

    void OnCollisionEnter (Collision coll)
    {
        Mikan otherMikan = coll.rigidbody.GetComponent<Mikan>();
        if( otherMikan != null ) {
            Main.MikanHitMama();
        }
    }

}
