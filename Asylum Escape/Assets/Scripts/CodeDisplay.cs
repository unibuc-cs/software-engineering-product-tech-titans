using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeDisplay : MonoBehaviour
{
    public void show()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(ExecuteAfterDelay(5f));
    }
    IEnumerator ExecuteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }
}
