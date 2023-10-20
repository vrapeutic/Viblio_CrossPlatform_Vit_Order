using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirDecCube : MonoBehaviour{

    Vector3 originPosition;
    Vector3 myPosition;
    Boolean myLock = true;

    // Start is called before the first frame update
    void Start(){

        originPosition = transform.position;
        myPosition = originPosition;
    }

    // Update is called once per frame
    void Update(){

        if(!originPosition.Equals(transform.position)){
            StartCoroutine(returnOrigin(0.5F));
        }
    }
 
    IEnumerator returnOrigin(float secs){

        yield return new WaitForSeconds(secs);
        myPosition = originPosition;
        transform.position = originPosition;
        myLock = true;
    }

    public void moVertical(Boolean dir){
        if(myLock){
            myLock = false;
            if(dir){
                Debug.Log("forward");
                myPosition = new Vector3(myPosition.x, 0.4F, myPosition.z);
            }
            else{
                Debug.Log("back");
                myPosition = new Vector3(myPosition.x, -0.4F, myPosition.z);
            }
            transform.position = myPosition;
        }
    }

    public void moHoriznotal(Boolean dir){
        if(myLock){
            myLock = false;
            if(dir){
                Debug.Log("left");
                myPosition = new Vector3(0.4F, myPosition.y, myPosition.z);
            }
            else{
                Debug.Log("right");
                myPosition = new Vector3(-0.4F, myPosition.y, myPosition.z);
            }
            transform.position = myPosition;
        }
    }
    
}
