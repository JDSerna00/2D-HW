﻿using System;
using System.Collections;
using UnityEngine;

namespace ClearSky
{
    public class SimplePlayerController : MonoBehaviour
    {
        public float movePower = 10f;
        public float speed = 5f;
        public float jumpPower = 15f; //Set Gravity Scale in Rigidbody2D Component to 5

        private Rigidbody2D rb;
        private Animator anim;
        Vector3 movement;
        public Joystick joystick;
        private int direction = 1;
        bool isJumping = false;
        private bool alive = true;
        public bool isDead = false;

        public Transform groundCheck;
        public GameObject DeadPanel; 
        public LayerMask groundLayer;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            Restart();
            if (alive)
            {
                Hurt();
                Die();
                Attack();
                Run();
                CheckGrounded();
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            anim.SetBool("isJump", false);
            isJumping = false;
        }


        void Run()
        {
            float horizontalInput = joystick.Horizontal;
            Vector3 move = new Vector3(horizontalInput, 0, 0) * speed;
            anim.SetBool("isRun", false);


            if (horizontalInput < 0)
            {
                direction = -1;
                move = Vector3.left;

                transform.localScale = new Vector3(direction, 1, 1);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            if (horizontalInput > 0)
            {
                direction = 1;
                move = Vector3.right;

                transform.localScale = new Vector3(direction, 1, 1);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

            }
            transform.position += move * movePower * Time.deltaTime;
        }
        public void Jump()
        {
            if (!isJumping)
            {
                anim.SetBool("isJump", true);
                rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
                isJumping = true;
            }
        }
        void CheckGrounded()
        {           
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);

            if (hit.collider != null)
            {
                anim.SetBool("isJump", false);
                isJumping = false;
            }
        }
        void Attack()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                anim.SetTrigger("attack");
            }
        }
        void Hurt()
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.SetTrigger("hurt");
                if (direction == 1)
                    rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
            }
        }
        void Die()
        {
            if (isDead)
            {
                anim.SetTrigger("die");
                alive = false;
                StartCoroutine(ActivateDeadPanelWithDelay());
            }
        }

        private IEnumerator ActivateDeadPanelWithDelay()
        {
            yield return new WaitForSeconds(1);
            DeadPanel.SetActive(true);
        }

        void Restart()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                anim.SetTrigger("idle");
                alive = true;
            }
        }
    }
}