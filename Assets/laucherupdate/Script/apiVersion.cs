using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this line for the Text type
using UnityEngine.Networking; // Add this line for UnityWebRequest

public class apiVersion : MonoBehaviour
{
    [SerializeField] Text version; // Add this line to reference the version input field

        [SerializeField] Button checkUpdateButton;
    [SerializeField] Button updateNowButton;
    [SerializeField] Button notNowButton;
    [SerializeField] Text alertText;
    [SerializeField] GameObject updatePopup;  // Popup window for update
    private string downloadURL = "https://example.com/download"; // URL สำหรับดาวน์โหลดเวอร์ชันใหม่


    public void OnLoginButtonClicked()
    {
        StartCoroutine(CheckUpdateCoroutine());
    }

    IEnumerator CheckUpdateCoroutine()
    {
        Dictionary<string, string> formData = new();
        formData["version"] = version.text;

        using UnityWebRequest www = UnityWebRequest.Post("https://manage.np-robotics.com/api/checkversion/index.php", formData);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            string response = www.downloadHandler.text;
            Debug.Log("Response: " + response);

            if (response.StartsWith("Version:"))
            {
                string versionFromServer = response.Substring(8).Trim(); // Extract the version after "Version: "
                if (versionFromServer == version.text)
                {

                    //เวอร์ชันตรงกันนนนนนนนนนนนนนนนนนนนนนนนนนนนนนน

                    Debug.Log("เวอร์ชันตรงกัน"); // Version matches
                }
                else
                {
                    Debug.Log("Error");
                }
            }
            else if(response.StartsWith("SERVER:"))
            {

                //เวอร์ชันไม่ตรงกันนนนนนนนนนนนนนนนนนนนนนนนนนนนนนน
                Debug.Log("เวอร์ชันไม่ตรงกัน");
            }
            else{
                Debug.LogError("ERROR");
            }
        }
    }
        void ShowUpdatePrompt()
    {
        alertText.text = "";  // Clear any existing text
        updatePopup.SetActive(true);  // Show popup
        updateNowButton.gameObject.SetActive(true);
        notNowButton.gameObject.SetActive(true);
    }

    // ฟังก์ชันเมื่อกดปุ่ม Update now
    void UpdateNow()
    {
        Debug.Log("Updating to the latest version...");
        Application.OpenURL(downloadURL); // เปิดลิงก์ดาวน์โหลดโปรแกรมเวอร์ชันใหม่
        updatePopup.SetActive(false); // Hide the popup after action
    }

    // ฟังก์ชันเมื่อกดปุ่ม Not now
    void NotNow()
    {
        Debug.Log("Skipping the update.");
        updatePopup.SetActive(false);  // Hide the popup
    }
    
}
