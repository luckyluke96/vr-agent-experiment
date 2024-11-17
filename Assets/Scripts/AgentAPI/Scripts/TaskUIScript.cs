using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TaskUIScript : MonoBehaviour
{
    public Button quitButton;
    public Button restartButton;

    public TMP_Text logText;

    public DataCollection dataCollection;

    // Start is called before the first frame update
    void Start()
    {
        if (logText != null)
        {
            logText.text = SceneManagerScript.exerciseString;
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(quitButtonPressed);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(restartButtonPressed);
        }
    }

    // Update is called once per frame
    void Update() { }

    void quitButtonPressed()
    {
        dataCollection.LogGameData();
        SceneManager.LoadScene(0);
    }

    void restartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
