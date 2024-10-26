using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonsMultiple : MonoBehaviour
{
    public Button[] buttons; // ปุ่มทั้งหมด
    public Texture2D[] textures; // Texture ของคอร์เซอร์สำหรับแต่ละปุ่ม
    private Color32 activeColor = new Color32(152, 251, 152, 255); // สีเขียวอ่อน
    private Color32 inactiveColor = new Color32(255, 255, 255, 255); // สีขาว

    public int ToolsNumber { get; private set; } // เก็บค่า ToolsNumber

    void Start()
    {
        // ตรวจสอบให้แน่ใจว่า buttons และ textures มีขนาดเท่ากัน
        if (buttons.Length != textures.Length)
        {
            Debug.LogError("จำนวนปุ่มและ textures ต้องเท่ากัน!");
            return;
        }

        // กำหนดให้ปุ่มทั้งหมด inactive เริ่มต้น
        foreach (Button button in buttons)
        {
            SetButtonInactive(button);
        }

        // เพิ่มฟังก์ชันให้ปุ่มทั้งหมดเมื่อกด
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // เก็บค่า index ไว้ในตัวแปรเพราะ closure
            buttons[i].onClick.AddListener(() => ActivateButton(index));
        }

        ActivateButton(0); // ทำให้ปุ่มแรกถูกเลือกเมื่อเริ่มโปรแกรม
    }

    // ฟังก์ชันสำหรับการสลับ active ของปุ่ม และเปลี่ยนคอร์เซอร์
    void ActivateButton(int activeIndex)
    {
        // ทำให้ปุ่มที่กด active
        SetButtonActive(buttons[activeIndex]);

        // เปลี่ยนคอร์เซอร์ให้ตรงกับปุ่มที่กด
        Cursor.SetCursor(textures[activeIndex], Vector2.zero, CursorMode.Auto);

        // ทำให้ปุ่มอื่นๆ inactive
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i != activeIndex)
            {
                SetButtonInactive(buttons[i]);
            }
        }

        // เก็บค่าปุ่มที่ถูกเลือกไว้ใน ToolsNumber
        ToolsNumber = activeIndex;

        // ส่งค่าไปยัง CentralScript
        CentralScript.Instance.ReceiveButtonNumber(ToolsNumber); // ส่ง ToolsNumber
    }

    // ฟังก์ชันสำหรับการทำให้ปุ่ม active
    void SetButtonActive(Button button)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = activeColor;      // สีเมื่อไม่ได้ถูกกดหรือชี้
        colorBlock.highlightedColor = activeColor; // สีเมื่อเมาส์ชี้
        colorBlock.pressedColor = activeColor;     // สีเมื่อปุ่มถูกกด
        colorBlock.selectedColor = activeColor;    // สีเมื่อปุ่มถูกเลือก
        colorBlock.disabledColor = Color.gray;     // สีเมื่อปุ่มถูก disable
        button.colors = colorBlock;
        button.interactable = true; // เปิดการกดปุ่ม
    }

    // ฟังก์ชันสำหรับการทำให้ปุ่ม inactive
    void SetButtonInactive(Button button)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = inactiveColor;      // สีเมื่อไม่ได้ถูกกดหรือชี้
        colorBlock.highlightedColor = inactiveColor; // สีเมื่อเมาส์ชี้
        colorBlock.pressedColor = inactiveColor;     // สีเมื่อปุ่มถูกกด
        colorBlock.selectedColor = inactiveColor;    // สีเมื่อปุ่มถูกเลือก
        colorBlock.disabledColor = Color.gray;       // สีเมื่อปุ่มถูก disable
        button.colors = colorBlock;
        button.interactable = true; // เปิดการกดปุ่ม
    }
}
