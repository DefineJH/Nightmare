using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public Transform HerosObject;
    public Transform MonstersObject;

    public List<Units> HerosList;
    public List<Units> MonstersList;

    public static BattleManager instance = null;

    public Text finishTxt = null;

    int herosCount = 0;
    int monstersCount = 0;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
  
    void Start()    
    {
        finishTxt = GameObject.Find("finishTxt").GetComponent<Text>();
        if(finishTxt)
        {
            finishTxt.gameObject.SetActive(false);
        }
    }

    void Update()
    { 

    }
    public void StartBattle()
    {
        herosCount = 0;
        monstersCount = 0;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Monsters"))
        {
            MonstersList.Add(g.GetComponent<Units>());
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Heros"))
        {
            if(g.active)
            {
                HerosList.Add(g.GetComponent<Units>());
            }
        }
        foreach (var hero in HerosList)
        {
            hero.GetComponent<Units>().Initalize();
        }
        foreach (var monster in MonstersList)
        {
            monster.GetComponent<Units>().Initalize();
        }
        herosCount = HerosList.Count;
        monstersCount = MonstersList.Count;
    }
    /// <summary>
    /// Parameter - 0(Monster) , 1(Hero)
    /// </summary>
    /// <param name="type"></param>
    public void ProcessDead(uint type)
    {
        if(type == 0)
        {
            monstersCount--;
            if (monstersCount == 0)
                ShowFinishTxt(true);

        }
        else if (type == 1)
        {
            herosCount--;
            if (herosCount == 0)
                ShowFinishTxt(false);
        }
    }

    void ShowFinishTxt(bool isWin)
    {
        finishTxt.gameObject.SetActive(true);
        string toAdd = isWin ? "\n(Win)" : "\n(Lose)";
        finishTxt.text += toAdd;
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
