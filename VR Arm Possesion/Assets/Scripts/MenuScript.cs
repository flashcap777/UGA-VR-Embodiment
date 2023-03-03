using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] GlobalControls GlobalControls;

    [SerializeField] Mesh MeshOnModel;
    [SerializeField] Material MaterialOnModel;

    [SerializeField] Mesh[] Meshes;
    [SerializeField] Material[] Materials;
    TextMesh MenuText;
    Camera _Camera;

    bool isZeroed = true;

    public bool swapTest = false;

    const int DOWN = -1;
    const int UP = 1;

    string _MenuText;
    int MenuNo = 0;
    int selectionIndex = 0;
    float time = 0.1f;
    public bool inMenu = false;

    string[] currentOptions;

    // Choices for currentOptions
    string[] baseOptions = { "Body", "Skin", "Reset Game", "Close Menu" };
    string[] bodyOptions = { "Feminine Type", "Masculine Type", "Back" };
    string[] skinOptions = { "Variation 1", "Variation 2", "Variation 3", "Back" };
    
    int TARGET_FRAMERATE = 60;
    bool ERROR_FLAG = false;


    // Scrolls the menu. Down is -1, Up is 1.
    public void scrollMenu(int dir)
    {
        Debug.Log("Scrolled Menu in direction " + dir);
        Debug.Log(selectionIndex + " | " + currentOptions);

        if (!inMenu)
            return;

        switch (dir)
        {
            case UP:
                {
                    if (selectionIndex > 0)
                    {
                        selectionIndex--;
                    }
                    else
                        selectionIndex = currentOptions.Length - 1;
                }
                break;
            case DOWN:
                {
                    if (selectionIndex < currentOptions.Length - 1)
                    {
                        selectionIndex++;
                    }
                    else
                        selectionIndex = 0;
                }
                break;
        }
    }

    // Selects an option on the menu, based on the currently displayed options
    // and the current value of selectionIndex. The effect of each selectionIndex
    // value can be found in the instance variables.
    public void selectOption()
    {
        Debug.Log("In select!");

        if (!inMenu)
            return;

        // Base Options
        if (currentOptions == baseOptions)
        {
            switch (selectionIndex)
            {
                case 0:
                    {
                        selectionIndex = 0;
                        currentOptions = bodyOptions;
                        MenuNo = 1;
                    }
                    break;
                case 1:
                    {
                        selectionIndex = 0;
                        currentOptions = skinOptions;
                        MenuNo = 2;
                    }
                    break;
                case 2:
                    {
                        // Reset Game
                        GameObject.Find("CONTROL").GetComponent<GlobalControls>().UnbindMenuControls();
                        GameObject.Find("CONTROL").GetComponent<GlobalControls>().UnbindRealControls();
                        GameObject.Find("CONTROL").GetComponent<GlobalControls>().UnbindStartToggle();
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
                    }
                    break;
                case 3:
                    {
                        // Close Menu
                        GameObject.Find("CONTROL").GetComponent<ControlMenu>().ToggleMenu();
                    }
                    break;
            }
            return;
        }

        // Body Options
        if (currentOptions == bodyOptions)
        {
            switch (selectionIndex)
            {
                case 0:
                    {
                        MeshOnModel = Meshes[0];
                        if (MaterialOnModel == Materials[3])
                            MaterialOnModel = Materials[0];
                        if (MaterialOnModel == Materials[4])
                            MaterialOnModel = Materials[1];
                        if (MaterialOnModel == Materials[5])
                            MaterialOnModel = Materials[2];
                    }
                    break;
                case 1:
                    {
                        MeshOnModel = Meshes[1];
                        if (MaterialOnModel == Materials[0])
                            MaterialOnModel = Materials[3];
                        if (MaterialOnModel == Materials[1])
                            MaterialOnModel = Materials[4];
                        if (MaterialOnModel == Materials[2])
                            MaterialOnModel = Materials[5];
                    }
                    break;
                case 2:
                    {
                        currentOptions = baseOptions;
                        selectionIndex = 0;
                        MenuNo = 0;
                    }
                    break;
            }

            // Changes each of the two Meshes in the model to the selected Arm Mesh
            SkinnedMeshRenderer[] meshes = GameObject.Find("MODEL").GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer m in meshes)
            {
                m.sharedMesh = MeshOnModel;
            }
            foreach (SkinnedMeshRenderer m in meshes)
            {
                m.material = MaterialOnModel;
            }
        }

        // Skin Options
        if (currentOptions == skinOptions)
        {
            switch (selectionIndex)
            {
                case 0:
                    {
                        if (MeshOnModel == Meshes[0])
                            MaterialOnModel = Materials[0];
                        if (MeshOnModel == Meshes[1])
                            MaterialOnModel = Materials[3];
                    }
                    break;
                case 1:
                    {
                        if (MeshOnModel == Meshes[0])
                            MaterialOnModel = Materials[1];
                        if (MeshOnModel == Meshes[1])
                            MaterialOnModel = Materials[4];
                    }
                    break;
                case 2:
                    {
                        if (MeshOnModel == Meshes[0])
                            MaterialOnModel = Materials[2];
                        if (MeshOnModel == Meshes[1])
                            MaterialOnModel = Materials[5];
                    }
                    break;
                case 3:
                    {
                        currentOptions = baseOptions;
                        selectionIndex = 0;
                        MenuNo = 0;
                    }
                    break;
            }
            
            // Changes each of the two Meshes' Materials to the selected skin type
            SkinnedMeshRenderer[] meshes = GameObject.Find("MODEL").GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer m in meshes)
            {
                m.material = MaterialOnModel;
            }
        }
    }

    // Creates a text representation of the selectionIndex for options in a
    // vertical list.
    void debugMenu(string[] options)
    {
        if (ERROR_FLAG)
            return;

        MenuText.text = "";

        for (int i = 0; i < options.Length; i++)
        {
            if (i == selectionIndex)
                openAppend(">", MenuText);
            openAppend(options[i] + "\n", MenuText);
        }
    }

    // Appends a string to the end of a Textmesh component's text.
    void openAppend(string val, TextMesh original)
    {
        original.text += val;
    }
    void UpdateMenu()
    {
        switch (MenuNo)
        {
            case 0 :
                {
                    debugMenu(baseOptions);
                }
                break;
            case 1:
                {
                    debugMenu(bodyOptions);
                }
                break;
            case 2:
                {
                    debugMenu(skinOptions);
                }
                break;
            case 3:
                {
                    Debug.Log("Exit");
                }
                break;
        }
    }

    // Sends the Menu body to the center of the screen with a short animation.
    // The camera will smoothly follow the position and rotation of the headset.
    void JumpToCameraCenter()
    {
        float horzCenter = _Camera.pixelWidth / 2;
        float vertCenter = _Camera.pixelHeight / 2;

        Vector3 MenuLocation = _Camera.ScreenToWorldPoint(new Vector3(horzCenter, vertCenter));
        Vector3 camDir = _Camera.gameObject.transform.forward;

        MenuLocation += camDir.normalized * 2f;

        this.transform.parent.transform.position = Vector3.Lerp(this.transform.parent.position, MenuLocation, time);
        RotateToCameraCenter(camDir);
        if (Vector3.Distance(MenuLocation, this.transform.parent.transform.position) < 0.01f)
        {
            time = 0.01f;
        }
    }

    // Math for rotating the camera to match the headset's rotation.
    void RotateToCameraCenter(Vector3 Target)
    {
        float rotSpeed = 10f;
        Vector3 inverseRot = this.transform.parent.rotation.eulerAngles;
        inverseRot = new Vector3(inverseRot.x, inverseRot.y, inverseRot.z + 180);
        Vector3 direction = (Target + this.transform.parent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        if (Mathf.Abs(_Camera.gameObject.transform.localRotation.x) > 0.7071068f) // don't ask where 0.7071068f comes from, it just works
        {
            lookRotation = lookRotation * Quaternion.Euler(0, 0, 180);
        }
        this.transform.parent.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, TARGET_FRAMERATE * Time.deltaTime * rotSpeed);
    }

    void DebugSwapTest()
    {
        if (swapTest)
        {


            // Changes each of the two Meshes in the model to the selected Arm Mesh
            SkinnedMeshRenderer[] meshes = GameObject.Find("MODEL").GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer m in meshes)
            {
                m.sharedMesh = MeshOnModel;
            }

            // Changes each of the two Meshes' Materials to the selected skin type
            SkinnedMeshRenderer[] meshes2 = GameObject.Find("MODEL").GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer m in meshes2)
            {
                m.material = MaterialOnModel;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MenuText = GetComponentInChildren<TextMesh>();
        _Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        MenuNo = 0;
        currentOptions = baseOptions;
    }

    // Update is called once per frame
    void Update()
    {
        DebugSwapTest();

        if (!inMenu)
            return;

        UpdateMenu();
        JumpToCameraCenter();

        if (time < 1)
            time += 0.01f * TARGET_FRAMERATE * Time.deltaTime;
        if (time > 1)
            time = 1;
    }
}
