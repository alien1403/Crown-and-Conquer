using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

public class FileDataHandler
{
    private string dataDirectoryPath = string.Empty;
    private string dataFileName = string.Empty;
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "echipa the weeknd";
    public FileDataHandler(string dataDirectoryPath, string dataFileName, bool useEncryption)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if(useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + ex);
            }
        }
        return loadedData;
    }
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data);
            if(useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + ex);
        }
    }
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    public void DeleteSavedData()
    {
        string filePath = Path.Combine(dataDirectoryPath, dataFileName);
        if(File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log("Deleted the file");
            }
            catch(Exception e)
            {
                Debug.LogError("Error deleting the file: " + e);
            }
        }
    }
}
