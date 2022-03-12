using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvent : MonoBehaviour
{
    public Inventory inventory;
    private GameObject target;
    
    void Update()
    {
        // 마우스 우클릭시 맵내의 영웅 slot으로 store
        if (Input.GetMouseButtonUp(1))
        {
            CastRay();

            if (target != null)
            {
                IInventoryHero hero = target.GetComponent<IInventoryHero>();
                inventory.AddItem(hero);
                Debug.Log(hero.Name + "마우스 클릭으로 들어감");
            }
        }
    }

    void CastRay()
    {
        target = null;

        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        // hit 되었다면 여기에서 실행된다
        if (hit.collider != null)
        {
            // Debug.Log (hit.collider.name);
            target = hit.collider.gameObject;  //히트 된 게임 오브젝트를 타겟으로 지정

        }
    }
}
