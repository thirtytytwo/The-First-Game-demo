using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    int playerLayer;

    public GameObject explosionVFX;
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        GameManager.RegisterOrb(this);

        DontDestroyOnLoad(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == playerLayer)
        {
            gameObject.SetActive(false);
            Instantiate(explosionVFX, transform.position, transform.rotation);

            AudioManager.SelectedOrb();

            GameManager.PlayerGrabbedOrb(this);
        }
    }


}
