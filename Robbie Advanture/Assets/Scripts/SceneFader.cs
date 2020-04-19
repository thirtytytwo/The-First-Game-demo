using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    Animator animator;
    int fadeID;
    void Start()
    {
        animator = GetComponent<Animator>();
        fadeID = Animator.StringToHash("Fade");

        GameManager.RegisterSceneFader(this);
    }
    public void FadeOut()
    {
        animator.SetTrigger(fadeID);
    }

}
