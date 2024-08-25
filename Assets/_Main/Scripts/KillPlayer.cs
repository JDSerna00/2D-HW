using ClearSky;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public Animator animator;
    public SimplePlayerController controller;

    private void Start()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"ahi ta {collision}");
            animator.SetTrigger("Die");
            controller = collision.GetComponent<SimplePlayerController>();
            controller.isDead = true;
        }
    }
}
