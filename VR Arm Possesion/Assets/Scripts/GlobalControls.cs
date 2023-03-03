using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class GlobalControls : MonoBehaviour
{
    [SerializeField] Transform Meter;
    [SerializeField] GameObject Hammer;
    [SerializeField] Transform ArmTargetL;
    [SerializeField] Transform ArmTargetR;

    [SerializeField] StartScreenUIUtil StartScreenUI;
    [SerializeField] ControlMenu ControlMenu;
    [SerializeField] MenuScript MenuScript;
    [SerializeField] ToggleMesh ToggleMesh;
    [SerializeField] EndScreenUtil EndScreenUI;

    public Controls _Controller;
    private MeterUtil MeterUtil;
    private Transform HammerTarget;

    float MeterPercent = 0;
    float CurrentRotation = 0;

    int TargetArm = -1;

    const int LEFT = -1;
    const int RIGHT = 1;

    Vector2 rightAxis = Vector2.zero;

    bool TriggerHeldR = false;
    bool TriggerHeldL = false;
    [SerializeField] bool IsZeroed = true;
    bool EndFlag = false;

    // Control Scripts Start Here
    private void Awake()
    {
        // Controls for Start Screen Only

        _Controller = new Controls();
        _Controller.Enable();

        _Controller.Player.RTrigger.performed += CallStartScreen;
        _Controller.Player.LTrigger.performed += CallStartScreen;

        _Controller.Player.RBumper.performed += CallStartScreen;
        _Controller.Player.LBumper.performed += CallStartScreen;

        _Controller.Player.StartButton.performed += CallStartScreen;
        _Controller.Player.SelectButton.performed += CallStartScreen;

        _Controller.Player.AButtonR.performed += CallStartScreen;
        _Controller.Player.AButtonL.performed += CallStartScreen;

        _Controller.Player.BButtonR.performed += CallStartScreen;
        _Controller.Player.BButtonL.performed += CallStartScreen;
    }

    void UnbindStartControls()
    {
        _Controller.Player.RTrigger.performed -= CallStartScreen;
        _Controller.Player.LTrigger.performed -= CallStartScreen;

        _Controller.Player.RBumper.performed -= CallStartScreen;
        _Controller.Player.LBumper.performed -= CallStartScreen;

        _Controller.Player.StartButton.performed -= CallStartScreen;
        _Controller.Player.SelectButton.performed -= CallStartScreen;

        _Controller.Player.AButtonR.performed -= CallStartScreen;
        _Controller.Player.AButtonL.performed -= CallStartScreen;

        _Controller.Player.BButtonR.performed -= CallStartScreen;
        _Controller.Player.BButtonL.performed -= CallStartScreen;

        _Controller.Player.StartButton.performed += CallToggleMenu;
    }

    public void BindRealControls()
    {
        UnbindStartControls();

        _Controller.Player.RTrigger.performed += _ => SetTrigger(RIGHT, true);
        _Controller.Player.LTrigger.performed += _ => SetTrigger(LEFT, true);

        _Controller.Player.RTrigger.canceled += _ => SetTrigger(RIGHT, false);
        _Controller.Player.LTrigger.canceled += _ => SetTrigger(LEFT, false);

        _Controller.Player.AButtonR.performed += CallSetMeterPercentR;
        _Controller.Player.AButtonL.performed += CallSetMeterPercentL;

        _Controller.Player.RStickClick.performed += _ => ToggleMesh.SwapActive(1);
        _Controller.Player.LStickClick.performed += _ => ToggleMesh.SwapActive(-1);
    }

    public void UnbindRealControls()
    {
        _Controller.Player.AButtonR.performed -= CallSetMeterPercentR;
        _Controller.Player.AButtonL.performed -= CallSetMeterPercentL;

        _Controller.Player.RTrigger.performed -= _ => SetTrigger(RIGHT, true);
        _Controller.Player.LTrigger.performed -= _ => SetTrigger(LEFT, true);

        _Controller.Player.RTrigger.canceled -= _ => SetTrigger(RIGHT, false);
        _Controller.Player.LTrigger.canceled -= _ => SetTrigger(LEFT, false);

        _Controller.Player.RStickClick.performed -= _ => ToggleMesh.SwapActive(1);
        _Controller.Player.LStickClick.performed -= _ => ToggleMesh.SwapActive(-1);

        Debug.Log("Real Controls Unbinded!");
    }

    public void BindMenuControls()
    {
        UnbindRealControls();

        _Controller.Player.AButtonR.performed += CallMenuSelect;
        _Controller.Player.AButtonL.performed += CallMenuSelect;

        Debug.Log("Menu Controls Binded!");
    }

    public void UnbindStartToggle()
    {
        _Controller.Player.StartButton.performed -= CallToggleMenu;
    }

    public void UnbindMenuControls()
    {
        BindRealControls();

        _Controller.Player.AButtonR.performed -= CallMenuSelect;
        _Controller.Player.AButtonL.performed -= CallMenuSelect;
    }

    public void DisplayEndScreen()
    {
        UnbindRealControls();

        EndScreenUI.EndScreen.SetActive(true);

        _Controller.Player.StartButton.performed += _ => EndScreenUI.SwitchMenuText();
        _Controller.Player.SelectButton.performed += _ => EndScreenUI.SwitchMenuText();
    }

    public void UnbindEndScreenControls()
    {
        _Controller.Player.StartButton.performed -= _ => EndScreenUI.SwitchMenuText();
        _Controller.Player.SelectButton.performed -= _ => EndScreenUI.SwitchMenuText();
        _Controller.Disable();
    }
    // Control Scripts End Here
    private void CallMenuSelect(InputAction.CallbackContext context)
    {
        MenuScript.selectOption();
    }

    private void CallStartScreen(InputAction.CallbackContext context)
    {
        StartScreenUI.SwitchMenuText();
    }

    private void CallToggleMenu(InputAction.CallbackContext context)
    {
        ControlMenu.ToggleMenu();
    }

    public float GetMeterPercent()
    {
        // Debug.Log("meter is: " + MeterPercent);
        return MeterPercent;
    }

    public Vector2 GetRightAxis()
    {
        return rightAxis;
    }

    // For some reason Oculus controllers don't have a "held" state, so this is here :^ )
    void SetTrigger(int trigger, bool held)
    {
        if (trigger == RIGHT)
        {
            TriggerHeldR = held;
        }
        if (trigger == LEFT)
        {
            TriggerHeldL = held;
        }
    }

    // Sets the meter percentage to the provided value (out of 100). Call location refers to the
    // Arm / Controller the SetMeterPercent() call comes from; Left is -1, Right is 1
    public void SetMeterPercent(float value, int callLocation)
    {
        if (EndFlag)
            return;

        MeterUtil.SetSlider(value);
        TargetArm = -callLocation;

        if (MeterPercent >= 100f || value == 100f)
        {
            CreateHammerAtTarget();
            EndFlag = true;
        }
    }
    private void CallSetMeterPercentR(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Called Meter!");
        _Controller.Player.AButtonR.performed -= CallSetMeterPercentR;
        _Controller.Player.AButtonL.performed -= CallSetMeterPercentL;

        SetMeterPercent(100, RIGHT);
    }
    private void CallSetMeterPercentL(InputAction.CallbackContext context)
    {
        Debug.LogWarning("Called Meter!");
        _Controller.Player.AButtonR.performed -= CallSetMeterPercentR;
        _Controller.Player.AButtonL.performed -= CallSetMeterPercentL;

        SetMeterPercent(100, LEFT);
    }

    // Similar to SetMeterPercent() but just increases it with respect to delta time
    public void IncreaseMeter(float value, int callLocation)
    {
        SetMeterPercent(MeterPercent += value * Time.deltaTime, callLocation);
    }

    // Creates a hammer at the target location. The hammer's origin should always be the target transform
    // that it will hit. The animation accommodates for this position already. 
    // The hammer's angle depends on the arm it will hit.
    void CreateHammerAtTarget()
    {
        switch (TargetArm)
        {
            case LEFT:
                {
                    HammerTarget = ArmTargetL;
                }
                break;
            case RIGHT:
                {
                    HammerTarget = ArmTargetR;
                }
                break;
        }

        Hammer.transform.position = HammerTarget.position;
        if (TargetArm == RIGHT && CurrentRotation == 120f)
            Hammer.transform.Rotate(0, CurrentRotation = 60f, 0);
        if (TargetArm == LEFT && CurrentRotation == 60f)
            Hammer.transform.Rotate(0, CurrentRotation = 120f, 0);

        Hammer.SetActive(true);
        UnbindStartToggle();
    }

    void Start()
    {
        MeterPercent = 0;
        MeterUtil = Meter.GetComponent<MeterUtil>();
    }

    void Update()
    {
        rightAxis = _Controller.Player.RStickAxis.ReadValue<Vector2>();

        if (rightAxis.y >= 0.1f && IsZeroed)
        {
            MenuScript.scrollMenu(1);
            IsZeroed = false;
        }

        if (rightAxis.y <= -0.1f && IsZeroed)
        {
            MenuScript.scrollMenu(-1);
            IsZeroed = false;
        }

        if (rightAxis.y < 0.1f && rightAxis.y > -0.1f)
            IsZeroed = true;
        else
            IsZeroed = false;

        if (TriggerHeldR)
            SetMeterPercent(MeterPercent += 0.5f, RIGHT);
        if (TriggerHeldL)
            SetMeterPercent(MeterPercent += 0.5f, LEFT);
    }

}
