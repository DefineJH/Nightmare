using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Start : MonoBehaviour
{
    public void StartBattle()
    {
        gameObject.SetActive(false);
        BattleManager.instance.StartBattle();
    }
}
