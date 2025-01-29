using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavKeypad;

public class KeypadPlayerDetector : MonoBehaviour
{
    public Keypad keypad = null;
    private void OnTriggerEnter(Collider other)
    {
        if (keypad != null)
        {
            keypad.Unlock();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (keypad != null)
        {
            keypad.Lock();
        }
    }
}
