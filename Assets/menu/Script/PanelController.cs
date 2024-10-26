using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    // อ้างอิง Panel ที่จะเปิดหรือปิด
    public GameObject settingsPanel;

    // อ้างอิง Button สำหรับ toggle (เปิด/ปิด) Panel
    public Button openButton;
    public Button closeButton;

    void Start()
    {
        // ซ่อน Panel ตอนเริ่มต้น
        settingsPanel.SetActive(false);

        // ตั้งค่าให้ปุ่มฟันเฟือง toggle Panel
        openButton.onClick.AddListener(ToggleSettingsPanel);

        // ตั้งค่าให้ปุ่มกากบาทปิด Panel
        closeButton.onClick.AddListener(CloseSettingsPanel);
    }

    // ฟังก์ชัน toggle Panel (เปิด/ปิดตามสถานะปัจจุบัน)
    void ToggleSettingsPanel()
    {
        // สลับสถานะเปิด/ปิดของ Panel
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    // ฟังก์ชันปิด Panel
    void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }
}
