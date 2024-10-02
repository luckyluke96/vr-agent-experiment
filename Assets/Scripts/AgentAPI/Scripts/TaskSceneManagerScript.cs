using System.Collections;
using System.Collections.Generic;
using GLTFast.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskSceneManagerScript : MonoBehaviour
{
    public GameObject robot;
    public GameObject hannah;

    // Start is called before the first frame update
    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "HumanVisualHumanChat":
                SceneManagerScript.humanVisual = true;
                SceneManagerScript.humanChat = true;
                break;
            case "MachineVisualMachineChat":
                SceneManagerScript.humanVisual = false;
                SceneManagerScript.humanChat = false;
                break;
            case "HumanVisualMachineChat":
                SceneManagerScript.humanVisual = true;
                SceneManagerScript.humanChat = false;
                break;
            case "MachineVisualHumanChat":
                SceneManagerScript.humanVisual = false;
                SceneManagerScript.humanChat = true;
                break;
        }

        // position agents
        if (SceneManagerScript.humanVisual)
        {
            hannah.transform.position = new Vector3(0, 0.1f, 0.5f);
            robot.transform.position = new Vector3(1000, 1000, 1000);
        }
        else
        {
            robot.transform.position = new Vector3(0, 1.5f, 0.3f);
            hannah.transform.position = new Vector3(1000, 1000, 1000);
        }
    }

    // Update is called once per frame
    void Update() { }
}
