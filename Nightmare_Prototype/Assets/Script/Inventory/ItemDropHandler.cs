using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public Inventory _Inventory;
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            
            IInventoryHero hero = eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().Hero;

            if (hero != null)
            { 
                _Inventory.RemoveItem(hero);
                hero.OnDrop(); 
                // Debug.Log("Drop Item");
            }
        }
    }
}