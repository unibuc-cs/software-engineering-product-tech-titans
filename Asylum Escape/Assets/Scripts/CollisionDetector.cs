using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public Door dor = null;
    private void OnTriggerEnter(Collider other)
    {
        if(dor != null)
        {
            dor.Lock();
            dor.Close();
            Destroy(gameObject);
        }
        
    }

}
