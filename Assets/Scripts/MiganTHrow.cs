using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiganTHrow : MonoBehaviour
{
    Rigidbody MiganRigid;   //蜜柑の自身RigidBody

    [SerializeField] float Force=4f;
    [SerializeField] float TimeToDestroy = 4f;

    private void Start()
    {
        MiganRigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.Translate(new Vector3(0.0f, 0.0f, Force * Time.deltaTime));
        Destroy(this.gameObject,TimeToDestroy);
    }
}
