using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Units : MonoBehaviour
{
    public UnitState uState = UnitState.idle;
    public Units targetUnit = null;

    public float unitMaxHP = 100.0f; // Max Health Point
    public float unitCurHP = 100.0f; // Current Health Point
    public float unitAD = 10.0f; // Attack Damage
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

    public GameObject UnitUIObject;
    GameObject UnitUI;
    Slider hpBar;
    public Vector3 uiOffset;

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

        UnitUI = Instantiate(UnitUIObject, transform.position, Quaternion.identity);
        UnitUI.transform.parent = transform;
        UnitUI.transform.position += uiOffset;
        hpBar = UnitUI.transform.GetChild(0).GetComponent<Slider>();

        localScaleX = transform.localScale.x;
    }

    protected virtual void Update()
    {
        if(uState != UnitState.death)
        {
            SetTarget();
            Move();
            Attack();
            CheckForFlipping();

            UpdateTimers();
            UpdateUI();
        }
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
                targetUnit.GetDamage(unitAD);
            }
        }
    }

    public void GetDamage(float damage)
    {
        if (damage > unitDP)
            unitCurHP -= damage - unitDP;
        else // 방어력이 데미지 보다 높으면 1데미지만 
            unitCurHP -= 1;

        if(unitCurHP <= 0)
        {
            SetState(UnitState.death);
            UnitUI.SetActive(false);
        }
    }
    

    private void UpdateTimers()
    {
        tTimer += Time.deltaTime;
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            attackTimer += Time.deltaTime;
    }

    private void UpdateUI()
    {
        hpBar.value = unitCurHP / unitMaxHP;
    }

    private void CheckForFlipping()
    {
        bool movingLeft = dirVec.x < 0;
        bool movingRight = dirVec.x > 0;

        if (movingLeft)
        {
            transform.localScale = new Vector3(-localScaleX, transform.localScale.y);
            UnitUI.transform.rotation = Quaternion.Euler(new Vector3(UnitUI.transform.rotation.x, 180, UnitUI.transform.rotation.z));
        }

        if (movingRight)
        {
            transform.localScale = new Vector3(localScaleX, transform.localScale.y);
            UnitUI.transform.rotation = Quaternion.Euler(new Vector3(UnitUI.transform.rotation.x, 0, UnitUI.transform.rotation.z));
        }
    }
}
