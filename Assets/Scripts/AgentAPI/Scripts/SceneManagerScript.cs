using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerScript : MonoBehaviour
{
    public static bool humanVisual = true;
    public static bool humanChat = true;
    public static bool humanVisualHumanChatDone = false;
    public static bool humanVisualMachineChatDone = false;
    public static bool machineVisualMachineChatDone = false;
    public static bool machineVisualHumanChatDone = false;
    public static bool startingScene = true;

    // public static int taskDuration = 30;

    public static string username = "Nutzer";
    public static List<string> exercises = new List<string>
    {
        "positiveRückmeldung",
        "dankbarkeit",
        "staerken",
        "alleFarben"
    };

    public static Dictionary<string, string> conditionExerciseMapping =
        new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        // List of conditions
        List<string> conditions = new List<string>
        {
            "humanVisualHumanChat",
            "humanVisualMachineChat",
            "machineVisualMachineChat",
            "machineVisualHumanChat"
        };

        // Shuffle the exercises list
        List<string> shuffledExercises = new List<string>(exercises);
        ShuffleList(shuffledExercises);

        // Map each condition to a shuffled exercise
        for (int i = 0; i < conditions.Count; i++)
        {
            conditionExerciseMapping[conditions[i]] = shuffledExercises[i];
        }
    }

    // Update is called once per frame
    void Update() { }

    public void ConfirmName()
    {
        //username = "ulrike";
    }

    // Method to shuffle a list
    private void ShuffleList(List<string> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
