using SocketIO;
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace Tachyon
{
    public class InvokationManager
    {
        private object objectClass;
        private string gameObjectName;
        private readonly string functionAttributeName = "functionName";
        private readonly string gameObjectAttributeName = "gameObjectName";

        public InvokationManager(object objectClass, string gameObjectName)
        {
            this.objectClass = objectClass;
            this.gameObjectName = gameObjectName;
        }

        public void InvokeFunction(SocketIOEvent e)
        {
            //Debug.Log("InvokationManager : invoke manager");
            List<string> keys = e.data.keys;
            List<JSONObject> values = e.data.list;

            object[] parameters = ConstructParamsArray(keys, values);

            string functionName = e.data[functionAttributeName].str;
            string targetGameObjectName = e.data[gameObjectAttributeName].str;
            //Debug.Log("gameObjectName: " + gameObjectName);
            //Debug.Log("targetGameObjectName: " + targetGameObjectName);
            //if (gameObjectName != targetGameObjectName)
            //{
            //    return;
            //}
            //Debug.Log("InvokeFunctionName: "+ functionName);
            MethodInfo method = objectClass.GetType().GetMethod(functionName.Replace(gameObjectName, ""));
            // Debug.Log(functionName.Replace(gameObjectName, ""));
            method.Invoke(objectClass, parameters);
            method = null;
        }

        private object[] ConstructParamsArray(List<string> keys, List<JSONObject> values)
        {
            List<object> parameters = new List<object>();

            for (int i = 2; i < keys.Count; i++)
            {
                if (keys[i].StartsWith("param"))
                {
                    switch (values[i].type)
                    {
                        case JSONObject.Type.INTEGER:
                            parameters.Add(values[i].n);
                            break;
                        case JSONObject.Type.BOOL:
                            parameters.Add(values[i].b);
                            break;
                        case JSONObject.Type.STRING:
                            parameters.Add(values[i].str);
                            break;
                        case JSONObject.Type.FLOAT:
                            parameters.Add(values[i].f);
                            break;
                    }
                }
            }

            return parameters.ToArray();
        }

        public string GameObjectName()
        {
            return gameObjectName;
        }


    }
}