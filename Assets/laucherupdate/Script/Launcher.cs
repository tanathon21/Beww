using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

public class Launcher : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button checkUpdateButton;
    [SerializeField] Button updateNowButton;
    [SerializeField] Button notNowButton;
    [SerializeField] Text alertText;
    [SerializeField] GameObject updatePopup;

    private string currentVersion = "0.0.1"; // Current application version
    private string versionCheckURL = "https://manage.np-robotics.com/api/checkversion/index.php";
    private string downloadURL = "https://manage.np-robotics.com/dowload/index.php";

    void Start()
    {
        // Attach button click listeners
        startButton.onClick.AddListener(StartApplication);
        checkUpdateButton.onClick.AddListener(CheckForUpdate);
        updateNowButton.onClick.AddListener(UpdateNow);
        notNowButton.onClick.AddListener(NotNow);

        // Hide update-related UI elements at the start
        updateNowButton.gameObject.SetActive(false);
        notNowButton.gameObject.SetActive(false);
        updatePopup.SetActive(false);

        // Check for updates on startup if internet is reachable
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            CheckForUpdate();
        }
    }

    // Start the application without checking for updates
    void StartApplication()
    {
        Debug.Log("Starting application without internet connection.");
        SceneManager.LoadScene("Menu");
    }

    // Check for updates
    void CheckForUpdate()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            alertText.text = "No internet connection.";
            return;
        }

        StartCoroutine(CheckUpdateCoroutine());
    }

    // Coroutine to check the latest version
IEnumerator CheckUpdateCoroutine()
{
    Dictionary<string, string> formData = new Dictionary<string, string>
    {
        { "version", currentVersion }
    };

    using UnityWebRequest www = UnityWebRequest.Post(versionCheckURL, formData);
    yield return www.SendWebRequest();

    if (www.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError("Error: " + www.error);
        alertText.text = "Error checking for updates.";
    }
    else
    {
        string response = www.downloadHandler.text;
        Debug.Log("Response: " + response);

        if (response.StartsWith("Version:"))
        {
            string versionFromServer = response.Substring(8).Trim(); // Extract the version after "Version: "
            if (IsVersionNewer(currentVersion, versionFromServer))
            {
                Debug.Log("New version available: " + versionFromServer);
                ShowUpdatePrompt();
            }
            else
            {
                Debug.Log("Application is up-to-date.");
                alertText.text = "You have the latest version.";
            }
        }
        else if (response.Contains("SERVER: error, version not found"))
        {
            Debug.LogError("Version not found in the database.");
            alertText.text = "The specified version was not found. Please check the version number.";
            ShowUpdatePrompt();
        }
        else if (response.Contains("SERVER: error, please enter a valid version"))
        {
            Debug.LogError("Invalid version format.");
            alertText.text = "Please enter a valid version.";
        }
        else
        {
            Debug.Log("Unexpected response format.");
            alertText.text = "Could not check for updates. Please try again.";
        }
    }
}


    // Compare versions
bool IsVersionNewer(string currentVersion, string latestVersion)
{
    try
    {
        string[] currentParts = currentVersion.Split('.');
        string[] latestParts = latestVersion.Split('.');

        for (int i = 0; i < 3; i++)
        {
            int currentPart = int.Parse(currentParts[i]);
            int latestPart = int.Parse(latestParts[i]);

            if (latestPart > currentPart)
            {
                return true; // Latest version is newer
            }
            else if (latestPart < currentPart)
            {
                return false; // Current version is newer
            }
        }

        // If all parts are equal, the versions are the same
        return false;
    }
    catch (Exception e)
    {
        Debug.LogError("Version comparison failed: " + e.Message);
        return false;
    }
}

    // Show the update prompt
    void ShowUpdatePrompt()
    {
        alertText.text = "A new version is available!";
        updatePopup.SetActive(true);
        updateNowButton.gameObject.SetActive(true);
        notNowButton.gameObject.SetActive(true);
    }

    // Update now action
    void UpdateNow()
    {
        Debug.Log("Updating to the latest version...");
        Application.OpenURL(downloadURL);
        updatePopup.SetActive(false);
    }

    // Skip update action
    void NotNow()
    {
        Debug.Log("Skipping the update.");
        updatePopup.SetActive(false);
    }
}
