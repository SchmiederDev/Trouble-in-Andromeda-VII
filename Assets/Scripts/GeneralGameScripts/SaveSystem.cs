using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void Save(SaveGame saveGame)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/TOASGs.toas";
        FileStream fileStream = new FileStream(filePath, FileMode.Create);

        SaveGame dataToStore = new SaveGame(saveGame.TotalXP, saveGame.LevelsPlayed);

        binaryFormatter.Serialize(fileStream, dataToStore);    
        fileStream.Close();
    }

    public static SaveGame Load()
    {
        string filePath = Application.persistentDataPath + "/TOASGs.toas";

        if(File.Exists(filePath))
        {
            BinaryFormatter binaryFormatter =new BinaryFormatter(); 
            FileStream fileStream = new FileStream(filePath, FileMode.Open);

            SaveGame loadedData = binaryFormatter.Deserialize(fileStream) as SaveGame;

            fileStream.Close();
            return loadedData;
        }

        else
        {
            Debug.Log("File not found.");
            return null;
        }
    }
}
