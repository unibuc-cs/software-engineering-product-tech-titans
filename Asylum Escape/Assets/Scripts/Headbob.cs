using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Headbob : MonoBehaviour
{
    [SerializeField] Player playerObject;

    [Range(0.001f, 0.01f)]
    public float Amount = 0.002f;
    [Range(1.0f, 40.0f)]
    public float Frequency = 10.0f;
    [Range(10f, 100f)]
    public float Smooth = 10.0f;

    Vector3 StartPos;
    private float StartFrequency;

    void Start()
    {
        StartFrequency = Frequency;
        StartPos = transform.localPosition;
    }

    void Update()
    {
        if (playerObject.GetComponent<PlayerActions>().isCrouching == true)
        {
            Frequency = StartFrequency / 2.0f;
        } else
        {
            Frequency = StartFrequency;
        }

        CheckForHeadbobTrigger();
        CheckForStopHeadBob();
    }

    private void CheckForHeadbobTrigger()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if(inputMagnitude > 0)
        {
            StartHeadBob();
        }
    }

    private Vector3 StartHeadBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);
        pos.y += Mathf.Lerp(pos.y, Mathf.Cos(Time.time * Frequency / 2.0f) * Amount * 1.6f, Smooth * Time.deltaTime);
        transform.localPosition += pos;

        return pos;
    }

    private void CheckForStopHeadBob()
    {
        if (transform.localPosition == StartPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, Time.deltaTime);
    }

}
