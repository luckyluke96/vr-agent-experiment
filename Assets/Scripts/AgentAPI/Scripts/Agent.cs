using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Agent : MonoBehaviour
{
    public TTSAPI TextToSpeech;
    public NLPAPI LanguageProcessing;
    public MicrophoneRecorder SpeechToText;

    public AgentState State = AgentState.Start;
    public bool startAutomatically = false;
    private int sessionDuration = 10;

    private int NumberOfAPICalls = 0;

    private long LastSTTTime = 0;
    private bool WaitingForFinalSTT = false;

    // Current Script
    public ChatExample ChatExample_Script;

    public enum AgentState
    {
        Start,
        Initializing,
        Listening,
        Processing,
        Talking,
    }

    private string startingPromp =
        ""
        + "The folowing is a conversation with an AI assistant for a psychotherapist"
        + "The assistant is helpful, creative, clever, and very friendly. "
        + "They can give known hints about psychiatric issues.\n\n"
        + "Human: Hi.\n\n"
        + "The doctor's AI assistant introduces itself and asks for the patient's name:\n\n"
        + "AI:";

    Func<string, string> initialPrompt = (i) =>
    {
        return ""
            + "The following is a conversation with the assistant Marc for doctor Gallinat. "
            + "The assistant is helpful, creative, clever, and very friendly.\n\n"
            + "Patient: Hello?\n\n"
            + "The assistant introduces itself and asks for the patient's name:\n\n"
            + "AI:";
    };

    public TMP_InputField UsernameInputField;
    public string Username = "";
    private bool Username_Confirmed = true;

    private string currentPrompt = "";

    // Start is called before the first frame update
    void Start()
    {
        // PHQ9_Script = new PHQ9();
        currentPrompt = startingPromp;
        // UsernameInputField.onValueChanged.AddListener(
        //     delegate
        //     {
        //         Username = UsernameInputField.text;
        //     }
        // );
        Username = SceneManagerScript.username;

        // Assume all necessary conditions are met
        if (startAutomatically)
        {
            StartCoroutine(Waiter());
        }

        StartCoroutine(CountDown());
        //ChatExample_Script.StartChatExample(Username, true);
    }

    IEnumerator CountDown()
    {
        ChatExample.timeIsUp = false;
        yield return new WaitForSeconds(sessionDuration);
        Debug.Log("Waiting over");
        ChatExample.endConv = true;
        Debug.Log("End Conv is now " + ChatExample.endConv);
    }

    IEnumerator Waiter()
    {
        Debug.Log("Wait for 2 seconds");
        //Wait for 2 seconds
        yield return new WaitForSeconds(2);

        Debug.Log("Waiting over");
        ChatExample_Script.StartChatExample(Username, true);

        //Debug.Log("finish");
        //StartCoroutine(ChatExample_Script.FinishConversation());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void IncomingTranscription(AudioAPI.TranscriptionResult transcriptionResult)
    {
        // If the result is final, or we waited 5 seconds without input, we finalize the input
        var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        if (LastSTTTime == 0)
        {
            LastSTTTime = now;
        }

        if (WaitingForFinalSTT && State != AgentState.Processing && transcriptionResult.isFinal)
        {
            WaitingForFinalSTT = false;
            Debug.Log($"Final: {transcriptionResult.alternatives[0].transcript}");
            State = AgentState.Processing;
            UnityMainThreadDispatcher
                .Instance()
                .Enqueue(() =>
                {
                    currentPrompt +=
                        transcriptionResult.alternatives[0].transcript + $"\n\n{AgentSettings.AI}:";
                    ;
                    LanguageProcessing.GetNLPResponse(
                        currentPrompt,
                        NLPAPI.GPT_Models.Davinci_Best,
                        ProcessNLP
                    );
                });
        }
        else
        {
            if (LastSTTTime + 5000 < now)
            {
                transcriptionResult.isFinal = true;
                SpeechToText.StopRecording();
                IncomingTranscription(transcriptionResult);
            }
        }

        LastSTTTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Debug.Log(now - LastSTTTime);
    }

    private void ProcessNLP(string response)
    {
        NumberOfAPICalls++;

        if (NumberOfAPICalls >= 200)
        {
            TextToSpeech.TextToSpeechAndPlay(
                response,
                () =>
                {
                    if (AgentSettings.LanguageString == "en-US")
                    {
                        TextToSpeech.TextToSpeechAndPlay(
                            "Thank you, if you want to make more requests, please restart.",
                            () => { }
                        );
                    }
                    else
                    {
                        TextToSpeech.TextToSpeechAndPlay(
                            "Vielen Dank, wenn sie ein neues Gespräch führen wollen, bitte neustarten.",
                            () => { }
                        );
                    }
                }
            );
        }
        else
        {
            currentPrompt += response + $"\n\n{AgentSettings.Human}:";

            State = AgentState.Talking;
            TextToSpeech.TextToSpeechAndPlay(
                response,
                () =>
                {
                    LastSTTTime = 0;
                    WaitingForFinalSTT = true;
                    // SpeechToText.StartRecording();
                }
            );
        }
    }

    public void SetName(string name)
    {
        Username = name;
    }

    public void ConfirmName()
    {
        if (Username == "")
        {
            Username = "User";
        }

        Username_Confirmed = true;
    }

    private void OnGUI()
    {
        GUI.color = Color.red;
        // Top right label "Escape to exit"
        GUI.Label(new Rect(Screen.width - 150, 10, 150, 20), "Escape to exit");
        GUI.color = Color.white;

        if (!SpeechToText.SelectedMicrophone || !Username_Confirmed)
        {
            return;
        }

        int y = 0;

        if (State == AgentState.Start)
        {
            GUI.color = Color.blue;
            if (GUI.Button(new Rect(10, 60 * y++, 350, 40), "* Start ChatExample"))
            {
                AgentSettings.LanguageString = "de-DE";
                State = AgentState.Initializing;
                Debug.Log("Start chat from gui");
                ChatExample_Script.StartChatExample(Username, true);
            }
        }
        else
        {
            if (GUI.Button(new Rect(10, 60, 350, 40), "Restart"))
            {
                NumberOfAPICalls = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (GUI.Button(new Rect(10, 120, 350, 40), "EXIT"))
            {
                Application.Quit();
            }
        }
    }
}
