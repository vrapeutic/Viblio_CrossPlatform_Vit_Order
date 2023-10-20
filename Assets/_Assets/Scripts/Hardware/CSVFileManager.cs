using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class CSVFileManager : MonoBehaviour
{
    //public Text pathText;
    public CloudStorage storage = new CloudStorage();

    private void Start()
    { 
    }

    public void writeStringToFile(string str, string filename)
    {
#if !WEB_BUILD
        string path = pathForDocumentsFile(filename);
        Debug.Log(path);
        FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);

        StreamWriter sw = new StreamWriter(file);
        sw.WriteLine(str);

        sw.Close();
        file.Close();
        storage.pushFirebase(path, filename);
#endif
    }
    public string pathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            //Debug.Log((Path.Combine(path, "Documents"), filename));
            //pathText.text = path;
            return Path.Combine(Path.Combine(path, "Documents"), filename);

            

        }

        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            //Debug.Log(Path.Combine(path, filename));
            //pathText.text = path;
            return Path.Combine(path, filename);

        }

        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            //Debug.Log(Path.Combine(path, filename));
            //pathText.text = path;
            return Path.Combine(path, filename);

        }

    }
}
