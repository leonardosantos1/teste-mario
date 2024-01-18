using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private float horizontal;
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpQuantity;
    [SerializeField] private bool isJumping;
    [SerializeField] private int currentJumpQuantity;

    [SerializeField] private Animator _animator;


    void Start()
    {

        isJumping = false;
        jumpQuantity = 1;
        currentJumpQuantity = jumpQuantity;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal");


        FlipCharacter();

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if(currentJumpQuantity > 0)
            {
                JumpPlayer();
                isJumping = true;
                _animator.SetInteger("transition", 2);
                currentJumpQuantity -= 1;

            }
        }


        if (!isJumping)
        {
            if ((Input.GetAxis("Horizontal") > 0.1 || Input.GetAxis("Horizontal") < -0.1) )
            {

                _animator.SetInteger("transition", 1);

            }
            else
            {
                _animator.SetInteger("transition", 0);
            }
        }
      
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }


    void MovePlayer()
    {

        _rigidbody.velocity = new Vector2(horizontal * speed, _rigidbody.velocity.y);

    }


    void JumpPlayer()
    {
        _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    void FlipCharacter()
    {

        if(horizontal > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }else if( horizontal < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            currentJumpQuantity = jumpQuantity;
            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "PowerUp":
                Destroy(collision.gameObject);
                jumpQuantity = 2;
                break;
        }
    }
}
