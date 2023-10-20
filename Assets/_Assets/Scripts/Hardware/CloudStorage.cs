using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//For Picking files
using System.IO;

//For firebase storage
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
public class CloudStorage : MonoBehaviour
{
    FirebaseStorage storage;
    StorageReference storageReference;
    StorageReference uploadRef;
    // Start is called before the first frame update
    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://test-6dfbd.appspot.com/");
    }


    public void pushFirebase(string path, string _fileName)
    {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://test-6dfbd.appspot.com/");
        byte[] bytes = File.ReadAllBytes(path);
        //Editing Metadata
        var newMetadata = new MetadataChange();
        newMetadata.ContentType = _fileName;

        //Create a reference to where the file needs to be uploaded
        uploadRef = storageReference.Child(_fileName);
        uploadRef.PutBytesAsync(bytes, newMetadata).ContinueWithOnMainThread((task) =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                Debug.Log("File Uploaded Successfully!");
                storageReference.Child(_fileName).GetDownloadUrlAsync().ContinueWithOnMainThread((urltask) => 
                {
                    if (!urltask.IsFaulted && !urltask.IsCanceled) {
                        writeStringToFile(urltask.Result.ToString(), "links.txt");
                    }
                    else{
                        Debug.Log("Failed");
                    }
                });
            }
        });
        
        
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


    public void writeStringToFile(string str, string filename)
    {
        string path = pathForDocumentsFile(filename);
        Debug.Log(str);
        Debug.Log(path);
        FileStream file = new FileStream(path, FileMode.Append, FileAccess.Write);

        StreamWriter sw = new StreamWriter(file);
        sw.WriteLine(str);

        sw.Close();
        file.Close();
    }

}
