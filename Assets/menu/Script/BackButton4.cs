using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButton4 : MonoBehaviour
{
    public Button backButton; // Button object for "Back"

    // Start is called before the first frame update
    void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackPressed);
        }
        else
        {
            Debug.LogError("Back Button not assigned in the Inspector.");
        }
    }

    void OnBackPressed()
    {
        // Load the "MainMenu" scene when Back is pressed
        SceneManager.LoadScene("Menu"); // Replace "MainMenu" with your target scene name
    }
}
