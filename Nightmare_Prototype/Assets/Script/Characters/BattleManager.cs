﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Transform HerosObject;
    public Transform MonstersObject;

    public List<Units> HerosList;
    public List<Units> MonstersList;

    public static BattleManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()    
    {
        InitLists();
        Debug.Log("Heros: " + HerosList.Count + ", Monsters: " + MonstersList.Count);
    }

    void Update()
    { 
        
    }

    void InitLists()
    {
         for(int i =0; i< HerosObject.childCount; i++)
        {
            HerosList.Add(HerosObject.GetChild(i).GetComponent<Units>());
        }
        for (int i = 0; i < MonstersObject.childCount; i++)
        {
            MonstersList.Add(MonstersObject.GetChild(i).GetComponent<Units>());
        }
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