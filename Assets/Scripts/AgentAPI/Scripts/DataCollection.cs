using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DataCollection : MonoBehaviour
{
    public static string conversationTranscription =
        "DateTime: " + DateTime.Now.ToString() + ". Conversation: ";

    public static TimeSpan speakingDuration;
    private string content;
    private string fileName;
    private bool columnNames;
    private static string path;
    private static string condition;

    public static TimeSpan sessionDuration;

    void Start()
    {
        SetUpPaths();
    }

    void OnApplicationQuit()
    {
        Debug.Log("Quitting");
        LogGameData();
    }

    public void SetUpPaths()
    {
        System.Random random = new System.Random();
        int seed = random.Next(1000, 10000);

        path = @"" + Application.persistentDataPath + "/logs/";
        Debug.Log(path);
        Directory.CreateDirectory(path);
        string name = $"conversation_logs_{DateTime.Now:yyyyMMdd_HHmm}.csv";

        path = Path.Combine(path, name);
    }

    public static void LogGameData()
    {
        checkCondition();

        string columnString =
            "ID;"
            + "DateTime;"
            + "Condition;"
            + "Task;"
            + "Conversation;"
            + "User_Speaking_Duration;"
            + "Duration_Of_Session;"
            + "\n";
        string logString =
            "******;"
            + DateTime.Now.ToString()
            + ";"
            + condition
            + ";"
            + ChatExample.task
            + ";"
            + conversationTranscription
            + ";"
            + speakingDuration
            + ";"
            + sessionDuration
            + ";";

        // Add column names only when the file is created
        if (!File.Exists(path))
        {
            // Write with BOM for UTF-8 encoding
            using (var sw = new StreamWriter(path, false, new UTF8Encoding(true)))
            {
                sw.Write(columnString);
            }
        }

        // Replace the username with "AnonymousUserName" and append the data as a new line
        logString = logString.Replace(ChatExample.username, "AnonymousUserName");

        // Append the log string as a new line in the file using UTF-8 encoding
        using (StreamWriter sw = new StreamWriter(path, true, new UTF8Encoding(true)))
        {
            sw.WriteLine(logString);
        }
    }

    public static void checkCondition()
    {
        if (SceneManagerScript.humanVisual)
        {
            if (SceneManagerScript.humanChat)
            {
                condition = "gelb-humanVisual-humanChat";
            }
            else
            {
                condition = "blau-humanVisual-machineChat";
            }
        }
        else
        {
            if (SceneManagerScript.humanChat)
            {
                condition = "gruen-machineVisual-humanChat";
            }
            else
            {
                condition = "rot-machineVisual-machineChat";
            }
        }
    }

    public void AddContent(string newContent)
    {
        content += newContent;
    }
}
