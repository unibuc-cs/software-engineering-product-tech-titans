using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjectOnDoorOpen : MonoBehaviour
{
    public GameObject door;        
    public GameObject objectToHide; 

    private bool isDoorOpen = false; 

    void Update()
    {
        if (door != null && CheckIfDoorIsOpen() && !isDoorOpen)
        {
            isDoorOpen = true;
            HideObject();
        }
    }

    
    void HideObject()
    {
        if (objectToHide != null)
        {
            objectToHide.SetActive(false); 
            Debug.Log($"{objectToHide.name} a fost ascuns deoarece usa s-a deschis!");
        }
    }

    
    bool CheckIfDoorIsOpen()
    {
        Animator doorAnimator = door.GetComponent<Animator>();
        if (doorAnimator != null)
        {
            return doorAnimator.GetBool("isOpen"); 
        }

        return false;
    }
}
