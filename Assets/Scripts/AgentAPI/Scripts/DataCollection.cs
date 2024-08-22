using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class DataCollection : MonoBehaviour
{
    public static string conversationTranscription = "DateTime: " + DateTime.Now.ToString() + ". Conversation: ";
    private string content;
    private string fileName;
    private bool columnNames;
    private static string path;

    

    private static int testInt = 44;

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
        
        path = Path.Combine(path, "conversation_"+ "test" + ".csv");
    }


    public static void LogGameData()
    {
        
        if(true)
        {
            
            string columnString = "ID;"+ "DateTime;" + "Test;"+"Conversation;" + "\n"; 
            string logString = "******;" + DateTime.Now.ToString() + ";" + testInt.ToString()+ ";" + conversationTranscription + ";";

            // add column names only when file is created
            if (!File.Exists(path))
            {
                File.WriteAllText(path, columnString);
                
            }
            // always save data recorded during one run
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(logString);
            }
        }
    }

    public void AddContent(string newContent)
    {
        content += newContent;
    }
}

