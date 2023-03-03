using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterCollisionHandler : MonoBehaviour
{
    bool isTouch = false;
    float MeterRate = 8f;
    GlobalControls Global;
    ToggleMesh ToggleMesh;
    [SerializeField] int CallLocation;

    void Start()
    {
        Global = GameObject.Find("CONTROL").GetComponent<GlobalControls>();
        ToggleMesh = GameObject.Find("CONTROL").GetComponent<ToggleMesh>();
    }

    void Update()
    {
        if (!ToggleMesh.GetOnControlL() && !ToggleMesh.GetOnControlR())
            return;
        if (isTouch)
        {
            Global.IncreaseMeter(MeterRate, CallLocation);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (this.gameObject.tag == "HAND_LEFT")
        {
            if (collision.gameObject.tag != "COL_RIGHT")
                return;
            isTouch = true;
            CallLocation = -1;
        }
        if (this.gameObject.tag == "HAND_RIGHT")
        {
            if (collision.gameObject.tag != "COL_LEFT")
                return;
            isTouch = true;
            CallLocation = 1;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (this.gameObject.tag == "HAND_LEFT")
        {
            if (collision.gameObject.tag != "COL_RIGHT")
                return;
            isTouch = false;
        }
        if (this.gameObject.tag == "HAND_RIGHT")
        {
            if (collision.gameObject.tag != "COL_LEFT")
                return;
            isTouch = false;
        }

    }
}
