using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform POVLocation; // CameraPosition

    void Update()
    {
        transform.position = POVLocation.position;
    }
}
