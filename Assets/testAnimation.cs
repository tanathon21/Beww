using UnityEngine;
using System.Collections;

public class PlayAnimationsOnClick : MonoBehaviour
{
    // Reference to the Animator components of all objects
    public Animator animatorA;
    public GameObject objectToDestroy;
    public float delayBeforeDestroy = 5f;
    //public Animator animatorB;
    //public Animator animatorC;
    //public Animator animatorD;

    // Name of the trigger parameter in the Animator
    public string triggerName = "A1";
    
    void OnMouseDown()
    {
        // Set trigger for all objects when this object is clicked
        if (animatorA != null) animatorA.SetTrigger(triggerName);
        StartCoroutine(WaitAndDestroy());
        //if (animatorB != null) animatorB.SetTrigger(triggerName);
        //if (animatorC != null) animatorC.SetTrigger(triggerName);
        //if (animatorD != null) animatorD.SetTrigger(triggerName);
    }
        private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(delayBeforeDestroy);
        Destroy(objectToDestroy);
    }
}
