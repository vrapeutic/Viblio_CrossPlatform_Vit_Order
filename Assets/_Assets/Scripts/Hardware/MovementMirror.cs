using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MovementMirror : MonoBehaviour{

    Quaternion quat;
    Quaternion orign;
    public string GameObjectId = "/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/ShoulderPart3_L/Elbow_L/ElbowPart1_L/ElbowPart2_L/ElbowPart3_L/Wrist_L";


    void Update(){

        quat = GameObject.Find(GameObjectId).transform.rotation;
        transform.rotation = orign * quat;
    
    }
 

    void Start(){
        orign = transform.rotation * Quaternion.Inverse(GameObject.Find(GameObjectId).transform.rotation);
    }

}
