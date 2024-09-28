using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DataCollection : MonoBehaviour
{
    public static string conversationTranscription =
        "DateTime: " + DateTime.Now.ToString() + ". Conversation: ";
    private string content;
    private string fileName;
    private bool columnNames;
    private static string path;
    private static string condition;

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
        path = @"" + Application.persistentDataPath + "/logs/";
        Debug.Log(path);
        Directory.CreateDirectory(path);

        path = Path.Combine(path, "conversation_" + "test" + ".csv");
    }

    public static void LogGameData()
    {
        checkCondition();

        if (true)
        {
            string columnString =
                "ID;" + "DateTime;" + "Condition;" + "Task;" + "Conversation;" + "\n";
            string logString =
                "******;"
                + DateTime.Now.ToString()
                + ";"
                + condition
                + ";"
                + ChatExample.task
                + ";"
                + conversationTranscription
                + ";";

            // add column names only when file is created
            if (!File.Exists(path))
            {
                File.WriteAllText(path, columnString);
            }
            // always save data recorded during one run
            using (StreamWriter sw = File.AppendText(path))
            {
                logString = logString.Replace(SceneManagerScript.username, "AnonymousUserName");
                sw.WriteLine(logString);
            }
        }
    }

    public static void checkCondition()
    {
        if (SceneManagerScript.humanVisual)
        {
            if (SceneManagerScript.humanChat)
            {
                condition = "humanVisual-humanChat";
            }
            else
            {
                condition = "humanVisual-machineChat";
            }
        }
        else
        {
            if (SceneManagerScript.humanChat)
            {
                condition = "machineVisual-humanChat";
            }
            else
            {
                condition = "machineVisual-machineChat";
            }
        }
    }

    public void AddContent(string newContent)
    {
        content += newContent;
    }
}
