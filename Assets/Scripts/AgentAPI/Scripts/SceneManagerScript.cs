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

    public static string username = "Nutzer";
    public static List<string> exercises = new List<string>
    {
        "positiveRückmeldung",
        "dankbarkeit",
        "staerken",
        "alleFarben"
    };

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void ConfirmName()
    {
        //username = "ulrike";
    }
}
