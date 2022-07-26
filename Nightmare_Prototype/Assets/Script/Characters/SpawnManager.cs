using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager: MonoBehaviour
{
    private static SpawnManager instance = null;

    public List<Enemy> EnemyList = new List<Enemy>();
    private HeroObject m_ObjectInfo = new HeroObject();

    public static SpawnManager Instance
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

    void Start()
    {
        // Test();
        // Enemy List node 정보에 맞게 설정

        // Scene이 시작되면 Enemy가 배치된다.
        SpawnEnemy();
    }

    void Test()
    {
        Enemy enemy1 = new Enemy();
        Enemy enemy2 = new Enemy();
        Enemy enemy3 = new Enemy();

        enemy1.SetEnemyType(0);
        enemy1.SetPosition(new Vector3(1, -1, 0));
        enemy1.SetGUID(18);

        enemy2.SetEnemyType(0);
        enemy2.SetPosition(new Vector3(4, 0, 0));
        enemy2.SetGUID(19);

        enemy3.SetEnemyType(0);
        enemy3.SetPosition(new Vector3(6, 3, 0));
        enemy3.SetGUID(19);

        EnemyList.Add(enemy1);
        EnemyList.Add(enemy2);
        EnemyList.Add(enemy3);
       
    }

    public void InitList(List<Enemy> Enemies)
    {
        this.EnemyList = Enemies;
    }

    void SpawnEnemy()
    {
        // Scene이 시작되면 Enemy가 배치된다.
        foreach (Enemy _enemy in EnemyList)
        {
            // Prefab 폴더에서 load 
            GameObject Prefab = Resources.Load("Prefabs/Monsters/" + _enemy.GetGUID()) as GameObject;
            GameObject EnemyObj = Instantiate(Prefab);

            // Load된 게임 Data에서 해당 ememy의 속성 가져옴
            m_ObjectInfo = (HeroObject)(MasterManager.Instance.LoadObject(_enemy.GetGUID()));
            if (m_ObjectInfo == null)
            {
                Debug.Log(" Data Load fail");
            }
            else
            {
                Debug.Log(" Enemy 배치: " + m_ObjectInfo.name);
            }

            Units EnemyUnits = EnemyObj.GetComponent<Units>();

            // Enemy의 위치 및 속성 적용
            EnemyObj.transform.position = _enemy.GetPosition();
            EnemyUnits.unitMaxHP = m_ObjectInfo.MaxHP; // Max Health Point
            EnemyUnits.unitAD = m_ObjectInfo.AttackDamage; // Attack Damage
            EnemyUnits.unitAS = m_ObjectInfo.AttackSpeed; // Attack Speed
            EnemyUnits.unitDP = m_ObjectInfo.DefensePoint; // Defense Point
            EnemyUnits.unitMM = m_ObjectInfo.MaxMP; // Max Mana
            EnemyUnits.unitMS = m_ObjectInfo.MoveSpeed; // Movement Speed
            EnemyUnits.unitAR = m_ObjectInfo.AttackRange; // Attack Range

        }
    }
}
