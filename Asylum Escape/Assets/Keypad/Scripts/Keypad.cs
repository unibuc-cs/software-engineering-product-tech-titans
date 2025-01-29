using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NavKeypad
{
    public class Keypad : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;
        [SerializeField] private UnityEvent onAccessDenied;
        [Header("Combination Code (9 Numbers Max)")]
        [SerializeField] private int keypadCombo = 12345;

        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "Granted";
        [SerializeField] private string accessDeniedText = "Denied";
        [SerializeField] private string playerTag = "Player"; 

        [Header("Visuals")]
        [SerializeField] private float displayResultTime = 1f;
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f;
        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); //orangy
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f); //red
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f); //greenish
        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;
        [SerializeField] private AudioClip accessDeniedSfx;
        [SerializeField] private AudioClip accessGrantedSfx;
        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;
        [SerializeField] private TMP_Text keypadDisplayText;
        [SerializeField] private AudioSource audioSource;

        private string currentInput;
        private bool displayingResult = false;
        private bool accessWasGranted = false;
        private bool isPlayerNearby = false; 
        public bool isLocked = true;

        private void Awake()
        {
            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
        }

        private void Update()
        {
            if (displayingResult || accessWasGranted || isLocked) return;
            else print(isLocked);

            if (Input.GetKeyDown(KeyCode.Alpha0)) AddInput("0");
            if (Input.GetKeyDown(KeyCode.Alpha1)) AddInput("1");
            if (Input.GetKeyDown(KeyCode.Alpha2)) AddInput("2");
            if (Input.GetKeyDown(KeyCode.Alpha3)) AddInput("3");
            if (Input.GetKeyDown(KeyCode.Alpha4)) AddInput("4");
            if (Input.GetKeyDown(KeyCode.Alpha5)) AddInput("5");
            if (Input.GetKeyDown(KeyCode.Alpha6)) AddInput("6");
            if (Input.GetKeyDown(KeyCode.Alpha7)) AddInput("7");
            if (Input.GetKeyDown(KeyCode.Alpha8)) AddInput("8");
            if (Input.GetKeyDown(KeyCode.Alpha9)) AddInput("9");

            if (Input.GetKeyDown(KeyCode.Return)) AddInput("enter");
            if (Input.GetKeyDown(KeyCode.Backspace)) ClearInput();
        }

        public void Lock()
        {
            isLocked = true;
            Debug.Log("Keypad is now locked.");
        }

        public void Unlock()
        {
            isLocked = false;
            Debug.Log("Keypad is now unlocked.");
        }
        public void openDoor()
        {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, 2f);
            foreach (Collider collider in colliderArray)
            {

                if (collider.TryGetComponent(out Door door))
                {
                    Debug.Log($"Name door:{door.name}");
                    if (!door.isOpen)
                    {
                        door.unLock();
                    }
                }

            }
        }

        public void AddInput(string input)
        {
            if (isLocked) return;

            Debug.Log(input);
            audioSource.PlayOneShot(buttonClickedSfx);

            if (displayingResult || accessWasGranted) return;

            switch (input)
            {
                case "enter":
                    CheckCombo();
                    break;
                default:
                    if (currentInput != null && currentInput.Length == 9) return;
                    currentInput += input;
                    keypadDisplayText.text = currentInput;
                    break;
            }
        }

        public void CheckCombo()
        {
            if (int.TryParse(currentInput, out var currentKombo))
            {
                bool granted = currentKombo == keypadCombo;
                if (!displayingResult)
                {
                    StartCoroutine(DisplayResultRoutine(granted));
                }
            }
            else
            {
                Debug.LogWarning("Couldn't process input.");
            }
        }

        private IEnumerator DisplayResultRoutine(bool granted)
        {
            displayingResult = true;

            if (granted) AccessGranted();
            else AccessDenied();

            yield return new WaitForSeconds(displayResultTime);
            displayingResult = false;
            if (granted) yield break;
            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
        }

        private void AccessDenied()
        {
            keypadDisplayText.text = accessDeniedText;
            onAccessDenied?.Invoke();
            panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity);
            audioSource.PlayOneShot(accessDeniedSfx);
        }

        private void ClearInput()
        {
            currentInput = "";
            keypadDisplayText.text = currentInput;
        }

        private void AccessGranted()
        {
            accessWasGranted = true;
            keypadDisplayText.text = accessGrantedText;
            onAccessGranted?.Invoke();
            panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);
            audioSource.PlayOneShot(accessGrantedSfx);
        }
    }
}
