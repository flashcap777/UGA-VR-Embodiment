using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ToggleMesh : MonoBehaviour
{
    bool onControlR = false;
    bool onControlL = false;

    [SerializeField] GameObject armMeshR;
    [SerializeField] GameObject armMeshL;

    [SerializeField] GameObject controllerMeshR;
    [SerializeField] GameObject controllerMeshL;

    public bool GetOnControlR()
    {
        return onControlR;
    }

    public bool GetOnControlL()
    {
        return onControlL;
    }

    void Start()
    {
        onControlR = false;
        onControlL = false;

        armMeshR.GetComponent<SkinnedMeshRenderer>().enabled = true;
        controllerMeshR.GetComponent<SkinnedMeshRenderer>().enabled = false;

        armMeshL.GetComponent<SkinnedMeshRenderer>().enabled = true;
        controllerMeshL.GetComponent<SkinnedMeshRenderer>().enabled = false;
    }

    // Swaps the given controller with the matching arm for Multiplayer mode,
    // and vice versa. Left is -1, Right is 1.
    public void SwapActive(int type)
    {
        switch (type)
        {
            case 1:
                {
                    if (onControlR)
                    {
                        onControlR = false;
                        armMeshR.GetComponent<SkinnedMeshRenderer>().enabled = true;
                        controllerMeshR.GetComponent<SkinnedMeshRenderer>().enabled = false;
                    }
                    else
                    {
                        onControlR = true;
                        armMeshR.GetComponent<SkinnedMeshRenderer>().enabled = false;
                        controllerMeshR.GetComponent<SkinnedMeshRenderer>().enabled = true;
                    }
                }
                break;
            case -1:
                {
                    if (onControlL)
                    {
                        onControlL = false;
                        armMeshL.GetComponent<SkinnedMeshRenderer>().enabled = true;
                        controllerMeshL.GetComponent<SkinnedMeshRenderer>().enabled = false;
                    }
                    else
                    {
                        onControlL = true;
                        armMeshL.GetComponent<SkinnedMeshRenderer>().enabled = false;
                        controllerMeshL.GetComponent<SkinnedMeshRenderer>().enabled = true;
                    }
                }
                break;
        }
    }
}
