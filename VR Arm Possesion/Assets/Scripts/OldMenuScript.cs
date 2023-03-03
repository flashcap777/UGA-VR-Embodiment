using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMenuScript : MonoBehaviour
{
    [SerializeField] GlobalControls GlobalControls;
    TextMesh MenuText;
    Camera _Camera;
    GameObject[] ItemList;

    bool isZeroed = true;

    const int DOWN = -1;
    const int UP = 1;

    string _MenuText;
    int MenuNo = 0;
    int selectionIndex = 0;
    float time = 0.1f;
    public bool inMenu = false;

    string[] currentOptions;

    // Choices for currentOptions
    string[] baseOptions = { "Item", "Meter", "Reset Game", "Close Menu" };
    string[] itemOptions = { "Clear", "Back" };
    string[] meterOptions = { "Toggle Visibility", "Reset Meter", "Back" };

    int TARGET_FRAMERATE = 60;
    bool ERROR_FLAG = false;


    // Scrolls the menu. Down is -1, Up is 1.
    public void scrollMenu(int dir)
    {
        switch (dir)
        {
            case UP:
                {
                    if (selectionIndex > 0)
                    {
                        selectionIndex--;
                    }
                    else
                        selectionIndex = currentOptions.Length;
                }
                break;
            case DOWN:
                {
                    if (selectionIndex < currentOptions.Length)
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
        if (gameObject.activeSelf == false)
            return;

        // Item Options
        if (currentOptions == itemOptions)
        {
            if (selectionIndex < ItemList.Length)
            {
                Instantiate(ItemList[selectionIndex], new Vector3(0, 1, 0), Quaternion.identity);
                return;
            }
            else if (selectionIndex <= itemOptions.Length)
            {
                if (selectionIndex == itemOptions.Length - 1)
                {
                    Debug.Log("Cleared all Items!");
                    return;
                }

                if (selectionIndex == itemOptions.Length)
                {
                    MenuNo = 0;
                    selectionIndex = 0;
                    currentOptions = baseOptions;
                    return;
                }
            }
        }

        // Meter Options
        if (currentOptions == meterOptions)
        {
            // meter stuff here

            MenuNo = 0;
            selectionIndex = 0;
            currentOptions = baseOptions;
            return;
        }

        // Base Options
        switch (selectionIndex)
        {
            case 0:
                {
                    if (currentOptions == baseOptions)
                    {
                        selectionIndex = 0;
                        currentOptions = itemOptions;
                        MenuNo = 1;
                    }
                }
                break;
            case 1:
                {
                    if (currentOptions == baseOptions)
                    {
                        selectionIndex = 0;
                        currentOptions = meterOptions;
                        MenuNo = 2;
                    }
                }
                break;
            case 2:
                {
                    if (currentOptions == baseOptions)
                    {
                        selectionIndex = 0;
                        currentOptions = baseOptions;
                        MenuNo = 0;
                        // Insert method for Restart game here
                    }
                }
                break;
            case 3:
                {
                    if (currentOptions == baseOptions)
                    {
                        // close menu
                        selectionIndex = 0;
                        gameObject.SetActive(false);
                    }
                }
                break;
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
            case 0:
                {
                    debugMenu(baseOptions);
                }
                break;
            case 1:
                {
                    debugMenu(itemOptions);
                }
                break;
            case 2:
                {
                    debugMenu(meterOptions);
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
        // Debug.Log(Mathf.Abs(_Camera.gameObject.transform.localRotation.x));
        if (Mathf.Abs(_Camera.gameObject.transform.localRotation.x) > 0.7071068f) // don't ask where 0.7071068f comes from, it just works
        {
            lookRotation = lookRotation * Quaternion.Euler(0, 0, 180);
        }
        this.transform.parent.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, TARGET_FRAMERATE * Time.deltaTime * rotSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        ItemList = GameObject.Find("ITEMS").GetComponent<ItemUtil>().GetAllItems();
        MenuText = GetComponent<TextMesh>();
        _Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        MenuNo = 0;

        itemOptions = new string[ItemList.Length + 2];
        for (int i = 0; i < ItemList.Length; i++)
        {
            itemOptions[i] = ItemList[i].ToString();
        }
        itemOptions[itemOptions.Length - 2] = "Clear";
        itemOptions[itemOptions.Length - 1] = "Back";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMenu();
        JumpToCameraCenter();

        if (time < 1)
            time += 0.01f * TARGET_FRAMERATE * Time.deltaTime;
        if (time > 1)
            time = 1;
    }
}
