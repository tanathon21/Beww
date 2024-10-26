using UnityEngine;

public class SequentialClick : MonoBehaviour
{
    public GameObject objectA;
    public GameObject objectB;
    public GameObject objectC;
    public GameObject objectD;

    private int currentStep = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (currentStep == 0 && hit.transform.gameObject == objectA)
                {
                    Debug.Log("A Clicked");
                    currentStep++;
                }
                else if (currentStep == 1 && hit.transform.gameObject == objectB)
                {
                    Debug.Log("B Clicked");
                    currentStep++;
                }
                else if (currentStep == 2 && hit.transform.gameObject == objectC)
                {
                    Debug.Log("C Clicked");
                    currentStep++;
                }
                else if (currentStep == 3 && hit.transform.gameObject == objectD)
                {
                    Debug.Log("D Clicked");
                    // ทั้งหมดเสร็จสมบูรณ์
                    Debug.Log("Sequence Complete!");
                }
                else
                {
                    Debug.Log("Wrong order or object");
                }
            }
        }
    }
}
