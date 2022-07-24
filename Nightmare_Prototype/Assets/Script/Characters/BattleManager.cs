using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Transform HerosObject;
    public Transform MonstersObject;

    public List<Units> HerosList;
    public List<Units> MonstersList;

    public static BattleManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
  
    void Start()    
    {

    }

    void Update()
    { 
    }
    public void StartBattle()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Monsters"))
        {
            MonstersList.Add(g.GetComponent<Units>());
        }
        foreach (var hero in HerosList)
        {

            hero.GetComponent<Units>().Initalize();
        }
        foreach (var monster in MonstersList)
        {

            monster.GetComponent<Units>().Initalize();
        }
    }
    
    public void EnrollHero(Units hero)
    {
        HerosList.Add(hero);
    }
    public List<Units> GetHerosList()
    {
        return HerosList;
    }
    public List<Units> GetMonstersList()
    {
        return MonstersList;
    }
}
