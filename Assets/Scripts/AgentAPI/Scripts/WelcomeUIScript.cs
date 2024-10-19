using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeUIScript : MonoBehaviour
{
    public Button humanVisualHumanChatButton;
    public Button machineVisualMachineChatButton;
    public Button machineVisualHumanChatButton;
    public Button humanVisualMachineChatButton;

    //public Agent agentScript;
    public Button quitButton;
    public TMP_Text logText;

    public TMP_InputField UsernameInputField;

    // Start is called before the first frame update
    void Start()
    {
        if (humanVisualHumanChatButton != null)
        {
            humanVisualHumanChatButton.gameObject.SetActive(false);
            //SceneManagerScript.humanVisualHumanChatDone = false;
            humanVisualHumanChatButton.onClick.AddListener(OnHumanVisualHumanChatButtonClicked);
        }

        if (humanVisualMachineChatButton != null)
        {
            humanVisualMachineChatButton.gameObject.SetActive(false);
            //SceneManagerScript.humanVisualMachineChatDone = false;
            humanVisualMachineChatButton.onClick.AddListener(OnHumanVisualMachineChatButtonClicked);
        }

        if (machineVisualMachineChatButton != null)
        {
            machineVisualMachineChatButton.gameObject.SetActive(false);
            //SceneManagerScript.machineVisualMachineChatDone = false;
            machineVisualMachineChatButton.onClick.AddListener(
                OnMachineVisualMachineChatButtonClicked
            );
        }

        if (machineVisualHumanChatButton != null)
        {
            machineVisualHumanChatButton.gameObject.SetActive(false);
            //SceneManagerScript.machineVisualHumanChatDone = false;
            machineVisualHumanChatButton.onClick.AddListener(OnMachineVisualHumanChatButtonClicked);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(Application.Quit);
        }

        if (SceneManagerScript.startingScene)
        {
            UsernameInputField.onEndEdit.AddListener(
                delegate(string text)
                {
                    SceneManagerScript.username = text;
                    humanVisualHumanChatButton.gameObject.SetActive(!string.IsNullOrEmpty(text));
                    machineVisualMachineChatButton.gameObject.SetActive(
                        !string.IsNullOrEmpty(text)
                    );
                    machineVisualHumanChatButton.gameObject.SetActive(!string.IsNullOrEmpty(text));
                    humanVisualMachineChatButton.gameObject.SetActive(!string.IsNullOrEmpty(text));
                    SceneManagerScript.startingScene = false;
                }
            );
        }
        else
        {
            Debug.Log(
                "conditions done: hh: "
                    + SceneManagerScript.humanVisualHumanChatDone
                    + "mm: "
                    + SceneManagerScript.machineVisualMachineChatDone
                    + "mh"
                    + SceneManagerScript.machineVisualHumanChatDone
                    + "hm"
                    + SceneManagerScript.humanVisualMachineChatDone
            );
            humanVisualHumanChatButton.gameObject.SetActive(
                !SceneManagerScript.humanVisualHumanChatDone
            );
            machineVisualMachineChatButton.gameObject.SetActive(
                !SceneManagerScript.machineVisualMachineChatDone
            );
            machineVisualHumanChatButton.gameObject.SetActive(
                !SceneManagerScript.machineVisualHumanChatDone
            );
            humanVisualMachineChatButton.gameObject.SetActive(
                !SceneManagerScript.humanVisualMachineChatDone
            );
            UsernameInputField.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update() { }

    void OnHumanVisualHumanChatButtonClicked()
    {
        logText.text = "Hallo " + SceneManagerScript.username;

        SceneManager.LoadScene(1);
        // if (agentScript != null)
        // {
        //     agentScript.ChatExample_Script.StartChatExample(agentScript.Username, true);

        //     //disable start and switch button
        //     vrStartButton.gameObject.SetActive(false);
        // }
    }

    void OnMachineVisualMachineChatButtonClicked()
    {
        SceneManager.LoadScene(2);
    }

    void OnMachineVisualHumanChatButtonClicked()
    {
        SceneManager.LoadScene(3);
    }

    void OnHumanVisualMachineChatButtonClicked()
    {
        SceneManager.LoadScene(4);
    }
}
