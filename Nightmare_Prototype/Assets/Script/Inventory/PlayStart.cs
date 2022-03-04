using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStart : MonoBehaviour
{
    public Inventory inventory;
    public GameObject[] heroes;

    private void Start()
    {
        inventory = new Inventory();

        /*
        heroes = GameObject.FindGameObjectsWithTag("Heros");
        // Debug.Log(heroes.Length + " 개. 초기화해서 영웅들 인벤토리에 들어감");

        foreach (GameObject man in heroes)
        {
            IInventoryHero hero = man.GetComponent<IInventoryHero>();

            if (hero != null)
            {
                inventory.AddItem(hero);
                // Debug.Log(man.name + "들어감 - PlayStart");
            }
        }
        */
    }
}
