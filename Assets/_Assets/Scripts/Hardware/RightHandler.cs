using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RightHandler : MonoBehaviour{

    Quaternion quatOfWrist;
    Vector3 posOfWristDelta;
    Vector3 posOfWristorigin;

    Quaternion originRotation;
    Vector3 originPosition;

    public float scalConstModiferX = 1;
    public float scalConstModiferY = 1;

    public float scalConstX = 12;
    public float scalConstY = 12;


    void Update(){

        quatOfWrist = GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R/ElbowPart1_R/ElbowPart2_R/ElbowPart3_R/Wrist_R").transform.rotation;
        posOfWristDelta = GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R/ElbowPart1_R/ElbowPart2_R/ElbowPart3_R/Wrist_R").transform.position;
        posOfWristDelta -= posOfWristorigin;
        posOfWristDelta.x *= scalConstX;
        posOfWristDelta.y *= scalConstY;
        posOfWristDelta.z = 0;
        //Debug.Log("quat of wrist: " + quatOfWrist);
        //Debug.Log("pos of wrist: " + posOfWristDelta);
        //transform.rotation = originRotation * quatOfWrist;
        transform.position = originPosition + posOfWristDelta;        
    
    }
 

    void Start(){

        originRotation = transform.rotation;
        originPosition = transform.position;
        posOfWristorigin = GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R/ElbowPart1_R/ElbowPart2_R/ElbowPart3_R/Wrist_R").transform.position;
        scalConstX *= scalConstModiferX;
        scalConstY *= scalConstModiferY;

    }

}
