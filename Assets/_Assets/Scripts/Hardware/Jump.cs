using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour{

    Vector3 originPosition;
    Boolean myLock = true;

    // Start is called before the first frame update
    void Start(){

        originPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update(){

        if(!originPosition.Equals(transform.position)){
            StartCoroutine(returnOrigin(0.2F));
        }
        
    }

    IEnumerator returnOrigin(float secs){
        
        yield return new WaitForSeconds(secs);
        transform.position = originPosition;
        myLock = true;
    }

    public void moVertical(){
        if(myLock){
            myLock = false;
            Debug.Log("jump");
            transform.position += new Vector3(0, 1.1F, 0);
        }
    }

}
