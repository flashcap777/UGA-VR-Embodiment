using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenUIUtil : MonoBehaviour
{
    Text UIText;
    GameObject CONTROL;
    bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {
        UIText = GetComponentInChildren<Text>();
        
        // Disable all control components, make sure this gets run last in Script Execution Order
        DisableControls();
        // Debug.Log(SceneManager.sceneCount);
    }

    void DisableControls()
    {
        CONTROL = GameObject.Find("CONTROL");
        CONTROL.GetComponent<ToggleMesh>().enabled = false;
        // CONTROL.GetComponent<ControlMenu>().enabled = false;
    }
    void EnableControls()
    {
        CONTROL.GetComponent<ToggleMesh>().enabled = true;
        // CONTROL.GetComponent<ControlMenu>().enabled = true;
    }

    // Bind this to controls
    // Switches to the second slide of text, and then on
    // the next press, starts the game.
    public void SwitchMenuText()
    {
        if (!isReady)
        {
            UIText.text = "To access the main menu, press the Start button on your right controller. \n" +
            "To enable Multiplayer mode, click down on the analog stick of the controller you’d like to share. \n" +
            "<<Press any button to continue>>\n" +
            "[2 / 2]";
            isReady = true;
            return;
        }
        else
        {
            // Re-enable all components here
            EnableControls();
            CONTROL.GetComponent<GlobalControls>().BindRealControls();
            gameObject.SetActive(false);
        }
    }
}
