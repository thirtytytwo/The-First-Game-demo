using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    PlayerMovement movement;
    Rigidbody2D rB;

    //动作编号
    int groundID;
    int moveID;
    int jumpID;
    int handingID;
    int crouchID;

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerMovement>();
        rB = GetComponentInParent<Rigidbody2D>();

        //在编号里获取string
        groundID = Animator.StringToHash("isOnGround");
        moveID = Animator.StringToHash("speed");
        jumpID = Animator.StringToHash("verticalVelocity");
        crouchID = Animator.StringToHash("isCrouching");
        handingID = Animator.StringToHash("isHanging");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(moveID, Math.Abs(movement.xVelocity));
        animator.SetBool(groundID, movement.isOnGround);
        animator.SetBool(handingID, movement.isHanding);
        animator.SetBool(crouchID, movement.isCrouch);
        animator.SetFloat(jumpID, rB.velocity.y);
    }
    public void StepAudio()
    {
        AudioManager.PlayFootStepAudio();
    }
    public void CrouchStepAudio()
    {
        AudioManager.PlayCrouchAudio();
    }
}
