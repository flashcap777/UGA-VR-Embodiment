using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenu : MonoBehaviour
{
    [SerializeField] GlobalControls GlobalControls;
    [SerializeField] GameObject Menu;
    [SerializeField] MenuScript MenuScript;

    public void ToggleMenu()
    {
        switch (MenuScript.inMenu)
        {
            case true:
                {
                    Debug.Log("Menu set to false");
                    GlobalControls.UnbindMenuControls();
                    Menu.GetComponent<MeshRenderer>().enabled = false;
                    MeshRenderer[] m = Menu.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer _m in m)
                    {
                        _m.enabled = false;
                    }
                    MenuScript.inMenu = false;
                }
                break;
            case false:
                {
                    Debug.Log("Menu set to true");
                    GlobalControls.BindMenuControls();
                    Menu.GetComponent<MeshRenderer>().enabled = true;
                    MeshRenderer[] m = Menu.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer _m in m)
                    {
                        _m.enabled = true;
                    }
                    MenuScript.inMenu = true;
                }
                break;
        }
    }
}
