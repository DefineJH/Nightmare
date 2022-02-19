using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private Rigidbody2D body;
    private Vector2 axisMovement;
    public Animator animator;
    public bool bHasSkillAnimation;

    float timer;
    int waitingTime;

    float localScaleX;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        waitingTime = 2;

        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        localScaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        axisMovement.x = Input.GetAxis("Horizontal");
        axisMovement.y = Input.GetAxis("Vertical");

        // 임시로 H키를 눌렀을 때 GetHit
        if (Input.GetKeyDown(KeyCode.H) && !animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
        {
            animator.SetTrigger("GetHit");
        }

        // 임시로 J키를 눌렀을 때 Attack
        if (Input.GetKeyDown(KeyCode.J) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("Attack");
        }

        // 임시로 L키를 눌렀을 때 Death
        if (Input.GetKeyDown(KeyCode.L) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            animator.SetTrigger("Death");
            //float delay = 0;
            //Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
        }

        if (bHasSkillAnimation)
        {
            // 임시로 K키를 눌렀을 때 Skill
            if (Input.GetKeyDown(KeyCode.K) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Skill"))
            {
                animator.SetTrigger("Skill");
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (axisMovement.x != 0 || axisMovement.y != 0)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        body.velocity = axisMovement.normalized * speed;
        CheckForFlipping();
    }
    private void CheckForFlipping()
    {
        bool movingLeft = axisMovement.x < 0;
        bool movingRight = axisMovement.x > 0;

        if (movingLeft)
        {
            transform.localScale = new Vector3(-localScaleX, transform.localScale.y);
        }

        if (movingRight)
        {
            transform.localScale = new Vector3(localScaleX, transform.localScale.y);
        }
    }
}