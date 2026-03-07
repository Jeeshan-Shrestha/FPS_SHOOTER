using System;
using UnityEngine;

public class AnimatingCube : Interactable
{

    private Animator animator;
    private String startMessage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        startMessage = promptMessage;
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            promptMessage = startMessage;
        }else
        {
            promptMessage = "Animating..";
        }
    }

    protected override void Interact()
    {
        animator.Play("SpinningCubeAnimation");
    }
    
}
