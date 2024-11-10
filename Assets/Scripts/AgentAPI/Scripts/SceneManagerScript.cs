using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static bool humanVisual = true;
    public static bool humanChat = true;
    public static bool humanVisualHumanChatDone = false;
    public static bool humanVisualMachineChatDone = false;
    public static bool machineVisualMachineChatDone = false;
    public static bool machineVisualHumanChatDone = false;
    public static bool startingScene = true;

    public static string exerciseString = "exercise string";

    // public static int taskDuration = 30;

    public static string username = "Nutzer";
    public static List<string> exercises = new List<string>
    {
        "positiveRückmeldung",
        "dankbarkeit",
        "staerken",
        "alleFarben"
    };

    public static List<string> shuffledExercises = new List<string>(exercises);

    public static Dictionary<string, string> conditionExerciseMapping =
        new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        // Shuffle the exercises list
        if (startingScene)
        {
            ShuffleList(shuffledExercises);
        }

        Debug.Log("List contents: " + string.Join(", ", shuffledExercises));
        exerciseString = "List contents: " + string.Join(", ", shuffledExercises);

        // SceneManager.LoadScene(3);
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
