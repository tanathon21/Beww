using UnityEngine;

public class WindowSize : MonoBehaviour
{
    void Start()
    {
        // ตั้งค่าขนาดหน้าต่าง
        Screen.SetResolution(1240, 768, false); // false = ไม่เต็มหน้าจอ
    }
}
