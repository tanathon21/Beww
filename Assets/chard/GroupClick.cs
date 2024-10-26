using UnityEngine;
using System.Collections.Generic;


public class GroupClick : MonoBehaviour
{
    public int groupNumber; // กำหนดตัวเลขกลุ่ม 1, 2 หรือ 3
    public GameObject[] groupObjects; // ออปเจคในกลุ่มนี้
    public float outlineWidth = 6f; // ขนาดของ Outline Width ที่จะกำหนด

    void OnMouseDown()
    {
        // ส่งหมายเลขกลุ่มไปยังสคริปต์ศูนย์กลาง
        CentralScript.Instance.ReceiveGroupNumber(groupNumber, groupObjects);
    }

    private void OnMouseEnter()
    {
        // เปิดการแสดง Outline ให้กับออปเจคทุกตัวในกลุ่ม
        UpdateOutlineColor();
    }

    private void OnMouseExit()
    {
        // ปิดการแสดง Outline ให้กับออปเจคทุกตัวในกลุ่ม
        foreach (GameObject obj in groupObjects)
        {
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

    private void UpdateOutlineColor()
    {
        int currentStep = CentralScript.Instance.GetCurrentStep();
        int stack1 = CentralScript.Instance.GetStack1();
        int stack2 = CentralScript.Instance.GetStack2();
        int stack3 = CentralScript.Instance.GetStack3();

        foreach (GameObject obj in groupObjects)
        {
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                HashSet<int> CONNECTER_MOTOR1 = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
                HashSet<int> CONNECTER_MOTOR2 = new HashSet<int> { 9, 10 };
                HashSet<int> CONNECTER_MOTOR3 = new HashSet<int> { 11, 12 };
                if (stack1 >= 36 && stack2 >= 19 && stack3 >= 23){
                    if (currentStep == groupNumber)
                    {
                        outline.OutlineColor = Color.green;
                    }
                    else
                    {
                        outline.OutlineColor = Color.red;
                    }
                }
                //step1
                else if(stack2 == 0 && stack3 == 0 && CONNECTER_MOTOR1.Contains(groupNumber))
                {
                    outline.OutlineColor = Color.green;
                }
                //step2
                else if(stack1 >= 36 && stack3 == 0 &&  CONNECTER_MOTOR2.Contains(groupNumber))
                {
                    outline.OutlineColor = Color.green;
                }
                //step3
                else if(stack1 >= 36 && stack2 >= 19 && CONNECTER_MOTOR3.Contains(groupNumber))
                {
                    outline.OutlineColor = Color.green;
                }
                else
                    {
                        outline.OutlineColor = Color.red;
                    }

                
                outline.OutlineWidth = outlineWidth; // ตั้งค่าขนาดของ Outline
                outline.enabled = true;
            }
        }
    }
}
