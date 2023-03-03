using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndScreenUtil : MonoBehaviour
{
    [SerializeField] public GameObject EndScreen;
    [SerializeField] GlobalControls GlobalControls;

    // Bind this to controls
    // Switches to the second slide of text, and then on
    // the next press, restarts the game.
    public void SwitchMenuText()
    {
        GlobalControls.UnbindEndScreenControls();
        // SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
