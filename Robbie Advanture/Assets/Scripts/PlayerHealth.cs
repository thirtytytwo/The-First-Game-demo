using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject playerDeathVFX;
    
    int trapLayer;
    void Start()
    {
        trapLayer = LayerMask.NameToLayer("Spike");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == trapLayer)
        {
            Instantiate(playerDeathVFX, transform.position, transform.rotation);
            
            AudioManager.PlayerDieAudio();

            gameObject.SetActive(false);

            GameManager.PlayerDie();
        }
    }
}  