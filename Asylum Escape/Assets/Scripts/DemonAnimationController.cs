using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAnimationController : MonoBehaviour
{
    [SerializeField] private CreatureModelSwitcher _modelSwitcher;
    [SerializeField] private GameObject _demon;

    private Animator animator;
    private CreatureAI _creatureAI;


    void Start()
    {
        animator = GetComponent<Animator>();
        _creatureAI = _demon.GetComponent<CreatureAI>();
    }

    void Update()
    {
        animator.SetBool("is_walking", _creatureAI.isWalking);
        animator.SetBool("is_attacking", _creatureAI.isAttacking);

    }
}

