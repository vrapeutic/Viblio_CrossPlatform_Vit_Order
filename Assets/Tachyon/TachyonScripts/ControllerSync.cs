using System.Diagnostics;
using SocketIO;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System;


namespace Tachyon
{
    public class ControllerSync : MonoBehaviour
    {
        static int counter = 0;

        [SerializeField] GameObject controller;
        [SerializeField] string controllerName;
        [SerializeField] bool syncRotation = false;
        [SerializeField] bool syncPosition = false;
        [SerializeField] float translateMultiplayer = 0.6f;
        [SerializeField] float waitTime = .2f;

        #region Private Members
        private Transform controllerTransform;
        private JSONObject rotation = new JSONObject(JSONObject.Type.OBJECT);
        private JSONObject position = new JSONObject(JSONObject.Type.OBJECT);
        private JSONObject controllerInfo = new JSONObject(JSONObject.Type.OBJECT);
        private Quaternion targetControllerRotation = new Quaternion();
        private Quaternion currentControllerRotation;
        private Vector3 targetControllerPosition = new Vector3();
        private Vector3 currentControllerPosition;
        private readonly float lerpSpeed = 16f;
        private bool canObserve = true;
        private Coroutine rotateControllerRoutine = null;
        private Coroutine translateControllerRoutine = null;
        #endregion

        #region Couting the number of instances of a class
        public ControllerSync()
        {
            Interlocked.Increment(ref counter);
        }

        ~ControllerSync()
        {
            Interlocked.Decrement(ref counter);
        }
        #endregion

        void Start()
        {
            controllerTransform = controller.GetComponent<Transform>();
            FixControllerName();
            //            StartCoroutine(StopObserving());
            if (SocketIOComponent.instance != null)
            {
#if UNITY_STANDALONE
            waitTime = waitTime * 1.25f;
                if (syncRotation)
                {
                    SocketIOComponent.instance.On("updateControllerRotation" + controllerName, OnUpdateControllerRotation);
                }
                if (syncPosition)
                {
                    SocketIOComponent.instance.On("updateControllerPosition" + controllerName, OnUpdateControllerPosition);
                }
#endif

#if UNITY_ANDROID
                if (syncRotation)
                {
                    controllerInfo.SetField("controllerName", controllerName);
                    SocketIOComponent.instance.Emit("registerControllerRotation", controllerInfo);

                    StartCoroutine(ChangeControllerRotation());
                }
                if (syncPosition)
                {
                    controllerInfo.SetField("controllerName", controllerName);
                    SocketIOComponent.instance.Emit("registerControllerPosition", controllerInfo);

                    StartCoroutine(ChangeControllerPosition());
                }
#endif                
            }
        }

        void FixControllerName()
        {
            controllerName = controllerName.Replace(" ", "");
            if (controllerName.Length == 0)
            {
                controllerName = counter.ToString();
            }
        }

        IEnumerator ChangeControllerRotation()
        {
            while (true)
            {
                rotation.SetField("x", controllerTransform.rotation.x);
                rotation.SetField("y", controllerTransform.rotation.y);
                rotation.SetField("z", controllerTransform.rotation.z);
                rotation.SetField("w", controllerTransform.rotation.w);
                rotation.SetField("controllerName", controllerName);
                SocketIOComponent.instance.Emit(controllerName + "Rotation", rotation);
                yield return new WaitForSeconds(waitTime);
            }
        }

        IEnumerator ChangeControllerPosition()
        {
            while (true)
            {
                position.SetField("x", controllerTransform.position.x);
                position.SetField("y", controllerTransform.position.y);
                position.SetField("z", controllerTransform.position.z);
                position.SetField("controllerName", controllerName);
                SocketIOComponent.instance.Emit(controllerName + "Position", position);
                yield return new WaitForSeconds(waitTime);
            }
        }

        private void OnUpdateControllerRotation(SocketIOEvent e)
        {
            //currentControllerRotation = controllerTransform.rotation;
            targetControllerRotation.x = JSONObjectToFloat(e.data["x"]);
            targetControllerRotation.y = JSONObjectToFloat(e.data["y"]);
            targetControllerRotation.z = JSONObjectToFloat(e.data["z"]);
            targetControllerRotation.w = JSONObjectToFloat(e.data["w"]);
            //transform.rotation = targetControllerRotation;
            if (rotateControllerRoutine != null) StopCoroutine(rotateControllerRoutine);
            rotateControllerRoutine = StartCoroutine(RotateController(targetControllerRotation, waitTime));
        }

        IEnumerator RotateController(Quaternion targetRotation, float rotationTime)
        {
            Quaternion beginningRotation = controllerTransform.rotation;
            float beginningTime = Time.time;
            float step = 0f;
            while ((Time.time - beginningTime) < rotationTime)
            {
                step += (1 / rotationTime) * Time.deltaTime;
                controllerTransform.rotation = Quaternion.Lerp(beginningRotation, targetRotation, step);
                yield return null;
            }
        }

        private void OnUpdateControllerPosition(SocketIOEvent e)
        {
            // currentControllerPosition = controllerTransform.position;
            targetControllerPosition.x = JSONObjectToFloat(e.data["x"]);
            targetControllerPosition.y = JSONObjectToFloat(e.data["y"]);
            targetControllerPosition.z = JSONObjectToFloat(e.data["z"]);
            //transform.position = targetControllerPosition;
            if (translateControllerRoutine != null) StopCoroutine(translateControllerRoutine);
            translateControllerRoutine = StartCoroutine(TranslateController(targetControllerPosition, waitTime));
            //controllerTransform.position = Vector3.Lerp(currentControllerPosition, targetControllerPosition, lerpSpeed * Time.deltaTime);
        }

        IEnumerator TranslateController(Vector3 targetPosition, float translateTime)
        {
            Vector3 beginningPostion = controllerTransform.position;
            float beginningTime = Time.time;
            float step = 0;
            while ((Time.time - beginningTime) < translateTime)
            {
                step += (1 / translateTime) * Time.deltaTime * translateMultiplayer;
                controllerTransform.position = Vector3.Lerp(beginningPostion, targetPosition, step);
                yield return null;
            }
        }

        private float JSONObjectToFloat(JSONObject jsonObject)
        {
            return float.Parse(jsonObject.ToString());
        }

        IEnumerator StopObserving()
        {
            yield return new WaitForSeconds(300);
            canObserve = false;
        }
    }
}
