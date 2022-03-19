using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
    public UnitState uState = UnitState.idle;
    public Units targetUnit = null;

    public float unitHP = 1.0f; // Health Point
    public float unitAD = 1.0f; // Attack Damage
    public float unitAS = 0.5f; // Attack Speed
    public float unitDP = 1.0f; // Defense Point
    public float unitMM = 1.0f; // Max Mana
    public float unitCM = 1.0f; // Current Mana
    public float unitMS = 1.0f; // Movement Speed
    public float unitAR = 1.0f; // Attack Range

    protected Vector2 dirVec;
    
    public Animator animator;
    public bool bHasSkillAnimation;
    float localScaleX;

    float tTimer = 1.0f;
    public float attackTimer = 0.0f;

    public enum UnitState
    {
        idle,
        attack,
        run,
        gethit,
        skill,
        death
    }
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();

        localScaleX = transform.localScale.x;
    }

    protected virtual void Update()
    {
        SetTarget();
        Move();
        Attack();
        CheckForFlipping();

        UpdateTimers();
    }
    
    public void SetTarget()
    {
        if (tTimer < 1.0f)
        {
            return;
        }
        tTimer = 0.0f;

        List<Units> targetList = null;
        if (transform.tag=="Heros")
        {
            targetList = BattleManager.instance.GetMonstersList();
        }
        else
        {
            targetList = BattleManager.instance.GetHerosList();
        }

        float dis = 1000;

        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i].uState != UnitState.death)
            {
                float tmpDis = ((Vector2)targetList[i].transform.localPosition - (Vector2)transform.localPosition).sqrMagnitude;
                if (tmpDis < dis)
                {
                    dis = tmpDis;
                    targetUnit = targetList[i];
                }
            }
        }
    }

    bool HasTarget()
    {
        if (targetUnit == null)
            return false;
        if (targetUnit.uState == UnitState.death)
            return false;

        return true;
    }

    void SetState(UnitState state)
    {
        uState = state;
        SetAnimation();
    }
    
    void SetAnimation()
    {
        switch (uState)
        {
            case UnitState.run:
                animator.SetBool("Run", true);
                break;
            case UnitState.gethit:
                animator.SetTrigger("GetHit");
                break;
            case UnitState.attack:
                animator.SetTrigger("Attack");
                break;
            case UnitState.death:
                animator.SetTrigger("Death");
                break;
            case UnitState.skill:
                animator.SetTrigger("Skill");
                break;
        }
    }

    void Move()
    {
        if (!HasTarget())
        {
            animator.SetBool("Run", false);
            return;
        }

        if (TargetInRange())
        {
            animator.SetBool("Run", false);
            return;
        }
        
        dirVec = ((Vector2)targetUnit.transform.localPosition - (Vector2)transform.localPosition).normalized;
        transform.position += (Vector3)dirVec * unitMS * Time.deltaTime;

        SetState(UnitState.run);
    }

    bool TargetInRange()
    {
        if (!HasTarget())
            return false;
        float dis = ((Vector2)targetUnit.transform.localPosition - (Vector2)transform.localPosition).sqrMagnitude;
        if (dis < unitAR)
            return true;

        return false;
    }

    void Attack()
    {
        if (TargetInRange())
        {
            if(attackTimer>unitAS)
            {
                Debug.Log("Attack");
                attackTimer = 0.0f;
                SetState(UnitState.attack);
            }
        }
    }
    

    void UpdateTimers()
    {
        tTimer += Time.deltaTime;
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            attackTimer += Time.deltaTime;
    }

    private void CheckForFlipping()
    {
        bool movingLeft = dirVec.x < 0;
        bool movingRight = dirVec.x > 0;

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
