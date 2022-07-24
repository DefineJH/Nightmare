using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ObjectType
{
    HeroData,
    MonsterData,
    Items,
}
public class GeneralObjects 
{
    public uint guid;
    public ObjectType type;
}

public class HeroObject : GeneralObjects
{
    public string name;
    public float MaxHP;
    public float MaxMP;
    public float AttackDamage;
    public float AttackSpeed;
    public float AttackRange;
    public float DefensePoint;
    public float MoveSpeed;
}
