using System.IO;
using UnityEngine;

public class LogWriter : MonoBehaviour
{
    string fileName = "/LogFile.Text";
    string metaFile = ".meta";

    string filePath;
    string metaFilePath;

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.dataPath + fileName;
        metaFilePath = filePath + metaFile;

        CheckForLogFile_and_DeleteOnStart();
    }

    private void CheckForLogFile_and_DeleteOnStart()
    {
        if(File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        if(File.Exists(metaFilePath))
        {
            File.Delete(metaFilePath);
        }
    }

    private void Log(string logTxt, string stackTrace, LogType log)
    {
        TextWriter writer = new StreamWriter(filePath, true);

        writer.WriteLine("[" + System.DateTime.Now + "] " + logTxt);

        writer.Close();
    }

}
