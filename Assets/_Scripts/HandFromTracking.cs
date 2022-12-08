using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFromTracking : MonoBehaviour
{
    public static HandGetter handInfo;
    public Transform[] phalanges;

    [SerializeField] Transform handModel;

    // Start is called before the first frame update
    void Start()
    {
        //phalanges[0] = handModel.transform.GetChild()
        for (int i = 0; i < handModel.childCount; i++)
            print(handModel.GetChild(i).gameObject.name);

       
    }

    // Update is called once per frame
    void Update()
    {
        //for(int i=0; i<26; i++)
        //{
        //    phalanges[i].pos = handInfo.positions[i];
        //    phalanges[i].rot = handInfo.orientations[i];
        //    phalanges[i].joint = handInfo.joints[i];
        //    phalanges[i].rad = handInfo.radius[i];
        //}

        //print(phalanges);
    }
}
