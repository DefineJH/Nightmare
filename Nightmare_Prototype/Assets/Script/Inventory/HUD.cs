using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Inventory inventory;
    public GameObject[] heroes;

    public void SetInventory (Inventory inventory)
    {
        this.inventory = inventory;
    }

    void Start()
    {
        inventory.ItemAdded += InventoryScript_ItemAdded;
        inventory.ItemRemoved += InventoryScript_ItemRemoved;

        heroes = GameObject.FindGameObjectsWithTag("Heros");
        // Debug.Log(heroes.Length + " 개. 초기화해서 영웅들 인벤토리에 들어감");

        foreach (GameObject man in heroes)
        {
            IInventoryHero hero = man.GetComponent<IInventoryHero>();

            if (hero != null)
            {
                inventory.AddItem(hero);
                // Debug.Log(man.name + "들어감 - HUD");
            }
        }
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        // Debug.Log("InventoryScript_ItemAdded 들어왔습니다");
        Transform inventoryPanel = transform.Find("Inventory");

        foreach (Transform slot in inventoryPanel)
        {
            // Border... Image                              
            Transform imageTransform = slot.GetChild(0).GetChild(0);
            Image image = imageTransform.GetComponent<Image>();
            ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

            // Found the empty slot
            if (!image.enabled)
            {
                image.enabled = true;
                image.sprite = e.Hero.Image;

                // Store a reference to the item
                itemDragHandler.Hero = e.Hero;

                // Debug.Log(e.Hero.Name +" 슬롯에 이미지 들어감");
                break;
            }
        }
    }

    private void InventoryScript_ItemRemoved(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("Inventory");

        foreach (Transform slot in inventoryPanel)
        {
            Transform imageTransform = slot.GetChild(0).GetChild(0);
            Image image = imageTransform.GetComponent<Image>();
            ItemDragHandler itemDragHandler = imageTransform.GetComponent<ItemDragHandler>();

            // We found the item in the UI
            if (itemDragHandler.Hero.Equals(e.Hero))
            {
                image.enabled = false;
                image.sprite = null;
                itemDragHandler.Hero = null;
                Debug.Log(e.Hero.Name +" 슬롯에 이미지 빠짐");
                break;
            }
            else
            {
                continue;
            }
        }
    }
}
