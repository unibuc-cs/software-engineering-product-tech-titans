using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    public GameObject neighbour = null;
    public bool isOpen = false;
    [SerializeField]
    private bool isRotatingDoor = true;
    [SerializeField]
    private float speed = 1f;
    [Header("Rotation Config")]
    [SerializeField]
    private float rotationAmount = 90f;
    [SerializeField]
    private Vector3 inwardDirection = Vector3.forward;
    private Vector3 startRotation;
    public bool isLocked = false;

    public AudioClip doorOpeningSound;
    public AudioClip doorLockedSound;

    private Coroutine animationCoroutine;

    private void Awake()
    {
        startRotation = transform.rotation.eulerAngles;
        Transform parent = transform.parent;

        if (parent != null)
        {
            
            foreach (Transform sibling in parent)
            {
                
                if (sibling != transform && !sibling.gameObject.name.ToLower().EndsWith("frame") )
                {
                    neighbour = sibling.gameObject;

                }
            }
        }


    }

    public void Open(Vector3 userPosition)
    {
        if (!isOpen && !isLocked )
        {
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);
            Vector3 worldInteriorDirection = transform.TransformDirection(inwardDirection);
            Vector3 userDirection = (userPosition - transform.position).normalized;
            float dot = Vector3.Dot(worldInteriorDirection, userDirection);
           
            if (isRotatingDoor)
            {
                animationCoroutine = StartCoroutine(DoRotationOpen(dot > 0)); 
            }

            if (neighbour != null)
                neighbour.GetComponent<Door>().Open(userPosition);
                    
        }
    }

    public void PlayDoorLockedSound()
    {
        AudioManager.Instance.PlaySFX(doorLockedSound);
    }

    public void Lock()
    {
        isLocked = true;
        if(neighbour != null)
            neighbour.GetComponent<Door>().isLocked = true;
    }
    public void unLock()
    {
        AudioManager.Instance.PlaySFX(doorOpeningSound);
        isLocked = false;
        if (neighbour != null)
            neighbour.GetComponent<Door>().isLocked = false;
    }

    private IEnumerator DoRotationOpen(bool openInward)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

     

        if (openInward)
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.eulerAngles.y - rotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.eulerAngles.y + rotationAmount, 0));
        }

        isOpen = true;
        float time = 0f;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null; // Wait for the next frame
            time += Time.deltaTime * speed;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);
            if (isRotatingDoor)
            {
                animationCoroutine = StartCoroutine(DoRotationClose());
            }
            if (neighbour != null)
                neighbour.GetComponent<Door>().Close();

        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(this.startRotation);

        isOpen = false;
        float time = 0f;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}
