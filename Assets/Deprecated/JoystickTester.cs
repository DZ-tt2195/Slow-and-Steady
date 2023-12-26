using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class JoystickTester : MonoBehaviour
{
    [Header("Press SPACE to enable/disable input.")]
    [Space(50)]
    [Header("REMEMBER: Assign the Joystick you want to test.")]
    [SerializeField] Joystick joystickToTest; // assign by dragging joystick to test into field form inspector
    private TextMeshProUGUI debugText;

    bool disableInput = false;
    private string errorMessage;

    // Start is called before the first frame update
    void Start()
    {
        debugText = GetComponent<TextMeshProUGUI>();
        errorMessage = "Joystick is null on JoystickTester Script on Gameobject " + gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (joystickToTest != null)
        {            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                disableInput = !disableInput;
            }

            debugText.text = $"Angle: {Mathf.Round(joystickToTest.angle)}";
        }
        else
        {
            debugText.text = errorMessage;
            Debug.LogError(errorMessage);
        }
    }
}
