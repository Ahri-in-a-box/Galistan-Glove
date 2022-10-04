using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class DummyBehavior : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        if(!(anim = GetComponent<Animator>()))
            throw new System.Exception("No animator found");
    }

    private void OnCollisionEnter(Collision collision)
    {
        anim.Play("pushed", 0, 0.0f);
    }
}
