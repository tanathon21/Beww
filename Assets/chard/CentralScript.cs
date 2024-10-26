using UnityEngine;
using System.Collections;
using Gaskellgames.CameraController;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.Animations;

public class CentralScript : MonoBehaviour
{
    public static CentralScript Instance;
    private int currentStep = 1; //1
    private int stack1 = 0; //0
    private int stack2 = 0; //0
    private int stack3 = 0; //0
    private GameObject destroyObject;
    [SerializeField]
    private CameraSwitcher cameraSwitcher;
    int cameraIndex = 1;

    private bool isProcessing;
    private bool isActive = false;
    private int receivedButtonNumber = -1;
    private bool isErrorStepsActive = false; // ตัวแปรสถานะของ error_steps
    private bool isErrorToolsActive = false; // ตัวแปรสถานะของ error_tools

    // เพิ่มตัวแปร GameObject error_steps
    [SerializeField] public GameObject error_steps;
    [SerializeField] public GameObject error_tools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (error_steps != null)
        {
            error_steps.SetActive(false);
        }
        if (error_tools != null)
        {
            error_tools.SetActive(false);
        }

    }

    public void SetActiveState(bool activeState)
    {
        isActive = activeState;
        Debug.Log("CentralScript active state updated: " + isActive);
    }

    public bool GetActiveState()
    {
        return isActive;
    }

    public void ReceiveGroupNumber(int groupNumber, GameObject[] groupObjects)
    {
        if (!isProcessing)
        {
            HashSet<int> NOT_M10 = new HashSet<int> { 100, 102, 104, 106, 108, 109, 111, 116, 118 };
            HashSet<int> MOTOR = new HashSet<int> { 101, 103, 105, 107, 110, 112, 117 };
            HashSet<int> CounterBalance1 = new HashSet<int> { 113 };
            HashSet<int> CounterBalance2 = new HashSet<int> { 114 };
            HashSet<int> CounterBalance3 = new HashSet<int> { 115 };
            HashSet<int> AXIS2 = new HashSet<int> { 119 };
            HashSet<int> CONNECTER_MOTOR1 = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            HashSet<int> CONNECTER_MOTOR2 = new HashSet<int> { 9, 10 };
            HashSet<int> CONNECTER_MOTOR3 = new HashSet<int> { 11, 12 };
            int sumOfConnecterMotor1 = CONNECTER_MOTOR1.Sum();
            int sumOfConnecterMotor2 = CONNECTER_MOTOR2.Sum();
            int sumOfConnecterMotor3 = CONNECTER_MOTOR3.Sum();
            Debug.Log("currentstep:  " + currentStep);

            if (receivedButtonNumber == 0 && CONNECTER_MOTOR1.Contains(groupNumber) ||
                 CONNECTER_MOTOR2.Contains(groupNumber) ||
                 CONNECTER_MOTOR3.Contains(groupNumber))
            {
                if (stack1 < sumOfConnecterMotor1 && stack2 == 0 && stack3 == 0 && !CONNECTER_MOTOR2.Contains(groupNumber) && !CONNECTER_MOTOR3.Contains(groupNumber))
                {
                    isProcessing = true;
                    string destroyObjectName = "A" + groupNumber;
                    destroyObject = GameObject.Find(destroyObjectName);
                    if (destroyObject != null)
                    {
                        TriggerAnimationForGroup(groupObjects, "A1");
                        stack1 += groupNumber;
                        if (stack1 >= sumOfConnecterMotor1)
                        {
                            cameraIndex = 2;
                            currentStep = 2;
                        }

                        if (cameraIndex > 50)
                        {
                            cameraIndex = 0;
                        }

                        StartCoroutine(DestroyObjectWithDelay(destroyObject, 2.5f, cameraIndex));
                    }
                    else
                    {
                        Debug.LogError("Object " + destroyObjectName + " not found.");
                        isProcessing = false;
                    }

                }
                else if (stack1 >= sumOfConnecterMotor1 && stack3 == 0 && stack2 < sumOfConnecterMotor2 && !CONNECTER_MOTOR1.Contains(groupNumber) && !CONNECTER_MOTOR3.Contains(groupNumber))
                {
                    if (CONNECTER_MOTOR2.Contains(groupNumber))
                    {
                        isProcessing = true;
                        string destroyObjectName = "A" + groupNumber;
                        destroyObject = GameObject.Find(destroyObjectName);
                        if (destroyObject != null)
                        {
                            TriggerAnimationForGroup(groupObjects, "A1");
                            stack2 += groupNumber;
                            if (stack2 >= sumOfConnecterMotor2)
                            {
                                cameraIndex = 3;
                                currentStep = 3;
                            }

                            if (cameraIndex > 50)
                            {
                                cameraIndex = 0;
                            }

                            StartCoroutine(DestroyObjectWithDelay(destroyObject, 2.5f, cameraIndex));
                        }
                        else
                        {
                            Debug.LogError("Object " + destroyObjectName + " not found.");
                            isProcessing = false;
                        }
                    }
                }
                else if (stack1 >= sumOfConnecterMotor1 && stack2 >= sumOfConnecterMotor2 && stack3 < sumOfConnecterMotor3 && !CONNECTER_MOTOR1.Contains(groupNumber) && !CONNECTER_MOTOR2.Contains(groupNumber))
                {
                    if (CONNECTER_MOTOR3.Contains(groupNumber) && receivedButtonNumber == 0)
                    {
                        isProcessing = true;
                        string destroyObjectName = "A" + groupNumber;
                        destroyObject = GameObject.Find(destroyObjectName);
                        if (destroyObject != null)
                        {
                            TriggerAnimationForGroup(groupObjects, "A1");
                            stack3 += groupNumber;
                            if (stack3 >= sumOfConnecterMotor3)
                            {
                                cameraIndex = 4;
                                currentStep = 99;
                            }

                            if (cameraIndex > 50)
                            {
                                cameraIndex = 0;
                            }

                            StartCoroutine(DestroyObjectWithDelay(destroyObject, 2.5f, cameraIndex));
                        }
                        else
                        {
                            Debug.LogError("Object " + destroyObjectName + " not found.");
                            isProcessing = false;
                        }
                    }
                }else if (error_steps != null && !isErrorStepsActive)
                {
                    error_steps.SetActive(true);
                    isErrorStepsActive = true;
                    StartCoroutine(HideErrorStepsWithDelay(3f));
                }
            }
            else if (receivedButtonNumber != 0 && CONNECTER_MOTOR1.Contains(groupNumber) ||
                 CONNECTER_MOTOR2.Contains(groupNumber) ||
                 CONNECTER_MOTOR3.Contains(groupNumber))
            {
                // แสดง error_tools
                if (error_tools != null && !isErrorToolsActive)
                {
                    error_tools.SetActive(true);
                    isErrorToolsActive = true;
                    StartCoroutine(HideErrorToolsWithDelay(3f));
                }
            }
            else if (groupNumber == currentStep &&
            stack1 >= sumOfConnecterMotor1 &&
            stack2 >= sumOfConnecterMotor2 &&
            stack3 >= sumOfConnecterMotor3)
            {
                if (NOT_M10.Contains(groupNumber) && receivedButtonNumber == 1)
                {
                    isProcessing = true;
                    string destroyObjectName = "A" + groupNumber;
                    destroyObject = GameObject.Find(destroyObjectName);
                    string toolName = "A" + groupNumber + "_1";
                    GameObject[] toolObjects = GameObject.FindGameObjectsWithTag(toolName); // หาอ็อบเจ็กต์ทั้งหมดที่มี Tag ตรงกัน
                    float delayAnimationTrigger = 2.5f;
                    if (toolObjects.Length > 0)
                    {
                        foreach (GameObject toolObject in toolObjects)
                        {
                            Debug.Log("Found tool object: " + toolObject.name); // log เมื่อพบอ็อบเจ็กต์

                            // Trigger animation for each tool object
                            StartCoroutine(ToolAnimation(toolObject, groupObjects, groupNumber, delayAnimationTrigger));
                        }
                    }
                    else
                    {
                        Debug.LogError("No tool objects found with name: " + toolName);
                    }
                    if (destroyObject != null)
                    {
                        StartCoroutine(delayAnimation(groupObjects, delayAnimationTrigger));
                        int cameraIndex = (groupNumber - 95);
                        if (cameraIndex > 50) cameraIndex = 0;
                        StartCoroutine(DestroyObjectWithDelay(destroyObject, 5f + delayAnimationTrigger, cameraIndex));

                    }
                    else
                    {
                        Debug.LogError("Object " + destroyObjectName + " not found.");
                        isProcessing = false;
                    }
                }
                else if (MOTOR.Contains(groupNumber) && receivedButtonNumber == 0)
                {
                    isProcessing = true;
                    string destroyObjectName = "A" + groupNumber;
                    destroyObject = GameObject.Find(destroyObjectName);
                    if (destroyObject != null)
                    {
                        float delayAnimationTrigger = 0f;
                        StartCoroutine(delayAnimation(groupObjects, delayAnimationTrigger));
                        int cameraIndex = (groupNumber - 95);
                        if (cameraIndex > 50) cameraIndex = 0;
                        StartCoroutine(DestroyObjectWithDelay(destroyObject, 5f + delayAnimationTrigger, cameraIndex));
                    }
                    else
                    {
                        Debug.LogError("Object " + destroyObjectName + " not found.");
                        isProcessing = false;
                    }
                }
                else if (CounterBalance1.Contains(groupNumber) && receivedButtonNumber == 1)
                {
                    isProcessing = true;
                    string destroyObjectName = "A" + groupNumber;
                    destroyObject = GameObject.Find(destroyObjectName);
                    string toolName = "A" + groupNumber + "_1";
                    GameObject[] toolObjects = GameObject.FindGameObjectsWithTag(toolName); // หาอ็อบเจ็กต์ทั้งหมดที่มี Tag ตรงกัน
                    float delayAnimationTrigger = 3f;
                    if (toolObjects.Length > 0)
                    {
                        foreach (GameObject toolObject in toolObjects)
                        {
                            Debug.Log("Found tool object: " + toolObject.name); // log เมื่อพบอ็อบเจ็กต์

                            // Trigger animation for each tool object
                            StartCoroutine(ToolAnimation(toolObject, groupObjects, groupNumber, delayAnimationTrigger));
                        }
                    }
                    else
                    {
                        Debug.LogError("No tool objects found with name: " + toolName);
                    }
                    if (destroyObject != null)
                    {
                        StartCoroutine(delayAnimation(groupObjects, delayAnimationTrigger));
                        int cameraIndex = (groupNumber - 95);
                        if (cameraIndex > 50) cameraIndex = 0;
                        StartCoroutine(DestroyObjectWithDelay(destroyObject, 5f + delayAnimationTrigger, cameraIndex));
                    }
                    else
                    {
                        Debug.LogError("Object " + destroyObjectName + " not found.");
                        isProcessing = false;
                    }
                }
                else if (CounterBalance2.Contains(groupNumber) && receivedButtonNumber == 5)
                {
                    isProcessing = true;
                    string destroyObjectName = "A" + groupNumber;
                    destroyObject = GameObject.Find(destroyObjectName);
                    string toolName = "A" + groupNumber + "_1";
                    GameObject[] toolObjects = GameObject.FindGameObjectsWithTag(toolName); // หาอ็อบเจ็กต์ทั้งหมดที่มี Tag ตรงกัน
                    float delayAnimationTrigger = 13.5f;
                    if (toolObjects.Length > 0)
                    {
                        foreach (GameObject toolObject in toolObjects)
                        {
                            Debug.Log("Found tool object: " + toolObject.name); // log เมื่อพบอ็อบเจ็กต์

                            // Trigger animation for each tool object
                            StartCoroutine(ToolAnimation(toolObject, groupObjects, groupNumber, delayAnimationTrigger));
                        }
                    }
                    else
                    {
                        Debug.LogError("No tool objects found with name: " + toolName);
                    }
                    if (destroyObject != null)
                    {
                        StartCoroutine(delayAnimation(groupObjects, delayAnimationTrigger));
                        int cameraIndex = (groupNumber - 95);
                        if (cameraIndex > 50) cameraIndex = 0;
                        StartCoroutine(DestroyObjectWithDelay(destroyObject, 5f + delayAnimationTrigger, cameraIndex));
                    }
                    else
                    {
                        Debug.LogError("Object " + destroyObjectName + " not found.");
                        isProcessing = false;
                    }
                }
                else if (CounterBalance3.Contains(groupNumber) && receivedButtonNumber == 4)
                {
                    isProcessing = true;
                    string destroyObjectName = "A" + groupNumber;
                    destroyObject = GameObject.Find(destroyObjectName);
                    string toolName = "A" + groupNumber + "_1";
                    GameObject[] toolObjects = GameObject.FindGameObjectsWithTag(toolName); // หาอ็อบเจ็กต์ทั้งหมดที่มี Tag ตรงกัน
                    float delayAnimationTrigger = 3f;
                    if (toolObjects.Length > 0)
                    {
                        foreach (GameObject toolObject in toolObjects)
                        {
                            Debug.Log("Found tool object: " + toolObject.name); // log เมื่อพบอ็อบเจ็กต์

                            // Trigger animation for each tool object
                            StartCoroutine(ToolAnimation(toolObject, groupObjects, groupNumber, 3f));
                        }
                    }
                    else
                    {
                        Debug.LogError("No tool objects found with name: " + toolName);
                    }
                    if (destroyObject != null)
                    {
                        StartCoroutine(delayAnimation(groupObjects, delayAnimationTrigger));
                        int cameraIndex = (groupNumber - 95);
                        if (cameraIndex > 50) cameraIndex = 0;
                        StartCoroutine(DestroyObjectWithDelay(destroyObject, 5f + delayAnimationTrigger, cameraIndex));
                    }
                    else
                    {
                        Debug.LogError("Object " + destroyObjectName + " not found.");
                        isProcessing = false;
                    }
                }
                else if (AXIS2.Contains(groupNumber) && receivedButtonNumber == 6)
                {
                    isProcessing = true;
                    string destroyObjectName = "A" + groupNumber;
                    destroyObject = GameObject.Find(destroyObjectName);
                    string toolName = "A" + groupNumber + "_1";
                    GameObject[] toolObjects = GameObject.FindGameObjectsWithTag(toolName); // หาอ็อบเจ็กต์ทั้งหมดที่มี Tag ตรงกัน
                    float delayAnimationTrigger = 6.5f;
                    if (toolObjects.Length > 0)
                    {
                        foreach (GameObject toolObject in toolObjects)
                        {
                            Debug.Log("Found tool object: " + toolObject.name); // log เมื่อพบอ็อบเจ็กต์

                            // Trigger animation for each tool object
                            StartCoroutine(ToolAnimation(toolObject, groupObjects, groupNumber, 6.5f));
                        }
                    }
                    else
                    {
                        Debug.LogError("No tool objects found with name: " + toolName);
                    }
                    if (destroyObject != null)
                    {
                        StartCoroutine(delayAnimation(groupObjects, delayAnimationTrigger));
                        int cameraIndex = (groupNumber - 95);
                        if (cameraIndex > 50) cameraIndex = 0;
                        StartCoroutine(DestroyObjectWithDelay(destroyObject, 7f + delayAnimationTrigger, cameraIndex));
                    }
                    else
                    {
                        Debug.LogError("Object " + destroyObjectName + " not found.");
                        isProcessing = false;
                    }
                }
            }
            else if (!NOT_M10.Contains(groupNumber) && receivedButtonNumber == 1)
            {
                // แสดง error_steps
                if (error_steps != null && !isErrorStepsActive)
                {
                    error_steps.SetActive(true);
                    isErrorStepsActive = true;
                    StartCoroutine(HideErrorStepsWithDelay(3f));
                }
            }
            else if (!MOTOR.Contains(groupNumber) && receivedButtonNumber == 0)
            {
                // แสดง error_tools
                if (error_tools != null && !isErrorToolsActive)
                {
                    error_tools.SetActive(true);
                    isErrorToolsActive = true;
                    StartCoroutine(HideErrorToolsWithDelay(3f));
                }
            }

        }
        else if (isProcessing)
        {
            Debug.Log("Action is already in progress.");
        }
        else
        {
            // แสดง error_steps และซ่อน error_tools หากต้องการ
            if (error_steps != null && error_tools != null && !isErrorStepsActive)
            {
                error_steps.SetActive(true);
                error_tools.SetActive(false);
                isErrorStepsActive = true;
                StartCoroutine(HideErrorStepsWithDelay(3f));
            }
        }
    }

    // ปรับปรุงฟังก์ชันการซ่อน error_tools

    private IEnumerator ToolAnimation(GameObject toolObject, GameObject[] groupObjects, int groupNumber, float delay)
    {
        Animator animator = toolObject.GetComponent<Animator>();
        Debug.Log("Starting animation for: " + toolObject.name); // log เมื่อเริ่มอนิเมชัน
        animator.SetTrigger("A1");

        // รอจนกว่าอนิเมชันจะทำงานเสร็จ (ปรับเวลาให้เหมาะสม)
        yield return new WaitForSeconds(delay);
        Debug.Log("Animation finished for: " + toolObject.name); // log เมื่ออนิเมชันเสร็จ

        // ทำลาย tool object หลังจากอนิเมชันเสร็จสิ้น
        Destroy(toolObject);
        if (animator != null)
        {
        }
        else
        {
            Debug.LogError("Animator not found on tool object: " + toolObject.name);
        }
    }


    private IEnumerator HideErrorToolsWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (error_tools != null)
        {
            error_tools.SetActive(false);
            isErrorToolsActive = false; // รีเซ็ตสถานะ
        }
    }

    // ปรับปรุงฟังก์ชันการซ่อน error_steps
    private IEnumerator HideErrorStepsWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (error_steps != null)
        {
            error_steps.SetActive(false);
            isErrorStepsActive = false; // รีเซ็ตสถานะ
        }
    }

    public void ReceiveButtonNumber(int buttonNumber)
    {
        receivedButtonNumber = buttonNumber;
        Debug.Log("Received button number: " + receivedButtonNumber);
    }

    private void TriggerAnimationForGroup(GameObject[] groupObjects, string triggerName)
    {
        foreach (GameObject obj in groupObjects)
        {
            Animator animator = obj.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(triggerName);
            }
        }
    }
    private IEnumerator delayAnimation(GameObject[] groupObjects, float delay)
    {
        yield return new WaitForSeconds(delay);
        TriggerAnimationForGroup(groupObjects, "A1");
    }
    private IEnumerator DestroyObjectWithDelay(GameObject obj, float delay, int cameraIndex)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
        {
            Destroy(obj);
            Debug.Log(obj.name + " has been destroyed after " + delay + " seconds.");
            /*
                        if (obj.name == "A9")
                        {
                            GameObject additionalObject = GameObject.Find("A9_1");
                            if (additionalObject != null)
                            {
                                Destroy(additionalObject);
                                Debug.Log(additionalObject.name + " has been destroyed as well.");
                            }
                            else
                            {
                                Debug.LogError("A9 not found.");
                            }
                        }*/
        }

        if (cameraSwitcher != null)
        {
            if (isActive)
            {
                cameraSwitcher.SwitchToCamera(0);
                Debug.Log("Switching to cameraIndex: " + cameraIndex);
            }
            else
            {
                cameraSwitcher.SwitchToCamera(cameraIndex);
                Debug.Log("Switching to cameraIndex: " + cameraIndex);
            }
        }
        else
        {
            Debug.LogError("CameraSwitcher is not assigned");
        }
        currentStep++;
        isProcessing = false;
    }

    public int GetCurrentStep()
    {
        return currentStep;
    }
    public int GetStack1()
    {
        return stack1;
    }
    public int GetStack2()
    {
        return stack2;
    }
    public int GetStack3()
    {
        return stack3;
    }
    public int GetSumStack1()
    {
        return stack1;
    }
}
