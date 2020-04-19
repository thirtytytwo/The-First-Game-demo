﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    int playerLayer;
    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == playerLayer)
        {
            Debug.Log("player won");

            GameManager.PlayerWin();

            AudioManager.PlayerWin();
        }
    }
}
