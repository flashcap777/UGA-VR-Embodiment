using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerExitUtil : MonoBehaviour
{
    // Once the hammer finishes the Swing animation and goes into the
    // exit state, disable it and reload the Swing animation.
    void GetExitState()
    {
        if (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Armature|Exit"))
        {
            GetComponentInChildren<Animator>().Play("Armature|Swing");
            this.gameObject.SetActive(false);
        } 
    }

    private void OnDisable()
    {
        GameObject.Find("CONTROL").GetComponent<GlobalControls>().DisplayEndScreen();
    }

    void Update()
    {
        GetExitState();
    }
}
