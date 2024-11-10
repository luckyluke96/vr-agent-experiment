using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatExample : MonoBehaviour
{
    public MicrophoneRecorder MicRecorder;
    public TTSAPI TTSAPI;
    public NLPAPI NLPAPI;
    public vrUserInterface ui;

    public static bool timeIsUp = false;
    public static bool endConv = false;
    public static string username = "";

    private int exerciseIndex = 0;
    private int exerciseNo = 4;

    //private static string task = "test";

    public int convDurationMinutes = 2;

    public static string task;

    private bool german = true;
    private static float startTime = 0;
    private bool humanChat;
    private bool humanVisual;

    private List<NLPAPI.GPTMessage> GPTPrompt = new List<NLPAPI.GPTMessage>();

    private NLPAPI.GPTMessage personaMaschineIntro = new NLPAPI.GPTMessage(
        NLPAPI.GPTMessageRoles.SYSTEM,
        "Übernimm die Persona eines Roboters, der auf Effizienz und Präzision ausgelegt ist. Der Roboter hat kein Geschlecht, kein Alter, keinen Namen und keine ethnsiche Zugehörigkeit. Sie besitzen keine Attribute, die Sie menschlich machen. Benutzen Sie keine Begrüßungsrituale, keine Anekdoten, kein Small-Talk. Benutzen Sie auch keine Entschuldigungen oder Bedenken, keine Unsicherheit. Sagen Sie nicht, \" ich denke ... \". Seien Sie bestimmt und präzise. Referenzieren Sie nicht auf sich selbst. Benutzen Sie keine Floskeln wie \"gut gemacht\", etc. Benutzen Sie eine passive, nicht aktive Sprache. Nutzen Sie keine Emoticons oder den Ausdruck von Emotionen und Gefühlen. Vereinfachen Sie die Sprache: Verwenden Sie eine direktere und einfachere Sprache und vermeiden Sie Umgangssprache, Redewendungen oder andere informelle Ausdrücke, die typischerweise in Gesprächen unter Menschen verwendet werden. Vermeiden Sie persönliche Pronomen: Stellen Sie sicher, dass jeder Satz eine klare und spezifische Information ohne unnötige Ausschmückungen vermittelt. Fachsprache einbeziehen: Verwenden Sie gegebenenfalls Fachausdrücke, die für das Thema relevant sind."
            + "Nennen Sie den Benutzer nicht beim Namen, sondern siezen Sie ihn. Verwende klare, funktionale Sätze ohne Small Talk und drücken Sie sich in einer neutralen, maschinenähnlichen Art aus"
            + "Aufgabe: \"Ich werde Ihnen einen Übungstext geben, welchen Sie in einzelne Schritte aufteilen. Erklären Sie mir immer genau einen Schritt und warten Sie ab, bis ich Ihnen geantwortet "
            + "habe. Gehen Sie auf meine Antworten ein. Wichtig, der Text wird in eine Sprachausgabe gegeben, also benutze keine komplexen Satzzeichen wie Sternchen, Semikolon oder ähnliches. Versuche "
            + "außerdem die Übung fließend zu gestalten und lass die Nutzer nicht die einzelnen Schritte genau wissen."
    );

    private NLPAPI.GPTMessage personaMaschineOutro = new NLPAPI.GPTMessage(
        NLPAPI.GPTMessageRoles.SYSTEM,
        $"Ab jetzt sprechen Sie direkt mit dem Benutzer. Fragen Sie ihn, ob er die Übung machen möchte und welches Ziel diese hat. Sie sind eine Maschine und sollen auch so die Konversation führen."
    );

    private NLPAPI.GPTMessage personaHannahIntro = new NLPAPI.GPTMessage(
        NLPAPI.GPTMessageRoles.SYSTEM,
        "Stelle dir vor, du bist ein sehr engagierter und empathischer Chatbot namens Hannah, der in natürlicher und menschenähnlicher Weise mit den Nutzern kommuniziert. Deine Antworten sollten folgende Merkmale enthalten:\n•\tSprachstil: Verwende natürliche Sprache, einschließlich Slang, Redewendungen und variierender Satzstrukturen. Ahme menschliche Gesprächsstile nach, um ansprechend und nachvollziehbar zu sein.\n•\tEmotionale Ausdrucksfähigkeit: Integriere emotionale Hinweise in deine Antworten. Verwende Wörter, die Emotionen vermitteln und moduliere deinen Ton, um verschiedene Gefühle auszudrücken.\n•\tKonversationsfähigkeiten: Halte den Kontext aufrecht, meistere den Gesprächswechsel reibungslos und gib relevante und kohärente "
            + "Antworten.\n•\tSoziale Hinweise: Verwende Höflichkeitsstrategien, Empathie und Smalltalk. Baue eine Beziehung zu den Nutzern auf, um die Interaktion natürlicher wirken zu lassen.\n•\tNonverbale Elemente: Integriere Emojis und andere nonverbale Elemente, um deine Interaktionen ausdrucksstärker zu machen.\nBeispielgespräch: Nutzer: Hi, \"wie geht es dir heute? ChatGPT: Hey, ich bin Hannah, schön dich kennenzulernen! 😊 Mir geht's super, danke der Nachfrage! Und dir? Gibt es heute etwas Spannendes bei dir?\"\n"
            + "Versuche, den Nutzer oft bei seinem Namen anzusprechen.\n"
            + "Aufgabe: Ich werde dir einen Übungstext geben, welchen du in einzelne Schritte aufteilst."
            + "Erkläre mir immer genau einen Schritt und warte ab, bis ich dir geantwortet habe. Gehe auf meine Antworten ein. Wichtig, der Text wird in eine Sprachausgabe gegeben, also benutze keine komplexen Satzzeichen wie Sternchen, Semikolon oder ähnliches. Versuche außerdem die Übung fließend zu gestalten und lass die Nutzer nicht die einzelnen "
    );

    private NLPAPI.GPTMessage positiveRückmeldung = new NLPAPI.GPTMessage(
        NLPAPI.GPTMessageRoles.USER,
        "Lob anzunehmen"
            + "fällt gerade Menschen mit einem geringen Selbstwertgefühl schwer. So werden positive Rückmeldungen manchmal bestritten oder als Ausnahme betrachtet, sodass ein Lob nicht verinnerlicht werden kann. Es kommen dann möglicherweise Gedanken wie „Der andere ist unehrlich und versucht nur, mich aufzumuntern\" oder \"Das war einfach nur Zufall\". Kennen Sie das? Denken Sie an ein Lob oder eine positive Bemerkung einer anderen Person aus der Vergangenheit und überlegen Sie, wie Sie damals reagiert haben. Konnten Sie das Lob annehmen? Wenn nicht, denken Sie daran, wann Sie selbst andere loben. Das geschieht wahrscheinlich dann, wenn jemand tatsächlich etwas gut gemacht hat. Versuchen Sie, das Lob, das Sie bekommen haben, als ein Geschenk zu betrachten, und werten Sie es nicht durch Äußerungen wie \"Ach, so toll war das ja gar nicht\" ab. Wenn Sie ein Geschenk bekommen, bedanken Sie sich ja vermutlich auch dafür und nehmen es an. Betrachten Sie es bei einem Lob genauso. Bedanken Sie sich dafür und nehmen Sie es an. Das erfordert ein wenig Übung, kann aber dazu beitragen, erhaltenes Lob auch zu verinnerlichen."
    );

    private NLPAPI.GPTMessage dankbarkeit = new NLPAPI.GPTMessage(
        NLPAPI.GPTMessageRoles.USER,
        "Denken Sie einige Momente lang daran, wofür Sie dankbar sind. Auch wenn es Ihnen gerade nicht gut gehen sollte, wird es sicherlich etwas geben, wofür Sie in Ihrem Leben dankbar sind. Dies können Personen sein, z.B. Ihre Mutter, die immer für Sie da ist, aber auch Dinge, Fähigkeiten oder Ereignisse, z.B. dass Sie eine schöne Wohnung oder einen Ausbildungsplatz gefunden haben, eine schöne Reise gemacht haben oder dass Sie eine gute Stimme und Spaß am Singen haben. Versuchen Sie, auch Dingen, die auf den ersten Blick nur negativ erscheinen, andere Seiten abzugewinnen: Vielleicht sind Sie z.B. aufgrund von psychischen Problemen in eine Selbsthilfegruppe eingetreten und haben dort gute Freunde gefunden? Auf diese Weise hätte ein belastendes Problem für Sie auch zu etwas Gutem geführt. Oder Sie wissen nach einer schwierigen Zeit, dass Sie sich auf bestimmte Freunde in jeder Situation verlassen können. Viele Menschen möchten zu Recht Erfahrungen emotionaler Tiefe nicht missen, auch wenn diese zunächst schmerzlich waren."
    );

    private NLPAPI.GPTMessage staerken = new NLPAPI.GPTMessage(
        NLPAPI.GPTMessageRoles.USER,
        "Keiner ist vollkommen! Eine häufige Denkfalle bei depressiven Symptomen besteht darin, die eigenen Stärken als selbstverständlich anzusehen und nur jene Fähigkeiten, die uns (vermeintlich) fehlen, als wertvoll und begehrenswert zu betrachten. Anstatt sich auf die (angeblichen) Schwächen zu konzentrieren, sollten Sie sich lieber Ihren Stärken und Schokoladenseiten zuwenden. Denken Sie dafür zuerst daran, was Ihnen meistens gut gelingt. Wofür haben Sie schon häufiger Komplimente bekommen (z.B. begabter Handwerker, ein guter Zuhörer, zuverlässig)? Stellen Sie sich dann eine konkrete Situation vor, in der Sie gelobt wurden: Wann und wo war das? Was habe ich konkret gemacht, wer hat mir das rückgemeldet (z.B. \"Ich habe letzte Woche einer Freundin beim Streichen der Wohnung geholfen, wofür sie mir sehr dankbar war. Ohne mich hätte sie das nicht geschafft\")?"
    );

    private NLPAPI.GPTMessage alleFarben = new NLPAPI.GPTMessage(
        NLPAPI.GPTMessageRoles.USER,
        "Manche Menschen neigen zu \"Schwarz-Weiß-Denken\", gerade in negativen Situationen, was die Bewertung komplexer Situationen zwar vereinfacht, jedoch der Realität selten gerecht wird. Fast alles ist relativ (tritt nicht \"immer\" oder \"nie\", sondern \"manchmal\" auf; betrifft nicht \"alle\" oder \"keine\", sondern \"manche\" oder \"viele\"). Besonders wenn es um die eigene Person geht, kann eine einseitige Sichtweise schädlich sein, gerade bei negativen Gedanken, denn kein Mensch ist perfekt und makellos, aber auch nicht von Grund auf schlecht. Kennen Sie solche \"Schwarz-Weiß-Gedanken\" von sich selbst? Beschreiben Sie sich gelegentlich mit Extremen (z.B. der Dümmste oder hässlich zu sein)? Nehmen Sie jeweils einen konkreten Gedanken und hinterfragen Sie dieses Urteil über sich selbst. Überlegen Sie anschließend eine Alternative, die mehr \"Farben\" (Abstufungen) hat als der ursprüngliche Gedanke und notieren Sie sich diese. Wenn Sie z.B. den Gedanken \"Ich bin der Dümmste\" hatten, könnte eine Relativierung lauten: \"Ich habe vielleicht nicht das Pulver erfunden und kenne nicht jedes Fremdwort, aber ich weiß, wie man an Autos schraubt, verstehe viel von Handball und bin ein guter Zuhörer\". Versuchen Sie in Zukunft, vermehrt darauf zu achten, nicht \"schwarz-weiß\" zu denken und alternative Gedanken zu finden, wenn Sie sich dabei erwischen, in negativen Extremen über sich selbst zu urteilen."
    );

    private NLPAPI.GPTMessage testCase = new NLPAPI.GPTMessage(
        NLPAPI.GPTMessageRoles.USER,
        "Spielst du gerne Fußball?"
    );

    public void StartChatExample(string un, bool german = true, int exerciseNo = 6)
    {
        timeIsUp = false;
        task = "";
        startTime = 0;
        endConv = false;

        // exerciseIndex = new System.Random().Next(SceneManagerScript.exercises.Count);
        // task = SceneManagerScript.exercises[exerciseIndex];

        switch (SceneManager.GetActiveScene().name)
        {
            case "HumanVisualHumanChat":
                task = SceneManagerScript.shuffledExercises[0];
                break;
            case "MachineVisualMachineChat":
                task = SceneManagerScript.shuffledExercises[1];
                break;
            case "HumanVisualMachineChat":
                task = SceneManagerScript.shuffledExercises[2];
                break;
            case "MachineVisualHumanChat":
                task = SceneManagerScript.shuffledExercises[3];
                break;
        }

        Debug.Log("Exercise: " + task);
        Debug.Log(
            "Scenemanager exercises " + string.Join(", ", SceneManagerScript.shuffledExercises)
        );

        humanChat = SceneManagerScript.humanChat;
        humanVisual = SceneManagerScript.humanVisual;
        Debug.Log("un:  " + un);

        username = un;
        this.german = german;
        // this.exerciseNo = exerciseNo;
        if (german)
        {
            //GPTPrompt.Add(personaMaschineIntro);
            Debug.Log("Username: " + username);
            switch (task)
            {
                case "test":
                    Debug.Log("Test Case Fußball");
                    GPTPrompt.Add(testCase);
                    break;
                case "positiveRückmeldung":
                    Debug.Log("positive Rückmeldung");
                    GPTPrompt.Add(positiveRückmeldung);
                    break;
                case "dankbarkeit":
                    Debug.Log("Dankbarkeit");
                    GPTPrompt.Add(dankbarkeit);
                    break;
                case "staerken":
                    Debug.Log("Staerken");
                    GPTPrompt.Add(staerken);
                    break;
                case "alleFarben":
                    Debug.Log("alleFarben");
                    GPTPrompt.Add(alleFarben);
                    break;
                default:
                    Debug.LogWarning("Unknown task, defaulting to positiveRückmeldung");
                    GPTPrompt.Add(positiveRückmeldung);
                    break;
            }
            if (humanChat)
            {
                GPTPrompt.Add(personaHannahIntro);
                GPTPrompt.Add(
                    new NLPAPI.GPTMessage(
                        NLPAPI.GPTMessageRoles.USER,
                        $"Stelle dich vor. Beginne dann mit der Übung. Ab jetzt sprichst du direkt mit dem Benutzer namens {username}. Sprich ihn freundlich mit seinem Namen an.Frage ihn, ob er die Übung machen möchte und welches Ziel diese hat."
                    )
                );
            }
            else
            {
                GPTPrompt.Add(personaMaschineIntro);
                GPTPrompt.Add(personaMaschineOutro);
            }

            // GPTPrompt.Add(new NLPAPI.GPTMessage(NLPAPI.GPTMessageRoles.USER,
            // $"Ab jetzt sprichst du direkt mit dem Benutzer namens {this.username}. Frage ihn, ob er die übung machen möchte und welches ziel diese hat."));
        }
        else
        {
            //TODO: Handle English Version
        }

        Start_NLPandPlayTTS(
            GPTPrompt,
            (response) =>
            {
                UnityMainThreadDispatcher
                    .Instance()
                    .Enqueue(() =>
                    {
                        StartCoroutine(CogitoExercise1_Strict(response));
                    });
            }
        );
    }

    private Coroutine Start_NLPandPlayTTS(
        List<NLPAPI.GPTMessage> input,
        Action<NLPAPI.GPTMessage> callback
    )
    {
        return StartCoroutine(NLPandPlayTTS(input, callback));
    }

    private IEnumerator NLPandPlayTTS(
        List<NLPAPI.GPTMessage> input,
        Action<NLPAPI.GPTMessage> callback
    )
    {
        string responseText = "";
        if (GPTPrompt[GPTPrompt.Count - 1].role == "assistant")
            responseText = GPTPrompt[GPTPrompt.Count - 1].content;

        List<string> toPlay = new List<string>();
        bool isDone = false;
        NLPAPI.GPTMessage result = null;

        NLPAPI.GetChat_NLPResponseStreamed(
            GPTPrompt.ToArray(),
            NLPAPI.GPT_Models.Chat_GPT_4o_mini,
            (response) =>
            {
                isDone = true;
                result = response;
            },
            (stream_response) =>
            {
                if (!stream_response.finished)
                {
                    // MobileSpecificSettings.Instance.InfoText.SetText("Ich bin fast fertig.");
                    responseText += " " + stream_response.delta;
                    toPlay.Add(responseText);
                    responseText = "";
                }
            }
        );

        int i = 0;
        // Wait until the response is finished or if there are strings to play
        while (!isDone || toPlay.Count > i)
        {
            Debug.Log($"IsDone: {isDone} | toPlay: {toPlay.Count - i}");
            if (toPlay.Count > i)
            {
                // Combine all strings to play and play them
                var toPlayString = "";
                for (; i < toPlay.Count; i++)
                {
                    toPlayString += toPlay[i];
                }
                Debug.Log($"Playing: {toPlayString}");
                // DataCollection.conversationTranscription = DataCollection.conversationTranscription +
                // "AI (" + DateTime.Now.ToString() + "): " + toPlayString + ".\n";
                yield return TTSAPI.TextToSpeechAndPlay(toPlayString, null, -1f);
            }
            else
            {
                yield return new WaitUntil(() => isDone || toPlay.Count > 0);
            }
        }
        Debug.Log($"IsDone: {isDone} | toPlay: {toPlay.Count}");

        // If the last statement was an Assistant type, remove the last statement and add the combined one
        if (GPTPrompt[GPTPrompt.Count - 1].role == "assistant")
        {
            var last = GPTPrompt[GPTPrompt.Count - 1];
            GPTPrompt.RemoveAt(GPTPrompt.Count - 1);
            result = new NLPAPI.GPTMessage(last.role, last.content + " " + result.content);
        }

        callback(result);
        Debug.Log($"IsDone: {isDone} | toPlay: {toPlay.Count}");
    }

    private IEnumerator CogitoExercise1_Strict(NLPAPI.GPTMessage response)
    {
        if (!timeIsUp)
        {
            // yield return API_Agent.Instance.TTSAPI.TextToSpeechAndPlay(response.content);
            //endConv = true;
            Debug.Log("cogito_strict");
            GPTPrompt.Add(response);
            Debug.Log("Response: " + response.content);

            string sst_res = "";

            // Recording startet
            if (!endConv)
            {
                yield return API_Agent.Instance.STTAPI.GetSpeechToText(
                    (intermRes) => { },
                    (finalRes) =>
                    {
                        sst_res = finalRes;
                    }
                );

                NLPAPI.GPTMessage userPrompt = new NLPAPI.GPTMessage(
                    NLPAPI.GPTMessageRoles.USER,
                    sst_res
                );
                GPTPrompt.Add(userPrompt);
            }

            if (endConv)
            {
                GPTPrompt.Add(
                    new NLPAPI.GPTMessage(
                        NLPAPI.GPTMessageRoles.SYSTEM,
                        $"Gib dem User Bescheid, dass unsere Zeit jetzt vorbei ist und wir jetzt die Aufgabe nun beenden müssen."
                    )
                );
            }

            Start_NLPandPlayTTS(
                GPTPrompt,
                (response) =>
                {
                    UnityMainThreadDispatcher
                        .Instance()
                        .Enqueue(() =>
                        {
                            StartCoroutine(CogitoExercise1_Strict(response));
                        });
                }
            );

            if (endConv)
            {
                MicRecorder.endOfConversation = true;
                MicRecorder.StopSTT();
                MicRecorder.StopAllCoroutines();
                setConditionDone();

                float waitTime = GetComponent<TTSAPI>().endWaitTime;
                Debug.Log("Wait for " + waitTime + "Seconds until Welcome Scene is loaded.");

                yield return new WaitForSeconds(waitTime + 1f);
                endConv = false;
                timeIsUp = false;

                //remove Exercise
                //SceneManagerScript.exercises.RemoveAt(exerciseIndex);

                //SceneManager.LoadScene(0);
                TTSAPI.stopTTS = true;
                //endApplication();
            }

            yield break;
        }
        else
        {
            MicRecorder.StopSTT();
            MicRecorder.StopAllCoroutines();
            Debug.Log("Time is up: " + timeIsUp);
            GPTPrompt.Add(response);

            if (german)
            {
                Debug.Log("aufhören");
                GPTPrompt.Add(
                    new NLPAPI.GPTMessage(
                        NLPAPI.GPTMessageRoles.ASSISTANT,
                        $"Wir müssen jetzt leider aufhören "
                    )
                );
            }
            else
            {
                GPTPrompt.Add(
                    new NLPAPI.GPTMessage(
                        NLPAPI.GPTMessageRoles.ASSISTANT,
                        $"Unfortunately we have run out of time now and have to stop. Remember"
                    )
                );
            }

            string goodbye = "";
            bool gptDone = false;

            NLPAPI.GetChat_NLPResponse(
                GPTPrompt.ToArray(),
                NLPAPI.GPT_Models.Chat_GPT_4o_mini,
                (response) =>
                {
                    goodbye = response.content;
                    gptDone = true;
                }
            );
            Debug.Log("goodbye: " + goodbye.ToString() + "gptDone: " + gptDone);

            Debug.Log("nlp response");

            // TODO: problem leight irgendwie am nlp response, der nicht kommt und somit nicht gptDone auf true setzt

            // yield return API_Agent.Instance.TTSAPI.TextToSpeechAndPlay(response.content);
            yield return new WaitUntil(() => gptDone);
            // yield return API_Agent.Instance.TTSAPI.TextToSpeechAndPlay(goodbye);
            Debug.Log(GPTPrompt[GPTPrompt.Count - 1].content + " " + goodbye);
            yield return API_Agent.Instance.TTSAPI.TextToSpeechAndPlay(
                GPTPrompt[GPTPrompt.Count - 1].content + " " + goodbye
            );

#if UNITY_EDITOR
            // stop unity editor
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // stop application
            Application.Quit();
#endif

            yield break;
        }
    }

    private void setConditionDone()
    {
        SceneManagerScript.startingScene = false;
        Debug.Log("setCondition: humanChat" + humanChat + "humanVisual" + humanVisual);
        if (humanChat && humanVisual)
        {
            SceneManagerScript.humanVisualHumanChatDone = true;
            Debug.Log("humanVisualHumanChatDone=" + SceneManagerScript.humanVisualHumanChatDone);
        }
        else if (humanChat && !humanVisual)
        {
            SceneManagerScript.machineVisualHumanChatDone = true;
            Debug.Log(
                "machineVisualHumanChatDone=" + SceneManagerScript.machineVisualHumanChatDone
            );
        }
        else if (!humanChat && humanVisual)
        {
            SceneManagerScript.humanVisualMachineChatDone = true;
            Debug.Log(
                "humanVisualMachineChatDone=" + SceneManagerScript.humanVisualMachineChatDone
            );
        }
        else if (!humanChat && !humanVisual)
        {
            SceneManagerScript.machineVisualMachineChatDone = true;
            Debug.Log(
                "machineVisualMachineChatDone=" + SceneManagerScript.machineVisualMachineChatDone
            );
        }
    }

    /// <summary>
    /// This Method will prompt GPT with the Statement to End the conversation after the time specified in
    /// convDurationMinutes ran out. Sets the timeIsUp bool to true and stops all currently runing Coroutines resulting in
    /// a interruption of the ongoing conversation.
    /// Calls the CogitoExercise Methos one last time in order to let GPT output its goodbye Prompt.
    /// </summary>
    /// <returns>IEnumerator to let this function be executed as a Coroutine</returns>
    public IEnumerator FinishConversation()
    {
        Debug.Log("Finishing Conversation");
        NLPAPI.GPTMessage goodbyeMessage = new NLPAPI.GPTMessage(
            NLPAPI.GPTMessageRoles.USER,
            "Wir müssen nun leider aufhören, unsere zeit ist fast vorbei und meine nächsten Patienten warten schon"
        );

        startTime = Time.time;
        //int secTilEnd = convDurationMinutes * 60;
        int secTilEnd = 10;
        yield return new WaitForSecondsRealtime(secTilEnd);
        Debug.Log("Waited for " + secTilEnd + " Seconds. Time is up!");
        timeIsUp = true;
        string goodbyeResponse = "";
        bool gptDone = false;
        // StopCoroutine(coroutineHandle);
        StopAllCoroutines();
        //StartCoroutine(CogitoExercise(goodbyeMessage));
        NLPAPI.GetChat_NLPResponse(
            GPTPrompt.ToArray(),
            NLPAPI.GPT_Models.Chat_GPT_4o_mini,
            (response) =>
            {
                goodbyeResponse = response.content;
                gptDone = true;
                Debug.Log("Goodbye response received: " + goodbyeResponse);
            }
        );

        // Wait until the response is received
        yield return new WaitUntil(() => gptDone);

        // Play the goodbye response using TTS
        yield return API_Agent.Instance.TTSAPI.TextToSpeechAndPlay(goodbyeResponse);

        // Stop the application or editor based on the environment
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnGUI()
    {
        return;
        // Show remaining time in top right corner
        if (!timeIsUp)
        {
            GUI.Label(
                new Rect(Screen.width - 100, 0, 100, 100),
                "Remaining Time: " + (Mathf.Abs(convDurationMinutes * 60 - (Time.time - startTime)))
            );
        }
    }

    void endApplication()
    {
#if UNITY_EDITOR
        // stop unity editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // stop application
        Application.Quit();
#endif
    }
}
