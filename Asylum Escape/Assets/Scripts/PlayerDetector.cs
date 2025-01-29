using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public CodeDisplay code = null;
    private void OnTriggerEnter(Collider other)
    {
        if (code != null)
        {
            code.show();
        }

    }
}
