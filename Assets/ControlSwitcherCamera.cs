using UnityEngine;
using System.Collections;
using Gaskellgames.CameraController; // ใช้สำหรับ CameraSwitcher
using System.Collections.Generic;

public class ControlSwitcherCamera : MonoBehaviour
{
    [SerializeField]
    private CameraSwitcher cameraSwitcher; // ตั้งค่าผ่าน Inspector

    private int currentStep;
    private int NewCurrentStep = 96;
    int stack1;
int stack2;
int stack3;
    private bool active = true;

    private void Update()
    {
            
        if (CentralScript.Instance != null)
        {
            currentStep = CentralScript.Instance.GetCurrentStep();
            stack1 = CentralScript.Instance.GetStack1();
            stack2 = CentralScript.Instance.GetStack2();
            stack3 = CentralScript.Instance.GetStack3();
            Debug.Log("currentStep:"+currentStep);
        }
        else
        {
            Debug.LogError("CentralScript Instance is not found");
            return;
        }

        // ตรวจสอบการกดปุ่ม C
        if (Input.GetKeyDown(KeyCode.C))
        {
            active = !active;

            CentralScript.Instance.SetActiveState(active);
            if(currentStep >= 100)
            {
                StartCoroutine(SwitchCameraBasedOnStep(currentStep-96, active));
            }
            else if(stack1 < 36 && stack2 == 0 && stack3 == 0)
            {
                StartCoroutine(SwitchCameraBasedOnStep(1, active));
            }
            else if(stack2 < 19 && stack1 >= 36 && stack3 == 0)
            {
                StartCoroutine(SwitchCameraBasedOnStep(2, active));
            }
            else if(stack3 < 23 && stack1 >= 36 && stack2 >= 19)
            {
                StartCoroutine(SwitchCameraBasedOnStep(3, active));
            }
        }
    }

    private IEnumerator SwitchCameraBasedOnStep(int step, bool isActive)
    {
        int cameraIndex = GetCameraIndex(step, isActive);

        if (cameraSwitcher != null)
        {
            // รอ 0.5 วินาที ก่อนสลับกล้อง
            yield return new WaitForSeconds(0.1f);
            cameraSwitcher.SwitchToCamera(cameraIndex);
        }
        else
        {
            Debug.LogError("CameraSwitcher is not assigned");
        }
    }

private int GetCameraIndex(int step, bool isActive)
{
    // ถ้า active ให้คืนค่าเป็น 0 (Firstperson), ถ้าไม่ active ให้คืนค่าเป็นกล้องตาม step
    return isActive ? 0 : step;
}
}
