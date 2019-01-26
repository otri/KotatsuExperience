using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiganTHrow : MonoBehaviour
{
    Rigidbody MiganRigid;   //蜜柑の自身RigidBody

    [SerializeField] float Force=4f;
    [SerializeField] float TimeToDestroy = 4f;
    [SerializeField] AudioClip bounceClip;
    private float velToVol = .2F;
    private float velocityClipSplit = 10F;

    private void Start()
    {
        MiganRigid = GetComponent<Rigidbody>();
        Destroy(this.gameObject,TimeToDestroy);
    }

    private void Update()
    {
        transform.Translate(new Vector3(0.0f, 0.0f, Force * Time.deltaTime));
    }

    void OnCollisionEnter (Collision coll)
    {
        float hitVol = coll.relativeVelocity.magnitude * velToVol;
        Main.PlaySound(bounceClip,randomizePitch:true, volume:hitVol);
    }
}
