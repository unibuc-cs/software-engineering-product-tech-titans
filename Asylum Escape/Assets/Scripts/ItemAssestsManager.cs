using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssestsManager : MonoBehaviour
{
    public static ItemAssestsManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    public Sprite Null;
    [SerializeField]
    public Sprite keySprite;
    [SerializeField]
    public Sprite escapeKeySprite;
    [SerializeField]
    public Sprite batterySprite;
}
