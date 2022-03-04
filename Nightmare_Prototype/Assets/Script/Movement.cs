using System.Collections;
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
    float acceptance_rad = 0.0f;
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
        if(bCanMove)
            Move();
    }
    public void TempAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.SetTrigger("Attack");
        }
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
            if (tempDist < acceptance_rad)
            {
                bCanMove = false;
                animator.SetBool("Run", false);

            }
            return;
        }
        animator.SetBool("Run", true);

        CheckForFlipping();
    }

    public void SetPath(ArrayList path, float Acceptance_Rad)
    {
        this.path = path;
        acceptance_rad = Acceptance_Rad;
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