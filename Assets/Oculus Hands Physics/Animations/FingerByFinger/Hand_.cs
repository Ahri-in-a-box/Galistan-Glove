using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Hand_ : MonoBehaviour
{
    


    Animator animator;
    public float speed;

    private float pointTarget;
    private float gripTarget;

    private float pointCurrent;
    private float gripCurrent;


    private float indexTarget;
    private float middleTarget;
    private float ringTarget;
    private float pinkyTarget;
    private float thumbTarget;

    private float indexCurrent;
    private float middleCurrent;
    private float ringCurrent;
    private float pinkyCurrent;
    private float thumbCurrent;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
    }

    internal void SetPoint(float v)
    {
        pointTarget = v;
    }
    
    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    internal void SetIndex(float v)
    {
        indexTarget = v;
    }

    internal void SetMiddle(float v)
    {
        middleTarget = v;
    }

    internal void SetRing(float v)
    {
        ringTarget = v;
    }

    internal void SetPinky(float v)
    {
        pinkyTarget = v;
    }

    internal void SetThumb(float v)
    {
        thumbTarget = v;
    }

    void AnimateHand()
    {
        if (pointCurrent != pointTarget)
        {
            pointCurrent = Mathf.MoveTowards(pointCurrent, pointTarget, Time.deltaTime * speed);
            animator.SetFloat("Point", pointCurrent);
        }

        if (gripCurrent != gripTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            animator.SetFloat("Grip", gripCurrent);
        }

        if (indexCurrent != indexTarget)
        {
            indexCurrent = Mathf.MoveTowards(indexCurrent, indexTarget, Time.deltaTime * speed);
            animator.SetFloat("Index", indexCurrent);
        }

        if(middleCurrent != middleTarget)
        {
            middleCurrent = Mathf.MoveTowards(middleCurrent, middleTarget, Time.deltaTime * speed);
            animator.SetFloat("Middle", middleCurrent);
        }

        if(ringCurrent != ringTarget)
        {
            ringCurrent = Mathf.MoveTowards(ringCurrent, ringTarget, Time.deltaTime * speed);
            animator.SetFloat("Ring", ringTarget);
        }

        if(pinkyCurrent != pinkyTarget)
        {
            pinkyCurrent = Mathf.MoveTowards(pinkyCurrent, pinkyTarget, Time.deltaTime * speed);
            animator.SetFloat("Pinky", pinkyTarget);
        }
        
        if(thumbCurrent != thumbTarget)
        {
            thumbCurrent = Mathf.MoveTowards(thumbCurrent, 1.2f * thumbTarget, Time.deltaTime * speed);
            animator.SetFloat("Thumb", thumbCurrent);
        }
    }
}
