using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private string savePath, oldSavePath;
    private bool doneOnce = false;

    private void Start()
    {
        oldSavePath = Application.persistentDataPath + "/" + ManagerVar.Instance.lastTagID + ".json";
    }

    private void Update()
    {
        savePath = Application.persistentDataPath + "/" + ManagerVar.Instance.lastTagID + ".json";

        if(savePath != oldSavePath)
        {
            doneOnce = false;
        }
        
        if (!doneOnce)
        {
            if (!string.IsNullOrEmpty(ManagerVar.Instance.lastTagID))
            {
                if (File.Exists(savePath))
                {
                    Load();
                    Debug.Log("Save file loaded from path: " + savePath);
                }
                else
                {
                    CreateNewSaveFile();
                    Debug.Log("New save file created at path: " + savePath);
                }
                oldSavePath = savePath;
            }
            else
            {
                Debug.LogWarning("ManagerVar.Instance.lastTagID is null or empty, no save file loaded.");
            }
            doneOnce = true;
        }

        if (ManagerVar.Instance.shouldSave)
        {
            Save();
            ManagerVar.Instance.shouldSave = false;
        }
    }

    private void Save()
    {
        string jsonData = JsonUtility.ToJson(ManagerVar.Instance);
        File.WriteAllText(savePath, jsonData);
        Debug.Log("Save file saved to path: " + savePath);
    }

    private void Load()
    {
        string jsonData = File.ReadAllText(savePath);
        JsonUtility.FromJsonOverwrite(jsonData, ManagerVar.Instance);
    }

    private void CreateNewSaveFile()
    {
        string jsonData = JsonUtility.ToJson(ManagerVar.Instance);
        File.WriteAllText(savePath, jsonData);
        Debug.Log("New save file created at path: " + savePath);
        Save();
    }
}
