﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 0.000003f;

    private Rigidbody2D body;
    private Vector2 axisMovement;
    public Animator animator;
    public bool bHasSkillAnimation;

    float timer;
    int waitingTime;

    bool bCanMove = false;
    int curIdx = 0;
    int maxIdx = 0;
    Vector2 moveDir = new Vector2();
    ArrayList path;

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
    }

    private void FixedUpdate()
    {
        if(bCanMove)
            Move();
    }
    public void TempAttack()
    {
        
    }
    private void Move()
    {
        Vector2 curPos = gameObject.transform.position;
        float dist = Vector2.Distance(curPos, (path[path.Count - 1] as Path.Node).pos);
        if (curIdx != maxIdx - 1 )
        {
            moveDir = (path[curIdx + 1] as Path.Node).pos - (path[curIdx] as Path.Node).pos;
            gameObject.transform.Translate(moveDir * speed * Time.deltaTime);
            float tempDist = Vector2.Distance((path[curIdx + 1] as Path.Node).pos, gameObject.transform.position);
            if (tempDist < 0.1f)
            {
                curIdx++;
            }
        }
        else
        {
            moveDir = (path[curIdx + 1] as Path.Node).pos - (path[curIdx] as Path.Node).pos;
            gameObject.transform.Translate(moveDir.normalized * speed * Time.deltaTime);
            float tempDist = Vector2.Distance((path[curIdx + 1] as Path.Node).pos, gameObject.transform.position);
            if (tempDist <= GetComponent<Units>().unitAR)
            {
                bCanMove = false;
                animator.SetBool("Run", false);
            }
            return;
        }
        animator.SetBool("Run", true);

        CheckForFlipping();
    }
    public void StopMovement()
    {
        bCanMove = false;
    }
    public void SetPath(ArrayList path)
    {
        this.path = path;
        curIdx = 0;
        maxIdx = path.Count - 1;
        bCanMove = true;
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