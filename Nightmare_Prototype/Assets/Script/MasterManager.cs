using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MasterManager : MonoBehaviour
{
    private static MasterManager instance = null;

    private static Dictionary<uint, GeneralObjects> ObjectsEnrolled;
    public static MasterManager Instance
    {
        get
        {
            if (!instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        LoadGameData();
    }

 
    void LoadGameData()
    {
        LoadHeroData();
        LoadMonsterData();
    }
    void LoadHeroData()
    {
        CSVImporter csvImp = new CSVImporter();
        csvImp.OpenFile("Data/Heros_values");
        csvImp.ReadHeader();
        string line = csvImp.Readline();
        while (line != null)
        {
            string[] elems = line.Split(',');

            HeroObject hero = new HeroObject();
            hero.guid = uint.Parse(elems[0]);
            hero.name = elems[1];
            hero.MaxHP = float.Parse(elems[2]);
            hero.AttackDamage = float.Parse(elems[3]);
            hero.AttackSpeed = float.Parse(elems[4]);
            hero.DefensePoint = float.Parse(elems[5]);
            hero.MaxMP = float.Parse(elems[6]);
            hero.MoveSpeed = float.Parse(elems[7]);
            hero.AttackRange = float.Parse(elems[8]);

            ObjectsEnrolled.Add(hero.guid, hero);
            line = csvImp.Readline();
        }
    }
    void LoadMonsterData()
    {
        CSVImporter csvImp = new CSVImporter();
        csvImp.OpenFile("Data/Monsters_values");
        csvImp.ReadHeader();
        string line = csvImp.Readline();
        while (line != null)
        {
            string[] elems = line.Split(',');

            HeroObject hero = new HeroObject();
            hero.guid = uint.Parse(elems[0]);
            hero.name = elems[1];
            hero.MaxHP = float.Parse(elems[2]);
            hero.AttackDamage = float.Parse(elems[3]);
            hero.AttackSpeed = float.Parse(elems[4]);
            hero.DefensePoint = float.Parse(elems[5]);
            hero.MaxMP = float.Parse(elems[6]);
            hero.MoveSpeed = float.Parse(elems[7]);
            hero.AttackRange = float.Parse(elems[8]);

            ObjectsEnrolled.Add(hero.guid, hero);
            line = csvImp.Readline();
        }
    }
    public GeneralObjects LoadObject(uint guid)
    {
        if (!ObjectsEnrolled.ContainsKey(guid))
            throw new System.Exception("There's no object enrolled");
        else
            return ObjectsEnrolled[guid];
    }
}
