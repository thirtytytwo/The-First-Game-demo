using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;

    int openID;
    void Start()
    {
        animator = GetComponent<Animator>();
        openID = Animator.StringToHash("Open");

        GameManager.RegisterDoor(this);
    }
    public void Open()
    {
        AudioManager.DoorAudio();
        animator.SetTrigger(openID);
    }

}
