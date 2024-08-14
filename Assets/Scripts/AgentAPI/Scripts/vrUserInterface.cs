using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class vrUserInterface : MonoBehaviour
{
    public Button vrStartButton;
    public Agent agentScript;

    public TMP_Text logText;

    public Button switchAgentButton; 
    public GameObject robot; 
    public GameObject hannah;
    public bool hannahActive = true;

    void Start()
    {
        // Ensure the button and targetObject are assigned
        if (switchAgentButton != null && robot != null)
        {
            // Add a listener to the button to call the MoveObject method when clicked
            switchAgentButton.onClick.AddListener(SwapAgents);
            logText.text = "Switch agent button pressed.";
        }

        logText.text = "hallo";
        if (vrStartButton != null)
        {
            vrStartButton.onClick.AddListener(OnVRStartButtonClicked);
        }
        else
        {
            logText.text = "VR Start Button not assigned in the Inspector";
        }

        // if hannah is active at start, move robot away
        if (hannahActive)
        { 
            robot.transform.position = new Vector3(1000, 1000, 1000);
        }


    }

    void SwapAgents()
    {
        if (hannahActive)
        { 
            robot.transform.position = Vector3.zero;
            hannah.transform.position = new Vector3(1000, 1000, 1000);
        }
        else
        {
            hannah.transform.position = Vector3.zero;
            robot.transform.position = new Vector3(1000, 1000, 1000);
        }
        hannahActive = !hannahActive;
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
            

            //disable start and switch button
            vrStartButton.gameObject.SetActive(false);
            switchAgentButton.gameObject.SetActive(false);
        }
        else
        {
            ChangeButtonColor(vrStartButton, Color.red);
            logText.text = "Agent script not found in the scene";
        }
    }

    
}
