using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class vrUserInterface : MonoBehaviour
{
    public Button vrStartButton;
    public Agent agentScript;

    public TMP_Text logText;

    void Start()
    {
        logText.text = "hallo";
        if (vrStartButton != null)
        {
            vrStartButton.onClick.AddListener(OnVRStartButtonClicked);
        }
        else
        {
            logText.text = "VR Start Button not assigned in the Inspector";
        }
    }

    private void ChangeButtonColor(Button button, Color color)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        button.colors = cb;

        // Optionally, revert color back after a delay
        Invoke("ResetButtonColor", 1f);
    }

    void OnVRStartButtonClicked()
    {
        

        if (agentScript != null)
        {
            logText.text = "VR Start Button clicked, starting chat example";
            ChangeButtonColor(vrStartButton, Color.green);
            logText.text = "VR Start Button clicked, starting chat example";
            agentScript.ChatExample_Script.StartChatExample(agentScript.Username, true);
            vrStartButton.gameObject.SetActive(false);
        }
        else
        {
            ChangeButtonColor(vrStartButton, Color.red);
            logText.text = "Agent script not found in the scene";
        }
    }

    
}
