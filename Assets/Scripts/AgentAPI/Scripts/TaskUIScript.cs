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

    public DataCollection dataCollection;

    // Start is called before the first frame update
    void Start()
    {
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(quitButtonPressed);
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
